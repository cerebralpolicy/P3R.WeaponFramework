using P3R.WeaponFramework.Hooks.Models;
using P3R.WeaponFramework.Hooks.Services;
using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Interfaces.Definitions;
using P3R.WeaponFramework.Types;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using System.Linq;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;

namespace P3R.WeaponFramework.Hooks;

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
    private readonly WeaponRegistry registry;
    private readonly WeaponDescService weaponDesc;
    private readonly WeaponShellService weaponShells;
    private ItemEquipHooks itemEquip;

    public WeaponHooks(
        IUnreal unreal,
        IUObjects uobjects,
        WeaponRegistry registry,
        WeaponDescService weaponDesc,
        WeaponShellService shellService,
        ItemEquipHooks itemEquip)
    {
        this.unreal = unreal;
        this.uobjects = uobjects;
        this.registry = registry;
        this.weaponDesc = weaponDesc;
        this.weaponShells = shellService;
        this.itemEquip = itemEquip;

        foreach (var character in Characters.WFArmed)
        {
            if (character > ECharacter.Aigis12) break;
            foreach (var shell in character.Shells)
            {

            }
        }

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
        var weaponItemList = (UWeaponItemListTable*)obj.Self;
        var activeWeapons = registry.GetActiveWeapons();
        //var activeAstreaWeapons = registry.GetActiveAstreaWeapons();


        //Log.Debug("Manually generating list of valid weapons.");
        Log.Debug("Setting weapon data.");
        Dictionary<int, FWeaponItemList> weaponLookupTable = [];
        for (int i = 0; i < weaponItemList->Count; i++)
        {
            var weaponItem = (*weaponItemList)[i];
            var existingWeapon = activeWeapons.FirstOrDefault(x => x.Stats.Equals(weaponItem));
            if (existingWeapon != null)
            {
                existingWeapon.SetWeaponItemId(i);
                continue;
            }
        }
        var newItemIndex = 513;
        foreach (var weapon in registry.GetActiveWeapons())
        {
            if (weapon.WeaponId < GameWeapons.BASE_MOD_WEAP_ID)
            {
                continue;
            }

            var newItem = &weaponItemList->Data.allocator_instance[newItemIndex];
            newItem->ModelID = (ushort)weapon.ModelId;
            newItem->EquipID = (uint)AssetUtils.GetEquipFromChar(weapon.Character);
            weapon.SetWeaponItemId(newItemIndex);
            weaponDesc.SetWeaponDesc(newItemIndex, weapon.Description);
            //this.SetWeaponPaths(weapon);
            Log.Debug($"Added weapon item: {weapon.Name} || Weapon Item ID: {newItemIndex} || Weapon ID: {weapon.WeaponId}");
            newItemIndex++;
        }
        this.weaponDesc.Init();
    }

    private void SetWeaponIdImpl(UAppCharacterComp* comp)
    {
        var character = comp->baseObj.Character;
        Character wfCharacter = character;
        if (character < ECharacter.Player || character > ECharacter.Metis)
        {
            return;
        }
        if (!Characters.WFArmed.Contains((Character)character))
                { return; }
        var equipWeaponItemId = this.itemEquip.GetEquip(character, Equip.Weapon);
        this.registry.TryGetWeaponByItemId(equipWeaponItemId, out var weapon);
        if (weapon != null)
        {
            // comp->mSetWeaponType toggles between animation packages
            comp->mSetWeaponModelID = weaponShells.UpdateWeapon(character, weapon.WeaponId);
        }
    }
}