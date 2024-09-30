using MoreLinq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace P3R.WeaponFramework.Tools.DataUtils;

internal static partial class Subroutines
{
    public const string WeaponsPath = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Weapons.json";
    public const string WeaponsPath_Astrea = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Weapons_Astrea.json";
    public const string NamesPath = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Names.json";
    public const string NamesPath_Astrea = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Names_Astrea.json";
    internal static List<Weapon> GetWeaponRaws(Episode episode = Episode.VANILLA)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var weapStream = assembly.GetManifestResourceStream(episode == Episode.ASTREA ? WeaponsPath_Astrea : WeaponsPath)!;
        using var weapReader = new StreamReader(weapStream);
        var weapJson = weapReader.ReadToEnd();
        var weaponRaws = JsonSerializer.Deserialize<WeaponRaw[]>(weapJson)!;
        List<Weapon> weapons = [];
        var index = 0;
        foreach ( var weapon in weaponRaws )
        {
            weapons.Add(weapon.Cook(index,episode));
            index++;
        }
        return weapons;
    }

    internal static Dictionary<string, string> GetRawNameKeys(Episode episode = Episode.VANILLA)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var nameStream = assembly.GetManifestResourceStream(episode == Episode.ASTREA ? NamesPath_Astrea : NamesPath)!;
        using var nameReader = new StreamReader(nameStream);
        var nameJson = nameReader.ReadToEnd();
        var namePairs = JsonSerializer.Deserialize<KeyValuePair<string, string>[]>(nameJson)!;

        Dictionary<string, string> keyMap = [];
        foreach (var pair in namePairs)
        {
            keyMap.Add(pair.Key, pair.Value);
        }
        return keyMap;
    }

    internal static List<Weapon> GetCharaWeapons(Episode episode = Episode.VANILLA)
    {
        Console.WriteLine($"Processing {Enum.GetName(episode)}");

        for (int i = 0; i <= (episode == Episode.ASTREA ? 11 : 10); i++)
        {
            var charWeaps = WeaponList(episode).Where(w => w.Character == (ECharacter)i);
            if (charWeaps.Count() > 1)
            {
                Console.WriteLine($"{Enum.GetName((ECharacter)i)} has {charWeaps.Count()} weapons.");
            }
            else if (charWeaps.Count() > 0)
            {
                Console.WriteLine($"{Enum.GetName((ECharacter)i)} has {charWeaps.Count()} weapon.");
            }
        }
        return WeaponList(episode);
    }
    private static ECharacter GetCharacter(this EquipFlag flag)
    {
        var val = (int)flag;
        var log = Math.Log2(val);
        return (ECharacter)log;
    }

    private static Weapon Cook(this WeaponRaw weaponRaw, int index, Episode episode)
    {
        var chara = weaponRaw.EquipID.GetCharacter();
        var rawNames = RawNameKeys(episode);
        rawNames.TryGetValue(weaponRaw.Name, out var weaponEnName);
        var name = weaponEnName ?? weaponRaw.Name;
        var uniqueIndex = index;

        var weapon = new Weapon(chara, episode, uniqueIndex, name , weaponRaw.WeaponType, weaponRaw.ModelID, weaponRaw.WeaponStats);
        return weapon;
    }
    public static ShellDatabase ShellLookup => [
        new (ShellType.None, [],[0], true, true, true),
        new (ShellType.Unassigned, [],[], false, false, true),
        new (ShellType.Player, [EArmature.Wp0001_01], [10, 11, 12, 13, 14, 15, 16, 17, 18, 19]),
        new (ShellType.Yukari, [EArmature.Wp0002_01], [20, 21, 22, 23, 24, 25, 26, 27, 28]),
        new (ShellType.Stupei, [EArmature.Wp0003_01], [30, 31, 32, 33, 34, 35, 36, 37, 38, 39]),
        new (ShellType.Akihiko, [EArmature.Wp0004_01, EArmature.Wp0004_02], [40, 41, 42, 43, 44, 45, 46, 47, 48]),
        new (ShellType.Mitsuru, [EArmature.Wp0005_01], [50, 51, 52, 53, 54, 55, 56, 57]),
        new (ShellType.Aigis_SmallArms, [EArmature.Wp0007_01, EArmature.Wp0007_02], [326, 327]),
        new (ShellType.Aigis_LongArms, [EArmature.Wp0007_03], [584, 585, 586, 587, 588, 589]),
        new (ShellType.Ken, [EArmature.Wp0008_01], [80, 81, 82, 83, 84, 85, 86, 87, 88, 89]),
        new (ShellType.Koromaru, [EArmature.Wp0009_01], [90, 91, 92, 93, 94, 95, 96, 97]),
        new (ShellType.Shinjiro, [EArmature.Wp0010_01], [100, 101, 102, 103, 104, 105]),
        new (ShellType.Metis, [EArmature.Wp0011_01], [100, 101, 102, 103, 104, 105, 106]),
        ];
    public static ShellType ShellFromId(int modelId, bool astrea)
    {
        if (ShellLookup.All(x => !x.ModelIds.Contains(modelId)))
            return ShellType.None;
        else
            if (astrea)
                return ShellLookup.First(x => x.ModelIds.Contains(modelId) && x.Astrea).EnumValue;
            else
                return ShellLookup.First(x => x.ModelIds.Contains(modelId) && x.Vanilla).EnumValue;
    }



    private static List<Weapon> WeaponList(Episode episode = Episode.VANILLA)
        => episode switch
        {
            Episode.VANILLA => WeaponList_Xrd777,
            Episode.ASTREA => WeaponList_Astrea,
            _ => throw new NotImplementedException()
        };
    private static List<Weapon> WeaponList_Xrd777 => GetWeaponRaws();
    private static List<Weapon> WeaponList_Astrea => GetWeaponRaws(Episode.ASTREA);
    private static Dictionary<string, string> RawNameKeys(Episode episode = Episode.VANILLA)
        => episode switch
        {
            Episode.VANILLA => RawNameKeys_Xrd777,
            Episode.ASTREA => RawNameKeys_Astrea,
            _ => throw new NotImplementedException()
        };
    private static Dictionary<string, string> RawNameKeys_Xrd777 => GetRawNameKeys();
    private static Dictionary<string, string> RawNameKeys_Astrea => GetRawNameKeys(Episode.ASTREA);
    private static List<Weapon> CharaWeapons(Episode episode = Episode.VANILLA)
        => episode switch
        {
            Episode.VANILLA => CharaWeapons_Xrd777,
            Episode.ASTREA => CharaWeapons_Astrea,
            _ => throw new NotImplementedException()
        };
    private static List<Weapon> CharaWeapons_Xrd777 => GetCharaWeapons();
    private static List<Weapon> CharaWeapons_Astrea => GetCharaWeapons(Episode.ASTREA);

    private static string GetResource(JsonFile filetype, Episode episode)
    {
        bool usesuffix = (episode.Equals(Episode.ASTREA));
        string? listtype = Enum.GetName(typeof(JsonFile), filetype);
        string suffix = episode.Equals(Episode.ASTREA) ? "_Astrea" : string.Empty;
        return $"P3R.WeaponFramework.Tools.DataUtils.RawResources.{listtype}{suffix}.json";
    }
    public class EpisodeDictionary : Dictionary<int, List<Weapon>>
    {
        
    }
}
internal static partial class Subroutines
{
    public static EpisodeDictionary MergeDictionaries(this EpisodeDictionary first, EpisodeDictionary second)
    {
        var dict1count = first.Count;
        var dict2count = second.Count;
        EpisodeDictionary merged = [];
        for (int i = 1; i <= dict2count; i++) 
        { 
            if (i <= dict1count)
            {
                List<Weapon> weapons = [.. first[i]];
                weapons.AddRange([.. second[i]]);
                merged.Add(i, [.. weapons]);
            }
            else
            {
                merged.Add(i, [.. second[i]]);
            }
            continue;
        }
        return merged;
    }
}