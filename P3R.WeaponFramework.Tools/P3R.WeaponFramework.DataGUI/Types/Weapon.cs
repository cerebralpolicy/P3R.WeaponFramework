using P3R.WeaponFramework.Types;
using System.Text.Json.Serialization;

namespace P3R.WeaponFramework.DataGUI;

public enum EEpisode
{
    Vanilla,
    Astrea
}
public struct Weapon
{
    public Weapon() { }
    public Weapon(ECharacter character, bool astrea, int uniqueID, string name, int weaponType, int modelId, WeaponStats stats)
    {
        Character = character;
        IsVanilla = !astrea;
        IsAstrea = astrea;
        WeaponId = uniqueID;
        Name = name;
        WeaponType = weaponType;
        ModelId = modelId;
        Stats = stats;
        ShellType = Subroutines.ShellFromId(modelId, astrea);
    }

    [JsonPropertyName("Character")]
    public ECharacter Character { get; set; }
    [JsonPropertyName("IsVanilla")]
    public bool IsVanilla { get; set; }
    [JsonPropertyName("IsAstrea")]
    public bool IsAstrea { get; set; }
    [JsonPropertyName("WeaponId")]
    public int WeaponId { get; set; }
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    [JsonPropertyName("WeaponType")]
    public int WeaponType { get; set; }
    [JsonPropertyName("ModelId")]
    public int ModelId { get; set; }
    [JsonPropertyName("ShellTarget")]
    public ShellType ShellType { get; set; }
    [JsonPropertyName("Stats")]
    public WeaponStats Stats { get; set; }
    public static ShellType GetShell(int modelId, bool isAstrea)
    {
        if (modelId >= 584)
            return ShellType.Aigis_LongArms;
        else if (modelId >= 326)
            return ShellType.Aigis_SmallArms;
        else if (modelId >= 100)
        {
            if (isAstrea)
                return ShellType.Metis;
            else
                return ShellType.Shinjiro;
        }
        else if (modelId >= 90)
            return ShellType.Koromaru;
        else if (modelId >= 80)
            return ShellType.Ken;
        else if (modelId >= 50)
            return ShellType.Mitsuru;
        else if (modelId >= 40)
            return ShellType.Akihiko;
        else if (modelId >= 30)
            return ShellType.Stupei;
        else if (modelId >= 20)
            return ShellType.Yukari;
        else if (modelId >= 10)
            return ShellType.Player;
        else if (modelId == 8)
            return ShellType.Unassigned;
        else
            return ShellType.None;
    }
    public static explicit operator Weapon(FWeaponItemList item)
    {
        var astrea = item.SortNum == 4 * item.Attack;
        WeaponStats stats = new WeaponStats()
        {
            AttrId = item.AttrID,
            Rarity = item.Rarity,
            Tier = item.Tier,
            Attack = item.Attack,
            Accuracy = item.Accuracy,
            Strength = item.Strength,
            Magic = item.Magic,
            Endurance = item.Endurance,
            Agility = item.Agility,
            Luck = item.Luck,
            SkillId = item.SkillID,
            Price = (int)item.Price,
            SellPrice = (int)item.SellPrice,
        };
        Weapon weapon = new()
        {
            Character = item.EquipID.GetCharFromEquip(),
            Name = item.GetName(astrea),
            IsVanilla = !astrea,
            IsAstrea = astrea,
            WeaponType = (int)item.WeaponType,
            ModelId = item.ModelID,
            ShellType = GetShell(item.ModelID, astrea),
            Stats = stats,
        };
        return weapon;
    }
}
internal static partial class Subroutines
{
    public static string GetName(this FWeaponItemList item, bool astrea) => GetRawNameKeys(astrea)[item.ItemDef];
}