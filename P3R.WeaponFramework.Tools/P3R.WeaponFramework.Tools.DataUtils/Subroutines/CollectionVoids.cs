using System.Reflection;
using System.Text;
using System.Text.Json;

namespace P3R.WeaponFramework.Tools.DataUtils;

internal static partial class Subroutines
{
    internal static List<Weapon> GetWeaponRaws()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var weapFile = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Weapons.json";
        using var weapStream = assembly.GetManifestResourceStream(weapFile)!;
        using var weapReader = new StreamReader(weapStream);
        var weapJson = weapReader.ReadToEnd();
        var weaponRaws = JsonSerializer.Deserialize<WeaponRaw[]>(weapJson)!;
        List<Weapon> weapons = [];
        var index = 0;
        foreach ( var weapon in weaponRaws )
        {
            index++;
            weapons.Add(weapon.Cook(index));
        }
        var weaponArray = weapons.Where(w => (w.WeaponType != 0 && w.Character != Character.Fuuka) || w.Character == Character.Fuuka).ToList();
        return weaponArray;
    }

    internal static Dictionary<string, string> GetRawNameKeys()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var nameFile = "P3R.WeaponFramework.Tools.DataUtils.RawResources.Names.json";
        using var nameStream = assembly.GetManifestResourceStream(nameFile)!;
        using var nameReader = new StreamReader(nameStream);
        var nameJson = nameReader.ReadToEnd();
        var namePairs = JsonSerializer.Deserialize<KeyValuePair<string, string>[]>(nameJson)!;

        Dictionary<string, string> keyMap = new Dictionary<string, string>();
        foreach (var pair in namePairs)
        {
            keyMap.Add(pair.Key, pair.Value);
        }
        return keyMap;
    }

    internal static Dictionary<int, Weapon[]> GetCharaWeapons()
    {
        Dictionary<int, Weapon[]> pairs = new Dictionary<int, Weapon[]>();
        for (int i = 1; i <= (int)Character.Shinjiro; i++)
        {
            var charWeaps = WeaponList.Where(w => w.Character == (Character)i);
            if (charWeaps.Count() > 1)
            {
                Console.WriteLine($"{Enum.GetName((Character)i)} has {charWeaps.Count()} weapons.");
                pairs.Add(i, charWeaps.ToArray());
            }
            else if (charWeaps.Count() > 0)
            {
                Console.WriteLine($"{Enum.GetName((Character)i)} has {charWeaps.Count()} weapon.");
                pairs.Add(i, charWeaps.ToArray());
            }
        }
        return pairs;
    }
    private static Character GetCharacter(this EquipFlag flag)
    {
        var val = (int)flag;
        var log = Math.Log2(val);
        return (Character)log;
    }

    private static Weapon Cook(this WeaponRaw weaponRaw, int index)
    {
        var chara = weaponRaw.EquipID.GetCharacter();
        string name;
        if (!RawNameKeys.ContainsKey(weaponRaw.Name))
            name = weaponRaw.Name;
        else
            name = RawNameKeys[weaponRaw.Name];
        var uniqueIndex = index;
        int weaponModelID;
        if (!Assets.ModelPairsInt.ContainsKey(weaponRaw.ModelID))
            weaponModelID = weaponRaw.ModelID;
        else
            weaponModelID = Assets.ModelPairsInt[weaponRaw.ModelID];
        var weapon = new Weapon(chara, uniqueIndex, name,weaponRaw.WeaponType, weaponRaw.ModelID, weaponModelID, weaponRaw.WeaponStats);
        return weapon;
    }

    private static List<Weapon> WeaponList => GetWeaponRaws();
    private static Dictionary<string, string> RawNameKeys => GetRawNameKeys();
    private static Dictionary<int, Weapon[]> CharaWeapons => GetCharaWeapons();
}