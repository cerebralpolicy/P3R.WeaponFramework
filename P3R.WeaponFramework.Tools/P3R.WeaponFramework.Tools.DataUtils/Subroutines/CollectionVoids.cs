using MoreLinq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace P3R.WeaponFramework.Tools.DataUtils;

internal static partial class Subroutines
{
    public const string FModelPath = "P3R.WeaponFramework.Tools.DataUtils.FModel";
    public static string FModelResource(Episode episode, JsonFile jsonFile) => string.Join('.',FModelPath,episode.ToString(),jsonFile.ToString(),"json");
    public const string WeaponsPath = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Weapons.json";
    public const string WeaponsPath_Astrea = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Weapons_Astrea.json";
    public const string NamesPath = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Names.json";
    public const string NamesPath_Astrea = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Names_Astrea.json";
  
    private static List<string> DeserializeFModelNames(Episode episode, JsonFile jsonFile)
    {
        var resourcePath = FModelResource(episode, jsonFile);
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourcePath)!;
        using var reader = new StreamReader(stream);
        var listJson = reader.ReadToEnd();
        var list = JsonSerializer.Deserialize<FModelNameList[]>(listJson)!.First().Properties.First().Value.ToList();
        return list ?? [];
    }
    private static List<ExWeaponItemList> DeserializeFModelWeapons(Episode episode, JsonFile jsonFile)
    {
        var resourcePath = FModelResource(episode, jsonFile);
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourcePath)!;
        using var reader = new StreamReader(stream);
        var listJson = reader.ReadToEnd();
        var list = JsonSerializer.Deserialize<FModelWeaponList[]>(listJson)!.First().Properties.First().Value.ToList();
        return list ?? [];
    }
    internal static List<ExWeaponItemList> GetWeaponsFModel(Episode episode = Episode.Xrd777)
        => DeserializeFModelWeapons(episode, JsonFile.Weapons);
    internal static Dictionary<string, string> GetFModelKeyValuePairs(Episode episode)
    {
        Dictionary<string,string> valuePairs = [];
        var keyList = GetWeaponsFModel(episode).Select(x => x.ItemDef).ToList();
        var nameList = DeserializeFModelNames(episode, JsonFile.Names);
        for (int i = 0; i < keyList.Count; i++)
        {
            var key = keyList[i];
            var value = nameList[i];
            valuePairs.Add(key, value);
        }
        return valuePairs;
    }
    internal static List<Weapon> GetFModelWeapons(Episode episode)
    {
        var itemList = GetWeaponsFModel(episode);
        List<Weapon> weapons = [];
        var index = 0;
        foreach (var item in itemList)
        {
            weapons.Add(item.Cook(index, episode));
            index++;
        }
        return weapons;
    }
    internal static List<Weapon> GetWeaponRaws(Episode episode = Episode.Xrd777)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var weapStream = assembly.GetManifestResourceStream(episode == Episode.Astrea ? WeaponsPath_Astrea : WeaponsPath)!;
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

    internal static Dictionary<string, string> GetRawNameKeys(Episode episode = Episode.Xrd777)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var nameStream = assembly.GetManifestResourceStream(episode == Episode.Astrea ? NamesPath_Astrea : NamesPath)!;
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

    internal static List<Weapon> GetCharaWeapons(Episode episode = Episode.Xrd777)
    {
        Console.WriteLine($"Processing {Enum.GetName(episode)}");

        for (int i = 0; i <= (episode == Episode.Astrea ? 11 : 10); i++)
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
    private static ECharacter GetCharacter(this EEquipFlag flag)
    {
        var val = (int)flag;
        var log = Math.Log2(val);
        return (ECharacter)log;
    }
    private static Weapon Cook(this ExWeaponItemList weaponItem, int index, Episode episode)
    {
        var chara = weaponItem.EquipID.GetCharacter();
        var rawNames = GetFModelKeyValuePairs(episode);
        rawNames.TryGetValue(weaponItem.ItemDef, out var weaponEnName);
        var name = weaponEnName ?? "Unused";
        var weaponId = index;
        var stats = weaponItem.GetStats();

        var weapon = new Weapon(chara,episode,weaponId,name,weaponItem.ItemDef,weaponItem.WeaponType, weaponItem.GetFLG, weaponItem.ModelID, weaponItem.Flags, stats);
        return weapon;
    }
    private static Weapon Cook(this WeaponRaw weaponRaw, int index, Episode episode)
    {
        var chara = weaponRaw.EquipID.GetCharacter();
        var rawNames = RawNameKeys(episode);
        rawNames.TryGetValue(weaponRaw.Name, out var weaponEnName);
        var name = weaponEnName ?? weaponRaw.Name;
        var weaponId = index;

        var weapon = new Weapon(chara, episode, weaponId, name , weaponRaw.WeaponType, weaponRaw.ModelID, weaponRaw.WeaponStats);
        return weapon;
    }
    public static ShellDatabase ShellLookup => [
        new (ShellType.None, [],[], 0, false, false, true),
        new (ShellType.Unassigned, [],[], 0, false, false, true),
        new (ShellType.Player, [EArmature.Wp0001_01], [10, 11, 12, 13, 14, 15, 16, 17, 18, 19], 280),
        new (ShellType.Yukari, [EArmature.Wp0002_01], [20, 21, 22, 23, 24, 25, 26, 27, 28], 281),
        new (ShellType.Stupei, [EArmature.Wp0003_01], [30, 31, 32, 33, 34, 35, 36, 37, 38, 39], 282),
        new (ShellType.Akihiko, [EArmature.Wp0004_01, EArmature.Wp0004_02], [40, 41, 42, 43, 44, 45, 46, 47, 48], 283),
        new (ShellType.Mitsuru, [EArmature.Wp0005_01], [50, 51, 52, 53, 54, 55, 56, 57], 141),
        new (ShellType.Aigis_SmallArms, [EArmature.Wp0007_01, EArmature.Wp0007_02], [326, 327], 176),
        new (ShellType.Aigis_LongArms, [EArmature.Wp0007_03], [584, 585, 586, 587, 588, 589], 179),
        new (ShellType.Ken, [EArmature.Wp0008_01], [80, 81, 82, 83, 84, 85, 86, 87, 88, 89], 226),
        new (ShellType.Koromaru, [EArmature.Wp0009_01], [90, 91, 92, 93, 94, 95, 96, 97], 201),
        new (ShellType.Shinjiro, [EArmature.Wp0010_01], [100, 101, 102, 103, 104, 105], 251),
        new (ShellType.Metis, [EArmature.Wp0011_01], [100, 101, 102, 103, 104, 105, 106], 477, vanilla: false),
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



    private static List<Weapon> WeaponList(Episode episode = Episode.Xrd777)
        => episode switch
        {
            Episode.Xrd777 => WeaponList_Xrd777,
            Episode.Astrea => WeaponList_Astrea,
            _ => throw new NotImplementedException()
        };
    private static List<Weapon> WeaponList_Xrd777 => GetFModelWeapons(Episode.Xrd777);
    private static List<Weapon> WeaponList_Astrea => GetFModelWeapons(Episode.Astrea);
    private static Dictionary<string, string> RawNameKeys(Episode episode = Episode.Xrd777)
        => episode switch
        {
            Episode.Xrd777 => RawNameKeys_Xrd777,
            Episode.Astrea => RawNameKeys_Astrea,
            _ => throw new NotImplementedException()
        };
    private static Dictionary<string, string> RawNameKeys_Xrd777 => GetRawNameKeys();
    private static Dictionary<string, string> RawNameKeys_Astrea => GetRawNameKeys(Episode.Astrea);
    private static List<Weapon> CharaWeapons(Episode episode = Episode.Xrd777)
        => episode switch
        {
            Episode.Xrd777 => CharaWeapons_Xrd777,
            Episode.Astrea => CharaWeapons_Astrea,
            _ => throw new NotImplementedException()
        };
    private static List<Weapon> CharaWeapons_Xrd777 => GetCharaWeapons();
    private static List<Weapon> CharaWeapons_Astrea => GetCharaWeapons(Episode.Astrea);

    private static string GetResource(JsonFile filetype, Episode episode)
    {
        bool usesuffix = (episode.Equals(Episode.Astrea));
        string? listtype = Enum.GetName(typeof(JsonFile), filetype);
        string suffix = episode.Equals(Episode.Astrea) ? "_Astrea" : string.Empty;
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