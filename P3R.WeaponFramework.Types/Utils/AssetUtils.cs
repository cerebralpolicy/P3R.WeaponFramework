using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework;
public static partial class AssetUtils
{
    public static string? GetAssetFile(ECharacter chara, WeaponModelSet model, WeaponAssetType type)
    {
        string? assetFile = type switch
        {
            WeaponAssetType.Base_Mesh => GetAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/SKEL_Wp{chara.Format()}"),
            WeaponAssetType.Weapon_Mesh => GetAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/Models/SK_Wp{chara.Format()}_{model.Format()}"),
            WeaponAssetType.Weapon_Mesh2 => GetAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/Models/SK_Wp{chara.Format()}_{(model + 200):D3}"),

            WeaponAssetType.Base_Anim => GetAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/SKEL_Wp{chara.Format()}"),
            WeaponAssetType.Weapon_Anim => null,
            _ => throw new Exception(),
        };
        return assetFile;
    }
    public static string GetModAssetFile(ECharacter chara, string subfolder, string modelTypeName, int modelTypeIndex) => GetAssetPath($"$/Game/Xrd777/Characters/Weapon/{subfolder}/SK_Wp{chara.Format()}_{modelTypeName}{modelTypeIndex}");
    public static string GetVanillaAssetFile(ECharacter chara, int modelSuffix) => GetAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/Models/SK_Wp{chara.Format()}_{modelSuffix:000}");
    public static string GetUnrealAssetPath(string assetFile)
    {
        var assetPath = GetAssetPath(assetFile);
        return $"{assetPath}.{Path.GetFileName(assetPath)}";
    }
    public static ECharacter GetCharFromEquip(EEquipFlag flag)
    => Enum.Parse<ECharacter>(flag.ToString());

    public static EEquipFlag GetEquipFromChar(ECharacter character)
        => Enum.Parse<EEquipFlag>(character.ToString());

