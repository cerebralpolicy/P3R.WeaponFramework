namespace P3R.WeaponFramework.Interfaces.Types;

public struct WeaponStats : IEquatable<WeaponStats>
{
    public WeaponStats()
    {
    }

    public WeaponStats(EBtlDataAttr attrId, int rarity, int tier, int attack, int accuracy, int strength, int magic, int endurance, int agility, int luck, EItemSkillId skillId, int price, int sellPrice)
    {
        AttrId = attrId;
        Rarity = rarity;
        Tier = tier;
        Attack = attack;
        Accuracy = accuracy;
        Strength = strength;
        Magic = magic;
        Endurance = endurance;
        Agility = agility;
        Luck = luck;
        SkillId = skillId;
        Price = price;
        SellPrice = sellPrice;
    }

    public EBtlDataAttr AttrId { get; set; }
    public int Rarity { get; set; } = 1;
    public int Tier { get; set; } = 1;
    public int Attack { get; set; }
    public int Accuracy { get; set; }
    public int? Strength { get; set; } = 0;
    public int? Magic { get; set; } = 0;
    public int? Endurance { get; set; } = 0;
    public int? Agility { get; set; } = 0;
    public int? Luck { get; set; } = 0;
    public EItemSkillId SkillId { get; set; } = 0;
    public int Price { get; set; } = 400;
    public int SellPrice { get; set; } = 100;

    public override bool Equals(object? obj)
    {
        return obj is WeaponStats stats && Equals(stats);
    }

    public bool Equals(WeaponStats other)
    {
        return AttrId == other.AttrId &&
               Rarity == other.Rarity &&
               Tier == other.Tier &&
               Attack == other.Attack &&
               Accuracy == other.Accuracy &&
               Strength == other.Strength &&
               Magic == other.Magic &&
               Endurance == other.Endurance &&
               Agility == other.Agility &&
               Luck == other.Luck &&
               SkillId == other.SkillId &&
               Price == other.Price &&
               SellPrice == other.SellPrice;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(AttrId);
        hash.Add(Rarity);
        hash.Add(Tier);
        hash.Add(Attack);
        hash.Add(Accuracy);
        hash.Add(Strength);
        hash.Add(Magic);
        hash.Add(Endurance);
        hash.Add(Agility);
        hash.Add(Luck);
        hash.Add(SkillId);
        hash.Add(Price);
        hash.Add(SellPrice);
        return hash.ToHashCode();
    }

    public static bool operator ==(WeaponStats left, WeaponStats right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(WeaponStats left, WeaponStats right)
    {
        return !(left == right);
    }
}