using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using P3R.WeaponFramework.Utils;

namespace P3R.WeaponFramework.Types;

[StructLayout(LayoutKind.Explicit)]
public class WeaponItem : IEquatable<WeaponItem?>
{
    [FieldOffset(0x0000)] public FString ItemDef;
    [FieldOffset(0x0010)] public ushort SortNum;
    [FieldOffset(0x0014)] public uint WeaponType;
    [FieldOffset(0x0018)] public EquipFlag EquipID;
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
    [FieldOffset(0x0034)] public uint? Price;
    [FieldOffset(0x0038)] public uint? SellPrice;
    [FieldOffset(0x003C)] public ushort GetFLG;
    [FieldOffset(0x003E)] public ushort ModelID;
    [FieldOffset(0x0040)] public uint Flags;

    public WeaponItem()
    {
    }
    public WeaponItem(FString itemDef, ushort sortNum, uint weaponType, EquipFlag equipID, EBtlDataAttr attrID, ushort rarity, ushort tier, ushort attack, ushort accuracy, ushort strength, ushort magic, ushort endurance, ushort agility, ushort luck, EItemSkillId skillID, uint? price, uint? sellPrice, ushort getFLG, ushort modelID, uint flags)
    {
        ItemDef = itemDef;
        SortNum = sortNum;
        WeaponType = weaponType;
        EquipID = equipID;
        AttrID = attrID;
        Rarity = rarity;
        Tier = tier;
        Attack = attack;
        Accuracy = accuracy;
        Strength = strength;
        Magic = magic;
        Endurance = endurance;
        Agility = agility;
        Luck = luck;
        SkillID = skillID;
        Price = price ?? (uint)PriceUtils.GetBuyPrice(attack,accuracy);
        SellPrice = sellPrice ?? (uint)PriceUtils.GetSellPrice(attack,accuracy);
        GetFLG = getFLG;
        ModelID = modelID;
        Flags = flags;
    }

    public ECharacter Character => Enum.Parse<ECharacter>(EquipID.ToString());

    public WeaponStats Stats => new() 
    { 
        AttrId = AttrID,
        Rarity = Rarity,
        Tier = Tier,
        Attack = Attack,
        Accuracy = Accuracy,
        Strength = Strength,
        Magic = Magic,
        Endurance = Endurance,
        Agility = Agility,
        Luck = Luck,
        SkillId = SkillID,
        Price = (int?)Price ?? PriceUtils.GetBuyPrice(Attack, Accuracy),
        SellPrice = (int?)SellPrice ?? PriceUtils.GetSellPrice(Attack, Accuracy),
    };

    public override bool Equals(object? obj)
    {
        return Equals(obj as WeaponItem);
    }

    public bool Equals(WeaponItem? other)
    {
        return other is not null &&
               Character == other.Character &&
               Stats.Equals(other.Stats);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Character, Stats);
    }

    public static bool operator ==(WeaponItem? left, WeaponItem? right)
    {
        return EqualityComparer<WeaponItem>.Default.Equals(left, right);
    }

    public static bool operator !=(WeaponItem? left, WeaponItem? right)
    {
        return !(left == right);
    }
    public static implicit operator WeaponItem(FWeaponItemList fWeaponItem)
    {
        return new()
        {
            ItemDef = fWeaponItem.ItemDef,
            SortNum = fWeaponItem.SortNum,
            WeaponType = fWeaponItem.WeaponType,
            EquipID = (EquipFlag)fWeaponItem.EquipID,
            AttrID = (EBtlDataAttr)fWeaponItem.AttrID,
            Rarity = fWeaponItem.Rarity,
            Tier = fWeaponItem.Tier,
            Attack = fWeaponItem.Attack,
            Accuracy = fWeaponItem.Accuracy,
            Strength = fWeaponItem.Strength,
            Magic = fWeaponItem.Magic,
            Endurance = fWeaponItem.Endurance,
            Agility = fWeaponItem.Agility,
            Luck = fWeaponItem.Luck,
            Price = fWeaponItem.Price,
            SellPrice = fWeaponItem.SellPrice,
            GetFLG = fWeaponItem.GetFLG,
            ModelID = fWeaponItem.ModelID,
            Flags = fWeaponItem.Flags,
        };
    }
}
