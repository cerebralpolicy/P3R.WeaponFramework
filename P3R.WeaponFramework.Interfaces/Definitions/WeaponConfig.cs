namespace P3R.WeaponFramework.Interfaces.Types;

public class WeaponConfig
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
            _ => throw new NotImplementedException(),
        };

    public class WeaponPartsData
    {
        public string? MeshPath { get; set; }
        public string? AnimPath { get; set; }
    }
}