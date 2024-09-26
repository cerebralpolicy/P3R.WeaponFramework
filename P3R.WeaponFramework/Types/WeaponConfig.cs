using YamlDotNet.Serialization;

namespace P3R.WeaponFramework.Types;
[YamlSerializable]
public class WeaponConfig
{
    public WeaponConfig()
    {
    }

    public WeaponConfig(string? name, ShellType shell, WeaponMeshPart model, WeaponStats? stats)
    {
        Name = name;
        Shell = shell;
        Model = model;
        Stats = stats;
    }
    [YamlMember(Description = "[Optional] Specifies the name displayed in P3R")]
    public string? Name { get; set; }

    [YamlMember(Description = "Specifies the shell type used by the new weapon.")]
    public ShellType Shell { get; set; }
    [YamlMember(Description = "Specifies the model being used by the new weapon.")]
    public WeaponMeshPart Model { get; set; } = new();
    [YamlMember(Description = "Defines the stats for the weapon.")]
    public WeaponStats? Stats { get; set; } = new();

    [YamlIgnore]
    public int? SortNum => Stats?.Attack;
    [YamlIgnore]
    public bool? HasMultipleModels => Shell.RequiredMeshes() > 1;
    public string? GetAssetFile(WeaponAssetType assetType)
        => assetType switch
        {
            WeaponAssetType.Weapon_Mesh => Model?.MeshPath1,
            WeaponAssetType.Weapon_Mesh2 => Model?.MeshPath2,
            _ => throw new NotImplementedException(),
        };
}
[YamlSerializable]
public class WeaponMeshPart
{
    [YamlMember(Description = "Path to a model not inside the weapon folder.")]
    public string? MeshPath1 { get; set; } = null;
    [YamlMember(Description = "Path to a model not inside the weapon folder. Only need if adding a weapon to Akihiko or a dual-wield weapon to Aigis.")]
    public string? MeshPath2 { get; set; } = null;
    [YamlIgnore]
    internal string?[] MeshPaths => [MeshPath1, MeshPath2];
    public string[] DefinedPaths => MeshPaths.Where(path => path != null).ToArray()!;
}

public class WeaponBasePart
{
    public string? MeshPath { get; set; }
}