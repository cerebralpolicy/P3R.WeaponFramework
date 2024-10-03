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
    private readonly WeaponRegistry registry;
    private readonly WeaponDescService weaponDesc;
    private readonly WeaponShellService weaponShells;
    private ItemEquipHooks itemEquip;

    public WeaponHooks(
        IUnreal unreal,
        IUObjects uobjects,
        WeaponRegistry registry,
        WeaponDescService weaponDesc,
        WeaponShellService weaponShells,
        ItemEquipHooks itemEquip)
    {
        this.unreal = unreal;
        this.uobjects = uobjects;
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
        Log.Verbose("WeaponTable found");
        var weaponItemList = (UWeaponItemListTable*)obj.Self;
        var activeWeapons = registry.GetActiveWeapons();
        
        Log.Debug("Setting weapon data.");
        List<Weapon> tempList = [];
        for (int i = 0; i < weaponItemList->Count; i++)
        {
            var weaponItem = (*weaponItemList)[i];
            var existingWeapon = activeWeapons.FirstOrDefault(x => x.WeaponId == i);
            if (existingWeapon != null)
            {
                var id = tempList.Count;
                existingWeapon.SetWeaponItemId(id);
                tempList.Add(existingWeapon);
            }
            continue;
        }
        weaponDesc.Prime(); // Get descriptions for given episode
        var newItemIndex = 512;
        foreach (var weapon in registry.GetActiveWeapons())
        {
            if (weapon.WeaponId < Episode.BASE_EPISODE_WEAP_ID)
            {
                continue;
            }
            var item = new FWeaponItemList(weapon);
//            weaponItemList->Data.allocator_instance[newItemIndex] = item;
            var newItem = &weaponItemList->Data.allocator_instance[newItemIndex];
            newItem->SortNum = item.SortNum;
            newItem->WeaponType = item.WeaponType;
            newItem->EquipID = item.EquipID;
            newItem->Rarity = item.Rarity;
            newItem->Tier = item.Tier;
            newItem->Attack = item.Attack;
            newItem->Accuracy = item.Accuracy;
            newItem->Strength = item.Strength;
            newItem->Magic = item.Magic;
            newItem->Endurance = item.Endurance;
            newItem->Agility = item.Agility;
            newItem->Luck = item.Luck;
            newItem->Price = item.Price;
            newItem->SellPrice = item.SellPrice;
            newItem->GetFLG = 0;
            newItem->ModelID = (ushort)weapon.ModelId;
            newItem->Flags = 0;
            Log.Verbose(item.ToString()!);
            //weaponDesc.AddDescription(weapon.Description);
            weapon.SetWeaponItemId(newItemIndex);
            weaponDesc.SetWeaponDesc(newItemIndex, weapon.Description);
            Log.Debug($"Added weapon item: {weapon.Name} || Weapon Type {weapon.Character.ToWeaponType()} || Attack: {item.Attack} || Weapon ID: {weapon.WeaponId}");
            newItemIndex++;
        }
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