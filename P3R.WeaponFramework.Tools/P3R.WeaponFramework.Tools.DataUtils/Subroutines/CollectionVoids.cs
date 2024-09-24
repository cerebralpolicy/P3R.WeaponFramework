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
    internal static List<Weapon> GetWeaponRaws(Episode episode = Episode.Journey)
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
            index++;
            weapons.Add(weapon.Cook(index,episode));
        }
        var weaponArray = weapons.Where(w => (w.WeaponType != 0 && w.Character != ECharacter.Fuuka) || w.Character == ECharacter.Fuuka).ToList();
        return weaponArray;
    }

    internal static Dictionary<string, string> GetRawNameKeys(Episode episode = Episode.Journey)
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

    internal static Dictionary<int, Weapon[]> GetCharaWeapons(Episode episode = Episode.Journey)
    {
        Console.WriteLine($"Processing {Enum.GetName(episode)}");

        Dictionary<int, Weapon[]> pairs = [];
        for (int i = 1; i <= (episode == Episode.Astrea ? 11 : 10); i++)
        {
            var charWeaps = WeaponList(episode).Where(w => w.Character == (ECharacter)i);
            if (charWeaps.Count() > 1)
            {
                Console.WriteLine($"{Enum.GetName((ECharacter)i)} has {charWeaps.Count()} weapons.");
                pairs.Add(i, charWeaps.ToArray());
            }
            else if (charWeaps.Count() > 0)
            {
                Console.WriteLine($"{Enum.GetName((ECharacter)i)} has {charWeaps.Count()} weapon.");
                pairs.Add(i, charWeaps.ToArray());
            }
        }
        return pairs;
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
        int weaponModelID = !IAssetUtils.ModelPairsInt.TryGetValue(weaponRaw.ModelID, out int value) ? weaponRaw.ModelID : value;
        var weapon = new Weapon(chara, uniqueIndex, name , weaponRaw.WeaponType, weaponRaw.ModelID, weaponModelID, weaponRaw.WeaponStats);
        return weapon;
    }



    private static List<Weapon> WeaponList(Episode episode = Episode.Journey)
        => episode switch
        {
            Episode.Journey => WeaponList_Xrd777,
            Episode.Astrea => WeaponList_Astrea,
            _ => throw new NotImplementedException()
        };
    private static List<Weapon> WeaponList_Xrd777 => GetWeaponRaws();
    private static List<Weapon> WeaponList_Astrea => GetWeaponRaws(Episode.Astrea);
    private static Dictionary<string, string> RawNameKeys(Episode episode = Episode.Journey)
        => episode switch
        {
            Episode.Journey => RawNameKeys_Xrd777,
            Episode.Astrea => RawNameKeys_Astrea,
            _ => throw new NotImplementedException()
        };
    private static Dictionary<string, string> RawNameKeys_Xrd777 => GetRawNameKeys();
    private static Dictionary<string, string> RawNameKeys_Astrea => GetRawNameKeys(Episode.Astrea);
    private static Dictionary<int, Weapon[]> CharaWeapons(Episode episode = Episode.Journey)
        => episode switch
        {
            Episode.Journey => CharaWeapons_Xrd777,
            Episode.Astrea => CharaWeapons_Astrea,
            _ => throw new NotImplementedException()
        };
    private static Dictionary<int, Weapon[]> CharaWeapons_Xrd777 => GetCharaWeapons();
    private static Dictionary<int, Weapon[]> CharaWeapons_Astrea => GetCharaWeapons(Episode.Astrea);

    private static string GetResource(JsonFile filetype, Episode episode)
    {
        bool usesuffix = (episode.Equals(Episode.Astrea));
        string? listtype = Enum.GetName(typeof(JsonFile), filetype);
        string suffix = episode.Equals(Episode.Astrea) ? "_Astrea" : string.Empty;
        return $"P3R.WeaponFramework.Tools.DataUtils.RawResources.{listtype}{suffix}.json";
    }
}