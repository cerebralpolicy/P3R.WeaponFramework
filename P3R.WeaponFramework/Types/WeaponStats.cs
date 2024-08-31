namespace P3R.WeaponFramework.Interfaces.Types;

public struct WeaponStats
{
    public int Rarity = 1;
    public int Tier = 1;
    public int Attack;
    public int Accuracy;
    public int Strength;
    public int Magic;
    public int Endurance;
    public int Agility;
    public int Luck;
    public EItemSkill SkillID;
    public int Price;
    public int SellPrice;

    public WeaponStats()
    {
    }

    public WeaponStats(int attack, int accuracy, int strength = 0, int magic = 0, int endurance = 0, int agility = 0, int luck = 0, EItemSkill skill = EItemSkill.None, int price = 400, int sellPrice = 100) : this()
    {
        Attack = attack;
        Accuracy = accuracy;
        Strength = strength;
        Magic = magic;
        Endurance = endurance;
        Agility = agility;
        Luck = luck;
        Price = price;
        SellPrice = sellPrice;
    }

    public WeaponStats(int rarity, int tier, int attack, int accuracy, int strength = 0, int magic = 0, int endurance = 0, int agility = 0, int luck = 0, int price = 400, int sellPrice = 100)
    {
        Rarity = rarity;
        Tier = tier;
        Attack = attack;
        Accuracy = accuracy;
        Strength = strength;
        Magic = magic;
        Endurance = endurance;
        Agility = agility;
        Luck = luck;
        Price = price;
        SellPrice = sellPrice;
    }
}