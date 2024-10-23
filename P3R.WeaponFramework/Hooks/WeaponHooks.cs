using P3R.WeaponFramework.Hooks.Models;
using P3R.WeaponFramework.Hooks.Services;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;
using static P3R.WeaponFramework.Types.Characters;
using System.Text;
using P3R.WeaponFramework.Extensions;
using P3R.WeaponFramework.Utils;

namespace P3R.WeaponFramework.Hooks;

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
internal unsafe class WeaponHooks
{
    private const string Separator = " || ";

    [Function(CallingConventions.Microsoft)]
    private delegate void UAppCharacterComp_Update(UAppCharacterComp* comp);
    private UAppCharacterComp_Update? characterCompUpdate;

    [Function(Register.rcx, Register.rax, true)]
    private delegate void SetWeaponId(UAppCharacterComp* comp);
    private IReverseWrapper<SetWeaponId>? setWeaponWrapper;
    private IAsmHook? setWeaponHook;

    private readonly IUnreal unreal;
    private readonly IUObjects uobjects;
    //private readonly ICommonMethods common;
    private readonly IMemoryMethods memory;
    private WeaponRegistry registry;
    private WeaponOverridesRegistry overrides;
    private readonly WeaponDescService weaponDesc;
    private WeaponRedirectService redirects;
    private ItemEquipHooks itemEquip;


    private List<FWeaponItemList> ModifiedWeapons = [];
    /// <summary>
    /// Checks to see if any overrides point to a weapon created with Weapon Framework and overwrites the original data.
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="applyOverride">Action to apply the data found in <paramref name="weapon"/> to the weapons it overrides (if any).</param>
    private void CheckForOverrides(Weapon weapon, Action<Weapon,Weapon> applyOverride)
    {
        // First we get every override that points to the new weapon
        if (overrides.TryGetWeaponOverridesTo(weapon, out var overridenWeapons))
        {
            // If the above is true, iterate through each override
            foreach (var weaponItem in overridenWeapons)
            {
                /// Apply the name and description (see below)
                applyOverride(weaponItem, weapon);
            }
        }
    }

    public WeaponHooks(
        IUnreal unreal,
        IUObjects uobjects,
        IMemoryMethods memory,
        WeaponRegistry registry,
        WeaponOverridesRegistry overrides,
        WeaponDescService weaponDesc,
        WeaponRedirectService redirects,
        ItemEquipHooks itemEquip)
    {
        this.unreal = unreal;
        this.uobjects = uobjects;
        this.memory = memory;
        this.registry = registry;
        this.overrides = overrides;
        this.weaponDesc = weaponDesc;
        this.redirects = redirects;
        this.itemEquip = itemEquip;


        uobjects.FindObject("DatItemWeaponDataAsset", SetWeaponData);

        

        ScanHooks.Add(
            nameof(UAppCharacterComp_Update),
            "48 8B C4 48 89 48 ?? 55 41 54 48 8D 68 ?? 48 81 EC 48 01 00 00",
            (hooks, result) =>
            {
                this.characterCompUpdate = hooks.CreateWrapper<UAppCharacterComp_Update>(result, out _);

                // UAppCharacterComp::Update + 0x254 is FF in release (call to vtable), but 75 in episode aigis (jump)
                var setWeaponAddress = result + (*(byte*)(result + 0x254) == 0x75 ? 0x183 : 0x255);
                var setWeaponPatch = new string[]
                {
                    "use64",
                    Utilities.PushCallerRegisters,
                    hooks.Utilities.GetAbsoluteCallMnemonics(this.SetWeaponIdImpl, out this.setWeaponWrapper),
                    Utilities.PopCallerRegisters,
                    "mov rax, qword [rcx]",
                };

                this.setWeaponHook = hooks.CreateAsmHook(setWeaponPatch, setWeaponAddress).Activate();
            });
    }

