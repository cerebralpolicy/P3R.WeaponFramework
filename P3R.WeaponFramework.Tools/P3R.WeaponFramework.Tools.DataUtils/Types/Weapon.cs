using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Tools.DataUtils;



public struct Weapon
{
    public Weapon(ECharacter character, Episode episode, int uniqueID, string name, int weaponType, int modelId, WeaponStats stats)
    {
        Character = character;
        IsVanilla = episode == Episode.Xrd777;
        IsAstrea = episode == Episode.Astrea;
        WeaponId = uniqueID;
        ItemDef = name;
        Name = name;
        WeaponType = weaponType;
        ModelId = modelId;
        Stats = stats;
        ShellType = Subroutines.ShellFromId(modelId, episode == Episode.Astrea);
    }

    public Weapon(ECharacter character, Episode episode, int weaponId, string name, string itemDef, int weaponType, int getFLG, int modelId, int flags, WeaponStats stats) : this()
    {
        Character = character;
        IsVanilla = episode == Episode.Xrd777;
        IsAstrea = episode == Episode.Astrea;
        WeaponId = weaponId;
        ItemDef = itemDef;
        Name = name;
        WeaponType = weaponType;
        GetFLG = getFLG;
        ModelId = modelId;
        ShellType = Subroutines.ShellFromId(modelId, episode == Episode.Astrea);
        Flags = flags;
        Stats = stats;
    }

    [JsonPropertyName("Character")]
    public ECharacter Character { get; set; }
    [JsonPropertyName("IsVanilla")]
    public bool IsVanilla { get; set; }
    [JsonPropertyName("IsAstrea")]
    public bool IsAstrea { get; set; }
    [JsonPropertyName("WeaponId")]
    public int WeaponId { get; set; }
    [JsonPropertyName("ItemDef")]
    public string ItemDef { get; set; }
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    [JsonPropertyName("WeaponType")]
    public int WeaponType { get; set; }
    [JsonPropertyName("GetFLG")]
    public int GetFLG {  get; set; }
    [JsonPropertyName("ModelId")]
    public int ModelId { get; set; }
    [JsonPropertyName("Flags")]
    public int Flags {  get; set; }
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
}
internal partial class Subroutines
{
    
}