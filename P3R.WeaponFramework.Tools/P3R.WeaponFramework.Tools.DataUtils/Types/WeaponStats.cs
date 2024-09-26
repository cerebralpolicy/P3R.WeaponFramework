using System.Text.Json.Serialization;

namespace P3R.WeaponFramework.Tools.DataUtils;

public struct WeaponStats
{
    [JsonPropertyName("AttrId")]
    public int AttrId { get; set; }
    [JsonPropertyName("Rarity")]
    public int Rarity { get; set; } = 1;
    [JsonPropertyName("Tier")]
    public int Tier { get; set; } = 1;
    [JsonPropertyName("Attack")]
    public int Attack { get; set; }
    [JsonPropertyName("Accuracy")]
    public int Accuracy { get; set; }
    [JsonPropertyName("Strength")]
    public int Strength { get; set; } = 0;
    [JsonPropertyName("Magic")]
    public int Magic { get; set; } = 0;
    [JsonPropertyName("Endurance")]
    public int Endurance { get; set; } = 0;
    [JsonPropertyName("Agility")]
    public int Agility { get; set; } = 0;
    [JsonPropertyName("Luck")]
    public int Luck { get; set; } = 0;
    [JsonPropertyName("SkillId")]
    public int SkillId { get; set; } = 0;
    [JsonPropertyName("Price")]
    public int Price { get; set; } = 400;
    [JsonPropertyName("SellPrice")]
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
