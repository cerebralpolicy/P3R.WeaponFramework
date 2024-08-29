namespace P3R.WeaponFramework.Weapons;

public record AssetFNames(string assetFile)
{
    public string AssetName { get; } = Path.GetFileNameWithoutExtension(assetFile);
    public string AssetPath { get; } = Assets.GetAssetPath(assetFile);
}