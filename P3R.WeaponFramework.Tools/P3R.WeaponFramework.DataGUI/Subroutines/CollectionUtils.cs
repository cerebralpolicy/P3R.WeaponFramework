using P3R.WeaponFramework.Types;
using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.DataGUI;

internal static partial class Subroutines
{
    public const string WeaponsPath_Astrea = "P3R.WeaponFramework.Tools.DataGUI.Data.Astrea.DatItemWeaponDataAsset.COPY";
    public const string WeaponsPath_Xrd777 = "P3R.WeaponFramework.Tools.DataGUI.Data.Xrd777.DatItemWeaponDataAsset.COPY";
    public const string NamesPath_Astrea = "P3R.WeaponFramework.Tools.DataGUI.Data.Astrea.Names.json";
    public const string NamesPath_Xrd777 = "P3R.WeaponFramework.Tools.DataGUI.Data.Xrd777.Names.json";
    internal static List<FWeaponItemList> GetFWeapons(bool astrea = false)
    {
        string file = astrea ? WeaponsPath_Astrea : WeaponsPath_Xrd777;
        return CopyFileSerializer.DeserializeDataAsset<FWeaponItemList>(file);
    }
    internal static List<Weapon> GetWeapons(bool astrea = false)
    {
        var fWeapons = GetFWeapons(astrea);
        var weapons = new List<Weapon>();
        foreach (var fWeapon in fWeapons)
        {
            weapons.Add((Weapon)fWeapon);
        }
        return weapons;
    }
    internal static Dictionary<string, string> GetRawNameKeys(bool astrea = false)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var nameStream = assembly.GetManifestResourceStream(astrea ? NamesPath_Astrea : NamesPath_Xrd777)!;
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
    public static ECharacter GetCharFromEquip(this EEquipFlag flag)
    => Enum.Parse<ECharacter>(flag.ToString());

    public static EEquipFlag GetEquipFromChar(this ECharacter character)
        => Enum.Parse<EEquipFlag>(character.ToString());

}
