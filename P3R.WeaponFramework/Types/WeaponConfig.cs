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

    public string? GetOrParseAssetPath(string? assetPath)
    {
        if (assetPath == null)
            return null;

        if (assetPath.StartsWith("asset:"))
        {
            var parts = assetPath["asset:".Length..].Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length < 2)
            {
                return null;
            }

            var character = Enum.Parse<ECharacter>(parts[0], true);
            var type = Enum.Parse<WeaponAssetType>(parts[1], true);
            var modelSet = WeaponModelSet.SEES;
            if (parts.Length == 3)
            {
                modelSet = Enum.Parse<WeaponModelSet>(parts[2], true);
            }

            return AssetUtils.GetAssetFile(character, modelSet, type);
        }


        if (assetPath.StartsWith("modAsset:"))
        {
            var parts = assetPath["modAsset:".Length..].Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length < 2)
            {
                return null;
            }
            var character = Enum.Parse<ECharacter>(parts[0], true);
            var subfolder = parts[1];
            var modelTypeName = parts[2];
            _ = int.TryParse(parts[3], out var modelTypeIndex);

            return AssetUtils.GetModAssetFile(character,subfolder,modelTypeName,modelTypeIndex);
        }

        return assetPath;
    }
}



[YamlStaticContext]
[YamlSerializable(typeof(string))]
public class MeshPath(string path) : IEquatable<MeshPath?>
{
    #region Formatter
    static string format(string path) => AssetUtils.FormatAssetPath(path);
    #endregion
    #region Equals & Hashcode
    public override bool Equals(object? obj) => Equals(obj as MeshPath);

    public bool Equals(MeshPath? other) => other is not null &&
               _path == other._path;
    
    public override int GetHashCode() => HashCode.Combine(_path);
    #endregion
    [YamlConverter(typeof(string))]
    private readonly string _path = format(path);
    public override string ToString() => _path;
    #region Operators
    public static implicit operator MeshPath(string path) => new MeshPath(path);

    public static bool operator ==(MeshPath? left, MeshPath? right) => EqualityComparer<MeshPath>.Default.Equals(left, right);

    public static bool operator !=(MeshPath? left, MeshPath? right) => !(left == right);
    #endregion
}

[YamlSerializable]
public class WeaponMeshPart
{
    [YamlMember(Description = "Path to a model not inside the weapon folder.")]
    public string? MeshPath1 { get; set; } = null;
    [YamlMember(Description = "Path to a model not inside the weapon folder. Only need if adding a weapon to Akihiko or a dual-wield weapon to Aigis.")]
    public string? MeshPath2 { get; set; } = null;
 }

public class WeaponBasePart
{
    public string? MeshPath { get; set; }
}