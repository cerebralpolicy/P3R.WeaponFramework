namespace P3R.WeaponFramework.Types
{
    public interface IWeaponConfig
    {
        bool? HasMultipleModels { get; }
        WeaponMeshPart Model { get; set; }
        string? Name { get; set; }
        ShellType Shell { get; set; }
        WeaponStats? Stats { get; set; }

        string? GetAssetFile(WeaponAssetType assetType);
        string? GetOrParseAssetPath(string? assetPath);
    }
}