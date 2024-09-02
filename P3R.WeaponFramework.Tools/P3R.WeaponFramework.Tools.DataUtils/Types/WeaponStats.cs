namespace P3R.WeaponFramework.Tools.DataUtils;

public struct WeaponStats
{
    public int AttrId { get; set; }
    public int Rarity { get; set; } = 1;
    public int Tier { get; set; } = 1;
    public int Attack { get; set; }
    public int Accuracy { get; set; }
    public int Strength { get; set; } = 0;
    public int Magic { get; set; } = 0;
    public int Endurance { get; set; } = 0;
    public int Agility { get; set; } = 0;
    public int Luck { get; set; } = 0;
    public int SkillId { get; set; } = 0;
    public int Price { get; set; } = 400;
    public int SellPrice { get; set; } = 100;


    public WeaponStats()
    {
    }

    public WeaponStats(int attrId, int rarity, int tier, int attack, int accuracy, int strength, int magic, int endurance, int agility, int luck, int skillId, int price, int sellPrice)
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
}
