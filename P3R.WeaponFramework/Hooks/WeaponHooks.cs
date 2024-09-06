using P3R.WeaponFramework.Hooks.Models;
using P3R.WeaponFramework.Types;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using Project.Utils;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
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

    private readonly Core core;
    private readonly WeaponRegistry registry;
    private readonly WeaponDescService weaponDesc;
    private ItemEquipHooks itemEquip;
    private readonly Dictionary<Character, WeaponConfig> defaultWeapons = [];

    public WeaponHooks(
        Core core, 
        WeaponRegistry registry, 
        WeaponDescService weaponDesc, 
        ItemEquipHooks itemEquip)
    {
        this.core = core;
        this.registry = registry;
        this.weaponDesc = weaponDesc;
        this.itemEquip = itemEquip;

        foreach (var character in Enum.GetValues<Character>()) 
        { 
            if (character == Character.Fuuka) continue;
            if (character > Character.Shinjiro) break;
            defaultWeapons[character] = new DefaultWeapon(character);
        }

        core.FindObjectAsync("DatItemWeaponDataAsset", SetWeaponData);

        ScanHooks.Add(
            nameof(UAppCharacterComp_Update),
            "48 8B C4 48 89 48 ?? 55 41 54 48 8D 68 ?? 48 81 EC 48 01 00 00",
            (hooks, result) =>
            {
                this.characterCompUpdate = hooks.CreateWrapper<UAppCharacterComp_Update>(result, out _);

                var setWeaponAddress = result + 0x26B; // Costumes = 0x255
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

    private void SetWeaponData(nint obj)
    {
        var weaponItemList = (UWeaponItemListTable*)obj.ToPointer();

        core.Utils.Log("Manually generating list of valid weapons.", LogLevel.Debug);
        Dictionary<int, FWeaponItemList> weaponLookupTable = [];
        for (int i = 0; i < weaponItemList->Count; i++)
        {
            var weaponItem = (*weaponItemList)[i];
            weaponLookupTable.Add(i, weaponItem);
            continue;
        }

        core.Utils.Log("Setting weapon data.");

        var itemId = 0;
        foreach (var weaponEntry in weaponLookupTable)
        {
            var weaponItem = weaponEntry.Value;
            var id = weaponEntry.Key;
            registry.TryGetWeaponById(id, out var existingWeapon); // FILTER
            if (existingWeapon != null)
            {
                existingWeapon.SetWeaponItemId(itemId);
                itemId++;
            }
        }
        var newItemIndex = core.NewItemIndex;
        foreach (var weapon in registry.GetActiveWeapons())
        {
            if (weapon.WeaponId != default)
            {
                continue;
            }

            var newItem = &weaponItemList->Data.allocator_instance[newItemIndex];
            newItem->ModelID = (ushort)weapon.ModelId;
            newItem->EquipID = (uint)AssetUtils.GetEquipFromChar(weapon.Character);
            weapon.SetWeaponItemId(newItemIndex);
            weaponDesc.SetWeaponDesc(newItemIndex, weapon.Description);

            if (weapon.WeaponId >= GameWeapons.BASE_MOD_WEAP_ID)
            {
                this.SetWeaponPaths(weapon);
                core.Utils.Log($"Added weapon item: {weapon.Name} || Weapon Item ID: {newItemIndex} || Weapon ID: {weapon.WeaponId}");
                core.NewItemIndex++;
            }
            
        }
    }

    private void SetWeaponPaths(Weapon weapon)
    {
        foreach (var assetType in Enum.GetValues<WeaponAssetType>())
        {
            this.SetWeaponFile(weapon, assetType);
        }
    }

    private void SetWeaponFile(Weapon weapon, WeaponAssetType assetType)
    {
        var ogAssetFile = AssetUtils.GetAssetFile(weapon.Character, weapon.WeaponModelId, assetType);
        var currentAssetFile = weapon.Config.GetAssetFile(assetType) ?? this.GetDefaultAsset(weapon.Character, assetType);

        if (ogAssetFile == null)
        {
            core.Utils.Log($"Asset has no original: {assetType} || Weapon: {weapon.Name}", LogLevel.Debug);
            return;
        }

        if (currentAssetFile == null)
        {
            core.Utils.Log($"Asset has no default or new: {assetType} || Weapon: {weapon.Name}", LogLevel.Debug);
            return;
        }

        if (ogAssetFile == currentAssetFile)
        {
            return;
        }

        var ogAssetFNames = new AssetFNames(ogAssetFile);
        var newAssetFNames = new AssetFNames(currentAssetFile);

        core.Unreal.AssignFName(Mod.NAME, ogAssetFNames.AssetPath, newAssetFNames.AssetPath);
        core.Unreal.AssignFName(Mod.NAME, ogAssetFNames.AssetName, newAssetFNames.AssetName);
    }

    private string? GetDefaultAsset(Character character, WeaponAssetType assetType)
    {
        return defaultWeapons[character].GetAssetFile(assetType);
    }

    private void SetWeaponIdImpl(UAppCharacterComp* comp)
    {
        var character = comp->baseObj.Character;
        var weaponId = comp->mSetWeaponModelID;
        if (character < Character.Player || character > Character.Metis)
        {
            return;
        }
    }
}
