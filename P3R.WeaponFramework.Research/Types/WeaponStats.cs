namespace P3R.WeaponFramework.Research.Types;

public struct WeaponStats
{
    public ushort AttrID { get; set; }
    public ushort Rarity { get; set; } = 1;
    public ushort Tier { get; set; } = 1;
    public ushort Attack { get; set; }
    public ushort Accuracy { get; set; }
    public ushort Strength { get; set; }
    public ushort Magic { get; set; }
    public ushort Endurance { get; set; }
    public ushort Agility { get; set; }
    public ushort Luck { get; set; }
    public uint SkillID { get; set; } = 0;
    public uint Price { get; set; } = 400;
    public uint SellPrice { get; set; } = 100;

    public WeaponStats(FWeaponItemList item)
    {
        AttrID = item.AttrID;
        Rarity = item.Rarity;
        Tier = item.Tier;
        Attack = item.Attack;
        Accuracy = item.Accuracy;
        Strength = item.Strength;
        Magic = item.Magic;
        Endurance = item.Endurance;
        Agility = item.Agility;
        Luck = item.Luck;
        SkillID = item.SkillID;
        Price = item.Price;
        SellPrice = item.SellPrice;
    }

    public WeaponStats()
    {
    }

}