namespace P3R.WeaponFramework.Weapons.Models;

internal class WeaponConfig
{
    public string? Name { get; set; }

    public WeaponPartsData Base { get; set; } = new();
    public WeaponPartsData Mesh { get; set; } = new();

    public ushort? Rarity { get; set; }
    public ushort? Tier { get; set; }
    public ushort? Attack { get; set; }
    public ushort? Accuracy { get; set; }
    public ushort? Strength { get; set; }
    public ushort? Magic { get; set; }
    public ushort? Endurance { get; set;}
    public ushort? Agility { get; set; }
    public ushort? Luck { get; set; }

    public ushort? SortNum => Attack;

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