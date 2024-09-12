namespace P3R.WeaponFramework.Interfaces.Types;

public class WeaponConfig
{
    public string? Name { get; set; }

    public EArmatureType? Armature { get; set; }

    public bool? HasMultipleModels { get; set; }

    public WeaponPartsData? Base { get; set; } = new();
    public WeaponPartsData? Mesh { get; set; } = new();

    public WeaponStats? Stats { get; set; } = new();

    public int? SortNum => Stats?.Attack;

    public string? GetAssetFile(WeaponAssetType assetType)
        => assetType switch
        {
            WeaponAssetType.Base_Mesh => Base?.MeshPath1,
            WeaponAssetType.Weapon_Mesh => Mesh?.MeshPath1,
            WeaponAssetType.Weapon_Mesh2 => Mesh?.MeshPath2,
            _ => throw new NotImplementedException(),
        };
}

public class WeaponPartsData
{
    public string? MeshPath1 { get; set; }
    public string? MeshPath2 { get; set; }
    public string? AnimPath { get; set; }
}

public class WeaponPart
{

}