    public static string Format(this ECharacter character) => ((int)character).ToString("0000");
    public static string Format(this WeaponModelSet weaponModelSet) => ((int)weaponModelSet).ToString("000");
    public static string FormatAssetPath(string assetPath)
    {
        var formattedPath = assetPath.Replace("\\", "/").Replace(".uasset", string.Empty);
        if (!formattedPath.StartsWith("/Game/"))
        {
            formattedPath = $"/Game/{formattedPath}";
        }
        return formattedPath;
    }
    public static string GetAssetPath(string assetFile)
    {
        var adjustedPath = assetFile.Replace('\\', '/').Replace(".uasset", string.Empty);

        if (adjustedPath.IndexOf("Content") is int contentIndex && contentIndex > -1)
        {
            adjustedPath = adjustedPath.Substring(contentIndex + 8);
        }

        if (!adjustedPath.StartsWith("/Game/"))
        {
            adjustedPath = $"/Game/{adjustedPath}";
        }
        return adjustedPath;
    }
    public static Dictionary<uint, uint> ModelPairsUInt = new Dictionary<uint, uint>()
    {
        { 0 , 0 }, // So Fuuka doesn't fail
        { 10, 0 },
        { 11, 1 },
        { 12, 2 },
        { 13, 3 },
        { 14, 4 },
        { 15, 5 },
        { 16, 6 },
        { 17, 7 },
        { 18, 8 },
        { 19, 9 },
        { 20, 0 },
        { 21, 1 },
        { 22, 2 },
        { 23, 3 },
        { 24, 4 },
        { 25, 5 },
        { 26, 6 },
        { 27, 7 },
        { 28, 8 },
        { 30, 0 },
        { 31, 1 },
        { 32, 2 },
        { 33, 3 },
        { 34, 4 },
        { 35, 5 },
        { 36, 6 },
        { 37, 7 },
        { 38, 8 },
        { 39, 9 },
        { 40, 0 },
        { 41, 1 },
        { 42, 2 },
        { 43, 3 },
        { 44, 4 },
        { 45, 5 },
        { 46, 6 },
        { 47, 7 },
        { 48, 8 },
        { 50, 0 },
        { 51, 1 },
        { 52, 2 },
        { 53, 3 },
        { 54, 4 },
        { 55, 5 },
        { 56, 6 },
        { 57, 7 },
        { 80, 0 },
        { 81, 1 },
        { 82, 2 },
        { 83, 3 },
        { 84, 4 },
        { 85, 5 },
        { 86, 6 },
        { 87, 7 },
        { 88, 8 },
        { 89, 9 },
        { 90, 0 },
        { 91, 1 },
        { 92, 2 },
        { 93, 3 },
        { 94, 4 },
        { 95, 5 },
        { 96, 6 },
        { 97, 7 },
        { 100, 0 },
        { 101, 1 },
        { 102, 2 },
        { 103, 3 },
        { 104, 4 },
        { 105, 6 },
        { 326, 0 },
        { 327, 1 },
        { 584, 2 },
        { 585, 3 },
        { 586, 4 },
        { 587, 5 },
        { 588, 6 },
        { 589, 8 },
    };
    public static List<int> DualModels { get; } = [40, 41, 42, 43, 44, 45, 46, 47, 48, 326, 327];
    public static List<int> BaseModels { get; } = [0, 0, 10, 20, 30, 40, 50, 60, 326, 584, 80, 90, 100, 100];
    public static WeaponModelSet SetFromWeapon(this Weapon weapon)
    {
        var shellIndex = Enum.GetValues<ShellType>().ToList();
        int model;
        if (weapon.IsModded)
        {
            model = BaseModels[shellIndex.IndexOf(weapon.ShellTarget)];
        }
        else
        {
            model = weapon.ModelId;
        }
        var suffix = ModelPairsInt[model];
        return (WeaponModelSet)suffix;
    }
    public static Dictionary<int, int> ModelPairsInt { get; } = new Dictionary<int, int>()
    {
        { 0, 0 }, // So Fuuka doesn't fail
        { 10, 0 },
        { 11, 1 },
        { 12, 10 },
        { 13, 11 },
        { 14, 20 },
        { 15, 21 },
        { 16, 50 },
        { 17, 51 },
        { 18, 60 },
        { 19, 61 },
        { 20, 0 },
        { 21, 1 },
        { 22, 10 },
        { 23, 11 },
        { 24, 20 },
        { 25, 21 },
        { 26, 50 },
        { 27, 51 },
        { 28, 60 },
        { 30, 0 },
        { 31, 1 },
        { 32, 10 },
        { 33, 11 },
        { 34, 20 },
        { 35, 21 },
        { 36, 50 },
        { 37, 51 },
        { 38, 60 },
        { 39, 61 },
        { 40, 0 },
        { 41, 1 },
        { 42, 10 },
        { 43, 11 },
        { 44, 20 },
        { 45, 21 },
        { 46, 50 },
        { 47, 51 },
        { 48, 60 },
        { 50, 0 },
        { 51, 1 },
        { 52, 10 },
        { 53, 11 },
        { 54, 20 },
        { 55, 21 },
        { 56, 50 },
        { 57, 51 },
        { 70, 0 },
        { 71, 1 },
        { 72, 10 },
        { 73, 11 },
        { 74, 20 },
        { 75, 21 },
        { 76, 50 },
        { 77, 51 },
        { 78, 60 },
        { 79, 61 },
        { 80, 0 },
        { 81, 1 },
        { 82, 10 },
        { 83, 11 },
        { 84, 20 },
        { 85, 21 },
        { 86, 50 },
        { 87, 51 },
        { 88, 60 },
        { 89, 61 },
        { 90, 0 },
        { 91, 1 },
        { 92, 10 },
        { 93, 11 },
        { 94, 20 },
        { 95, 21 },
        { 96, 50 },
        { 97, 51 },
        { 100, 0 },
        { 101, 10 },
        { 102, 20 },
        { 103, 50 },
        { 104, 60 },
        { 105, 61 },
        { 106, 1 },
        { 110, 0 },
        { 111, 1 },
        { 112, 10 },
        { 113, 11 },
        { 114, 20 },
        { 115, 21 },
        { 116, 50 },
        { 117, 51 },
        { 118, 60 },
        { 119, 61 },
        { 326, 10 },
        { 327, 11 },
        { 584, 0 },
        { 585, 1 },
        { 586, 20 },
        { 587, 21 },
        { 588, 50 },
        { 589, 60 },
    };


}