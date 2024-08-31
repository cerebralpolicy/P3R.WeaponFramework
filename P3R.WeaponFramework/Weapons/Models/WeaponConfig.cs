namespace P3R.WeaponFramework.Weapons.Models;

internal class WeaponConfig
{
    public string? Name { get; set; }

    public WeaponPartsData Base { get; set; } = new();
    public WeaponPartsData Mesh { get; set; } = new();

    public WeaponStats? Stats { get; set; } = new();

    public int? SortNum => Stats?.Attack;

    public string? GetAssetFile(WeaponAssetType assetType)
        => assetType switch
        {
            WeaponAssetType.Base_Mesh => Base.MeshPath,
            WeaponAssetType.Weapon_Mesh => Mesh.MeshPath,
            WeaponAssetType.Base_Anim => Base.AnimPath,
            _ => throw new NotImplementedException(),
        };
}

internal class WeaponPartsData
{
    public string? MeshPath { get; set; }
    public string? AnimPath { get; set; }
}