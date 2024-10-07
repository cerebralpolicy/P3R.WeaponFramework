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

namespace P3R.WeaponFramework.Hooks;

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
internal unsafe class WeaponHooks
{
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
    private readonly WeaponRegistry registry;
    private readonly WeaponDescService weaponDesc;
    private readonly WeaponShellService weaponShells;
    private ItemEquipHooks itemEquip;

    public WeaponHooks(
        IUnreal unreal,
        IUObjects uobjects,
        IMemoryMethods memory,
        WeaponRegistry registry,
        WeaponDescService weaponDesc,
        WeaponShellService weaponShells,
        ItemEquipHooks itemEquip)
    {
        this.unreal = unreal;
        this.uobjects = uobjects;
        this.memory = memory;
        this.registry = registry;
        this.weaponDesc = weaponDesc;
        this.weaponShells = weaponShells;
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

    private void SetWeaponData(UnrealObject obj)
    {
        //Log.Verbose("WeaponTable found");
        weaponDesc.Prime(); // Get descriptions for given episode
        var weaponItemList = (UWeaponItemListTable*)obj.Self;
        var weaponItemArray = weaponItemList->Data.GetRef;
        var allWeapons = registry.Weapons;
        var activeWeapons = registry.GetActiveWeapons();
        var managedItemList = new TWeaponItemListTable(memory, &weaponItemList->Data);
        //FWeaponItemList? managedItem(int index) => managedArray[index];
        var weaponCount = 0;
        List<Weapon> unusedWeaps = [];
        List<int> unusedIDs = [];
        for (int i = 0; i < weaponItemList->Count; i++)
        {
            var id = i;
            var weaponItem = (*weaponItemList)[i];
            var slotWeapon = allWeapons.FirstOrDefault(x => x.WeaponId == id);
            var existingWeapon = activeWeapons.FirstOrDefault(x => x.WeaponId == i);
            if (existingWeapon != null)
            {
                if (existingWeapon.Name == "Unused" || existingWeapon.ModelId < 10)
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
        for (int i = weaponCount; i < 2048;  i++)
        {
//            var weaponItem = (*weaponItemList)[i];
            var existingWeapon = activeWeapons.FirstOrDefault(x => x.WeaponId == i);
            if (existingWeapon != null)
            {
                //var weaponItemInstance = weaponItemArray(i);
                //weaponItemInstance->SetFromWeapon(existingWeapon);
                // Apply to
                var newItem = new FWeaponItemList(existingWeapon).Malloc(memory);
                //newItem->EquipID = (uint)existingWeapon.Character.ToEquipID();
                //managedItemList.Add(*newItem);
                if (unusedIDs.Count != 0)
                {
                    var id = unusedIDs.FirstOrDefault();
                    var oldWeapon = activeWeapons.FirstOrDefault(x => x.WeaponItemId == id);
                    if (oldWeapon == null)
                    {
                        Log.Error("No slot available");
                        continue;
                    }
                    //managedItemList.Swap(id, i);
                    managedItemList.Overwrite(id, *newItem);
                    existingWeapon.SetWeaponItemId(id,true,i);
                    oldWeapon.SetWeaponItemId(i);
                    unusedIDs.RemoveAt(0);
                    this.weaponDesc.SetWeaponDesc(id, existingWeapon.Description);
                }
            }
            continue;
        }
        var managedCount = managedItemList.Count;
        managedItemList.Dispose();
        weaponCount = weaponItemList->Count;
        if (managedCount != weaponCount)
        {
            Log.Warning("Failed to apply new weapons.");
        }
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
        var arrayWrapper = new Emitter.TArrayWrapper<nint>(comp->baseObj.Weapons);
        var weaponModelId = comp->mSetWeaponModelID; // returns the LAST modelId
        var weaponType = comp->mSetWeaponType;
        Log.Debug($"Previous model ID {(weaponModelId > 0 ? weaponModelId : noModel)}");
        if (!Characters.Armed.Contains(character))
        { return; }
        var equipWeaponItemId = this.itemEquip.GetEquip(character, Equip.Weapon);
        Log.Debug($"{character}'s current weapon has an id of: {equipWeaponItemId}");
        if (!this.registry.TryGetWeaponByItemId(equipWeaponItemId, out var finalWeapon))
        {
            Log.Debug($"Weapon modelId: {weaponModelId}");
            comp->mSetWeaponModelID = weaponModelId;
            //comp->mSetWeaponModelID = weaponShells.UpdateWeapon(ShellExtensions.ShellFromId(finalWeapon.ModelId, character == ECharacter.Metis), finalWeapon.ModelId, equipWeaponItemId);
            //weaponShells.UpdateWeapon(finalWeapon.Character, equipWeaponItemId);
        }
        else
        {
            //var finalModelId = weaponShells.UpdateWeapon(finalWeapon.ShellTarget, finalWeapon.ModelId, equipWeaponItemId);
            var update = weaponShells.UpdateWeapon(finalWeapon, weaponId, weaponModelId);
            comp->mSetWeaponModelID = weaponModelId;
            //Log.Debug($"Result: {finalWeapon.Name}\nModel ID: {comp->mSetWeaponModelID}\nNew ModelID: {finalWeapon.ModelId}");
            //LogWeaponVariables(comp);
            weaponShells.RedirectHandler(finalWeapon); 
        }

    }
}