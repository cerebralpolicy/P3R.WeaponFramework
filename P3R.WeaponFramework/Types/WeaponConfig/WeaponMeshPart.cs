using YamlDotNet.Serialization;

namespace P3R.WeaponFramework.Types;

[YamlSerializable]
public class WeaponMeshPart
{
    [YamlMember(Description = "Path to a model not inside the weapon folder.")]
    public string? MeshPath1 { get; set; } = null;
    [YamlMember(Description = "Path to a model not inside the weapon folder. Only need if adding a weapon to Akihiko or a dual-wield weapon to Aigis.")]
    public string? MeshPath2 { get; set; } = null;
}
