
using System.Text.Json.Serialization;

namespace P3R.WeaponFramework.Tools.DataUtils;

[JsonSerializable(typeof(FWeaponItemList))]
public class ExWeaponItemList : IEquatable<ExWeaponItemList>
{
    [JsonInclude]
    [JsonPropertyOrder(0)]
    public string ItemDef = string.Empty;
    [JsonInclude]
    [JsonPropertyOrder(1)]
    public short SortNum;
    [JsonInclude]
    [JsonPropertyOrder(2)]
    public int WeaponType;
    [JsonInclude]
    [JsonPropertyOrder(3)]
    public EEquipFlag EquipID;
    [JsonInclude]
    [JsonPropertyOrder(4)]
    public EBtlDataAttr AttrID;
    [JsonInclude]
    [JsonPropertyOrder(5)]
    public short Rarity;
    [JsonInclude]
    [JsonPropertyOrder(6)]
    public short Tier;
    [JsonInclude]
    [JsonPropertyOrder(7)]
    public short Attack;
    [JsonInclude]
    [JsonPropertyOrder(8)]
    public short Accuracy;
    [JsonInclude]
    [JsonPropertyOrder(9)]
    public short Strength;
    [JsonInclude]
    [JsonPropertyOrder(10)]
    public short Magic;
    [JsonInclude]
    [JsonPropertyOrder(11)]
    public short Endurance;
    [JsonInclude]
    [JsonPropertyOrder(12)]
    public short Agility;
    [JsonInclude]
    [JsonPropertyOrder(13)]
    public short Luck;
    [JsonInclude]
    [JsonPropertyOrder(14)]
    public EItemSkillId SkillID;
    [JsonInclude]
    [JsonPropertyOrder(15)]
    public int Price;
    [JsonInclude]
    [JsonPropertyOrder(16)]
    public int SellPrice;
    [JsonInclude]
    [JsonPropertyOrder(17)]
    public short GetFLG;
    [JsonInclude]
    [JsonPropertyOrder(18)]
    public short ModelID;
    [JsonInclude]
    [JsonPropertyOrder(19)]
    public int Flags;

    [JsonConstructor]
    public ExWeaponItemList(string itemDef, short sortNum, int weaponType, EEquipFlag equipID, EBtlDataAttr attrID, short rarity, short tier, short attack, short accuracy, short strength, short magic, short endurance, short agility, short luck, EItemSkillId skillID, int price, int sellPrice, short getFLG, short modelID, int flags)
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
        Price = price;
        SellPrice = sellPrice;
        GetFLG = getFLG;
        ModelID = modelID;
        Flags = flags;
    }

    public ExWeaponItemList() { }
    public override bool Equals(object? obj)
    {
        return obj is ExWeaponItemList list && Equals(list);
    }

    public WeaponStats GetStats()
        => new()
        {
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
            Price = Price,
            SellPrice = SellPrice,
            AttrId = AttrID,
        };
    public bool Equals(ExWeaponItemList other)
    {
        return ItemDef == other.ItemDef &&
               SortNum == other.SortNum &&
               WeaponType == other.WeaponType &&
               EquipID == other.EquipID &&
               AttrID == other.AttrID &&
               Rarity == other.Rarity &&
               Tier == other.Tier &&
               Attack == other.Attack &&
               Accuracy == other.Accuracy &&
               Strength == other.Strength &&
               Magic == other.Magic &&
               Endurance == other.Endurance &&
               Agility == other.Agility &&
               Luck == other.Luck &&
               SkillID == other.SkillID &&
               Price == other.Price &&
               SellPrice == other.SellPrice &&
               GetFLG == other.GetFLG &&
               ModelID == other.ModelID &&
               Flags == other.Flags;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(ItemDef);
        hash.Add(SortNum);
        hash.Add(WeaponType);
        hash.Add(EquipID);
        hash.Add(AttrID);
        hash.Add(Rarity);
        hash.Add(Tier);
        hash.Add(Attack);
        hash.Add(Accuracy);
        hash.Add(Strength);
        hash.Add(Magic);
        hash.Add(Endurance);
        hash.Add(Agility);
        hash.Add(Luck);
        hash.Add(SkillID);
        hash.Add(Price);
        hash.Add(SellPrice);
        hash.Add(GetFLG);
        hash.Add(ModelID);
        hash.Add(Flags);
        return hash.ToHashCode();
    }

    public static bool operator ==(ExWeaponItemList left, ExWeaponItemList right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ExWeaponItemList left, ExWeaponItemList right)
    {
        return !(left == right);
    }
}
