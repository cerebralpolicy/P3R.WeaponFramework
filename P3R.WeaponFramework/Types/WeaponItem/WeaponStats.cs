using System.Text;
using YamlDotNet.Serialization;

namespace P3R.WeaponFramework.Types;
[YamlSerializable]
public class WeaponStats : IEquatable<WeaponStats>
{
    public WeaponStats()
    {
    }
    [YamlMember]
    public EBtlDataAttr AttrId { get; set; }
    [YamlMember]
    public ushort Rarity { get; set; } = 1;
    [YamlMember]
    public ushort Tier { get; set; } = 1;
    [YamlMember]
    public ushort Attack { get; set; } = 30;
    [YamlMember]
    public ushort Accuracy { get; set; } = 85;
    [YamlMember]
    public ushort Strength { get; set; } = 0;
    [YamlMember]
    public ushort Magic { get; set; } = 0;
    [YamlMember]
    public ushort Endurance { get; set; } = 0;
    [YamlMember]
    public ushort Agility { get; set; } = 0;
    [YamlMember]
    public ushort Luck { get; set; } = 0;
    [YamlMember]
    public EItemSkillId SkillId { get; set; } = 0;
    [YamlMember]
    public uint Price { get; set; } = 400;
    [YamlMember]
    public uint SellPrice { get; set; } = 100;

    public string Summarize()
    {
        var sb = new StringBuilder();
        sb.Append($"\n\t{nameof(AttrId)}: {AttrId}");
        sb.Append($"\n\t{nameof(Rarity)}: {Rarity}");
        sb.Append($"\n\t{nameof(Tier)}: {Tier}");
        sb.Append($"\n\t{nameof(Attack)}: {Attack}");
        sb.Append($"\n\t{nameof(Accuracy)}: {Accuracy}");
        sb.Append($"\n\t{nameof(Strength)}: {Strength}");
        sb.Append($"\n\t{nameof(Magic)}: {Magic}");
        sb.Append($"\n\t{nameof(Endurance)}: {Endurance}");
        sb.Append($"\n\t{nameof(Agility)}: {Agility}");
        sb.Append($"\n\t{nameof(Luck)}: {Luck}");
        sb.Append($"\n\t{nameof(SkillId)}: {SkillId}");
        sb.Append($"\n\t{nameof(Price)}: {Price}");
        return sb.ToString();
    }

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