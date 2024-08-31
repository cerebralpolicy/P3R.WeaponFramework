namespace P3R.WeaponFramework.Weapons;

public struct WeaponStats
{
    public WeaponStats()
    {
    }

    public WeaponStats(EBtlDataAttr attrID, int rarity, int tier, int attack, int accuracy, int strength, int magic, int endurance, int agility, int luck, EItemSkillID skillID, int price, int sellPrice)
    {
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
    }

    public EBtlDataAttr AttrID { get; set; }
    public int Rarity { get; set; } = 1;
    public int Tier { get; set; } = 1;
    public int Attack { get; set; }
    public int Accuracy { get; set; }
    public int? Strength { get; set; } = 0;
    public int? Magic { get; set; } = 0;
    public int? Endurance { get; set; } = 0;
    public int? Agility { get; set; } = 0;
    public int? Luck { get; set; } = 0;
    public EItemSkillID SkillID { get; set; } = 0;
    public int Price { get; set; } = 400;
    public int SellPrice { get; set; } = 100;
}