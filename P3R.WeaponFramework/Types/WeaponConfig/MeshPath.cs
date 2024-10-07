using YamlDotNet.Serialization;

namespace P3R.WeaponFramework.Types;

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
