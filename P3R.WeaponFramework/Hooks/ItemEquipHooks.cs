using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using System.Diagnostics.CodeAnalysis;

namespace P3R.WeaponFramework.Hooks;

internal unsafe class ItemEquipHooks
{
    private delegate nint GetGlobalWork();
    private GetGlobalWork? getGlobalWork;

    private readonly WeaponRegistry weapons;

    public ItemEquipHooks(WeaponRegistry weapons)
    {
        this.weapons = weapons;

        ScanHooks.Add(
            nameof(GetGlobalWork),
            "48 89 5C 24 ?? 57 48 83 EC 20 48 8B 0D ?? ?? ?? ?? 33 DB",
            (hooks, result) => this.getGlobalWork = hooks.CreateWrapper<GetGlobalWork>(result, out _));
    }

    public nint GetCharWork(ECharacter character)
        => this.getGlobalWork!() + 0x1b0 + ((nint)character * 0x2b4);

    public int GetEquip(ECharacter character, Equip equipId)
        => *(ushort*)(this.GetCharWork(character) + 0x28c + ((nint)equipId * 2));
    public bool TryGetEquipWeapon(ECharacter character, [NotNullWhen(true)] out Weapon? weapon)
    {
        var equipItemId = this.GetEquip(character, Equip.Weapon);
        return this.weapons.TryGetWeaponByItemId(equipItemId, out weapon);
    }
}

public enum Equip
    : ushort
{
    Weapon,
    Armor,
    Footwear,
    Accessory,
    Outfit,
}