    private static void LogItem(FWeaponItemList logItem)
    {
        Log.Debug($"New WeaponItem\n" +
            $"WeaponType: {logItem.WeaponType} " +
            Separator +
            $"EquipFlag: {logItem.EquipID}" +
            $"\n" +
            $"Attack: {logItem.Attack}" +
            Separator +
            $"Accuracy: {logItem.Accuracy}");
    }
    private static void LogItem(FWeaponItemList* logPtr)
    {
        var logItem = *logPtr;
        LogItem(logItem);
    }
    private void SetWeaponData(UnrealObject obj)
    {
        //Log.Verbose("WeaponTable found");
        weaponDesc.Prime(); // Get descriptions for given episode
        var weaponItemList = (UWeaponItemListTable*)obj.Self;
        var activeWeapons = registry.GetActiveWeapons();
        //var managedItemList = new TWeaponItemListTable(memory, &weaponItemList->Data);
        //FWeaponItemList? managedItem(int index) => managedArray[index];
        var weaponCount = 0;
        List<int> unusedIDs = [];
        for (int i = 0; i < weaponItemList->Count; i++)
        {
            var id = i;
            var weaponItem = (*weaponItemList)[i];
            var existingWeapon = activeWeapons.FirstOrDefault(x => x.WeaponId == i);
            if (existingWeapon != null)
            {
                if (existingWeapon.IsUnused)
                {
                    if (id != 0)
                    {
                        unusedIDs.Add(id);
                    }
                }
                existingWeapon.SetWeaponItemId(id);
            }
            weaponCount++;
            continue;
        }
        Log.Debug($"{unusedIDs.Count} unused weapons");
        if ( weaponCount != 512 )
        {
            Log.Error("Invalid weapon count");
            weaponCount = 512;
        }
        foreach (var existingWeapon in activeWeapons)
        {
            if (existingWeapon.WeaponItemId < weaponCount)
            {
                Log.Verbose($"{existingWeapon.Name} already exists.");
                continue;
            }
            var newFItem = new FWeaponItemList(existingWeapon);
            //Log.Debug(newFItem.ToString());
            var weaponItem = &weaponItemList->Data.allocator_instance[weaponCount];
            weaponItem->Update(newFItem);
            existingWeapon.SetWeaponItemId(weaponCount);
            weaponDesc.SetWeaponDesc(weaponCount,existingWeapon.Description);
            weaponCount++;
        }
        //var managedCount = managedItemList.Count;
        //managedItemList.Dispose();
        //weaponCount = weaponItemList->Count;
        Log.Debug($"{weaponCount}");
        //managedArray.Dispose();
        this.weaponDesc.Init();
    }
    private void LogWeaponVariables(UAppCharacterComp* comp)
    {
        var character = comp->baseObj.Character;
        var equipWeaponItemId = this.itemEquip.GetEquip(character, Equip.Weapon);
        var weaponId = comp->baseObj.WeaponId; // Updates with each change
        var weaponModelId = comp->mSetWeaponModelID; // returns the LAST modelId
        var weaponType = comp->mSetWeaponType;
        if (!this.registry.TryGetWeaponByItemId(equipWeaponItemId, out var finalWeapon))
            return;
        var sb = new StringBuilder();
        sb.AppendLine($"Character: {character}");
        sb.AppendLine($"Weapon:\n\tID: {equipWeaponItemId} [{weaponId}]\n\tName: {finalWeapon.Name}\n\tType: {weaponType}\n\tModelID: {weaponModelId}");
        Log.Debug(sb.ToString());
    }
    private void SetWeaponIdImpl(UAppCharacterComp* comp)
    {
        var character = comp->baseObj.Character;
        var weaponId = comp->baseObj.WeaponId; // Updates with each change
        var weapons = comp->baseObj.Weapons;
        const string noModel = "NONE";

        //var arrayWrapper = new Emitter.TArrayWrapper<nint>(comp->baseObj.Weapons);
        var weaponModelId = comp->mSetWeaponModelID; // returns the LAST modelId
        var astrea = character > ECharacter.Shinjiro;
        var shell = ShellExtensions.ShellFromId(weaponModelId, astrea);
        var weaponType = comp->mSetWeaponType;
        Log.Debug($"Previous model ID {(weaponModelId > 0 ? weaponModelId : noModel)}");
        if (!Characters.Armed.Contains(character))
        { 
            return; 
        }
        if (character == ECharacter.Akihiko || character == ECharacter.Aigis || character == ECharacter.AigisReal)
        {
            Log.Warning("Akihiko and Aigis do not have reconstructed BPs");
            comp->mSetWeaponModelID = weaponModelId;
        }
        var equipWeaponItemId = this.itemEquip.GetEquip(character, Equip.Weapon);
        //weaponId = equipWeaponItemId;
        Log.Debug($"{character}'s current weapon has an id of: {equipWeaponItemId}");

        /*if(this.overrides.TryGetWeaponOverrideFrom(character, equipWeaponItemId, out var weaponOverride))
        {
            weaponId = weaponOverride.WeaponId;
        }*/

        /*        if (this.registry.TryGetWeaponByItemId(equipWeaponItemId, out var finalWeapon))
                {
                    this.OnWeaponChanged.Invoke(finalWeapon);
                }*/

        comp->mSetWeaponModelID = this.redirects.UpdateFromEquippedWeapon(character, equipWeaponItemId, weaponModelId);
        var resultModelId = comp->mSetWeaponModelID;
        Log.Debug($"Final model id: {resultModelId}");
    }
}