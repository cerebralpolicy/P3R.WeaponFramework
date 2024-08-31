using P3R.WeaponFramework.Hooks.Models;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework.Hooks.Services;

internal static class AssetUtils
{
    public static string Format(this Character character) => character.ToString("0000");
    public static string Format(this WeaponModelSet weaponModelSet) => weaponModelSet.ToString("000");
    public static string GetAssetPath(string assetPath)
    {
        var formattedPath = assetPath.Replace("\\", "/").Replace(".uasset", string.Empty);
        if (!formattedPath.StartsWith("/Game/"))
        {
            formattedPath = $"/Game/{formattedPath}";
        }
        return formattedPath;
    }
    public static string? GetAssetPath(Character chara, WeaponModelSet model) => GetAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/Models/SK_Wp{chara.Format()}_{model.Format()}");

    public static Character GetCharFromEpuip(EquipFlag flag) => Enum.Parse<Character>(flag.ToString());
    public static EquipFlag GetEquipFromChar(Character character) => Enum.Parse<EquipFlag>(character.ToString());
    public static string GetCharIDString(Character character) => character.Format();
}