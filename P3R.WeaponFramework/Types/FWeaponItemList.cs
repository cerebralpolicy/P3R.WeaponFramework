using P3R.WeaponFramework.Weapons.Models;
using System.Runtime.InteropServices;

namespace P3R.WeaponFramework.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x44)]
public unsafe struct FWeaponItemList
{
    [FieldOffset(0x0000)] public Emitter.FString ItemDef;
    [FieldOffset(0x0010)] public ushort SortNum;
    [FieldOffset(0x0014)] public EWeaponType WeaponType;
    [FieldOffset(0x0018)] public EEquipFlag EquipID;
    [FieldOffset(0x001C)] public EBtlDataAttr AttrID;
    [FieldOffset(0x001E)] public ushort Rarity;
    [FieldOffset(0x0020)] public ushort Tier;
    [FieldOffset(0x0022)] public ushort Attack;
    [FieldOffset(0x0024)] public ushort Accuracy;
    [FieldOffset(0x0026)] public ushort Strength;
    [FieldOffset(0x0028)] public ushort Magic;
    [FieldOffset(0x002A)] public ushort Endurance;
    [FieldOffset(0x002C)] public ushort Agility;
    [FieldOffset(0x002E)] public ushort Luck;
    [FieldOffset(0x0030)] public EItemSkillId SkillID;
    [FieldOffset(0x0034)] public uint Price;
    [FieldOffset(0x0038)] public uint SellPrice;
    [FieldOffset(0x003C)] public ushort GetFLG;
    [FieldOffset(0x003E)] public ushort ModelID;
    [FieldOffset(0x0040)] public uint Flags;

    public FWeaponItemList(Weapon weapon)
    {
        ushort NullableField(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (ushort)statField.Value;
        }
        uint NullablePrice(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (uint)statField.Value;
        }
        SortNum = (ushort)weapon.SortNum;
        WeaponType = weapon.Character.ToWeaponType();
        EquipID = weapon.Character.ToEquipID();
        var stats = weapon.Stats;
        Rarity = (ushort)stats.Rarity;
        Tier = (ushort)stats.Tier;
        Attack = (ushort)stats.Attack;
        Accuracy = (ushort)stats.Accuracy;
        Strength = NullableField(stats.Strength);
        Magic = NullableField(stats.Magic);
        Endurance = NullableField(stats.Endurance);
        Agility = NullableField(stats.Agility);
        Luck = NullableField(stats.Luck);
        Price = NullablePrice(stats.Price);
        SellPrice = NullablePrice(stats.SellPrice);
        GetFLG = 0;
        ModelID = (ushort)weapon.ModelId;
        Flags = 0;
    }
}

public unsafe static class WeaponItemListUtils
{
    public static unsafe void ApplyWeaponItem(this FWeaponItemList newItem, Weapon weapon)
    {
        ushort NullableField(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (ushort)statField.Value;
        }
        uint NullablePrice(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (uint)statField.Value;
        }
        newItem.SortNum = (ushort)weapon.SortNum;
        newItem.WeaponType = weapon.Character.ToWeaponType();
        newItem.EquipID = weapon.Character.ToEquipID();
        var stats = weapon.Stats;
        newItem.Rarity = (ushort)stats.Rarity;
        newItem.Tier = (ushort)stats.Tier;
        newItem.Attack = (ushort)stats.Attack;
        newItem.Accuracy = (ushort)stats.Accuracy;
        newItem.Strength = NullableField(stats.Strength);
        newItem.Magic = NullableField(stats.Magic);
        newItem.Endurance = NullableField(stats.Endurance);
        newItem.Agility = NullableField(stats.Agility);
        newItem.Luck = NullableField(stats.Luck);
        newItem.Price = NullablePrice(stats.Price);
        newItem.SellPrice = NullablePrice(stats.SellPrice);
        newItem.GetFLG = 0;
        newItem.ModelID = (ushort)weapon.ModelId;
        newItem.Flags = 0;
        Log.Debug($"New weapon || {newItem.Attack} || {newItem.Accuracy}");
    }
    public static FWeaponItemList* FWeaponItemList(Weapon weapon)
    {
        ushort NullableField(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (ushort)statField.Value;
        }
        uint NullablePrice(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (uint)statField.Value;
        }
        var newItem = new FWeaponItemList();
        newItem.SortNum = (ushort)weapon.SortNum;
        newItem.WeaponType = weapon.Character.ToWeaponType();
        newItem.EquipID = weapon.Character.ToEquipID();
        var stats = weapon.Stats;
        newItem.Rarity = (ushort)stats.Rarity;
        newItem.Tier = (ushort)stats.Tier;
        newItem.Attack = (ushort)stats.Attack;
        newItem.Accuracy = (ushort)stats.Accuracy;
        newItem.Strength = NullableField(stats.Strength);
        newItem.Magic = NullableField(stats.Magic);
        newItem.Endurance = NullableField(stats.Endurance);
        newItem.Agility = NullableField(stats.Agility);
        newItem.Luck = NullableField(stats.Luck);
        newItem.Price = NullablePrice(stats.Price);
        newItem.SellPrice = NullablePrice(stats.SellPrice);
        newItem.GetFLG = 0;
        newItem.ModelID = (ushort)weapon.ModelId;
        newItem.Flags = 0;
        Log.Debug($"New weapon || {newItem.Attack} || {newItem.Accuracy}");
        return &newItem;
    }
}
