namespace P3R.WeaponFramework.Weapons;

internal static class Assets
{
    public static string Format(this Character character) => character.ToString("0000");
    public static string Format(this WeaponModelSet weaponModelSet) => weaponModelSet.ToString("000");
    public static string FormatAssetPath(string assetPath)
    {
        var formattedPath = assetPath.Replace("\\", "/").Replace(".uasset", string.Empty);
        if (!formattedPath.StartsWith("/Game/"))
        {
            formattedPath = $"/Game/{formattedPath}";
        }
        return formattedPath;
    }
    public static string GetExistingModelPath(Character chara, WeaponModelSet model) => FormatAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/Models/SK_Wp{chara.Format()}_{model.Format()}");
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
}