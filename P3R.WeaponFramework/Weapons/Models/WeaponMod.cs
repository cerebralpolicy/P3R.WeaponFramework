using YamlDotNet.Serialization;

namespace P3R.WeaponFramework.Weapons.Models;

public class WeaponMod
{
    const string MetadataFileName = "ModMetadata.yaml";
    const string OverridesFileName = "Overrides.yaml";
    public WeaponMod(string modId, string modDir) {
        var mainDir = Path.Join(modDir, "Weapons");
        var unrealDir = Path.Join(mainDir, "UnrealEssentials");
        var configDir = Path.Join(mainDir, "Config");
        var metadata = Utils.YamlSerializer.DeserializeFile<WeaponModMetadata>(Path.Join(configDir, MetadataFileName));
        ModId = modId;
        ModDir = modDir;
        ModTag = metadata.ModTag;
        MetadataFile = Path.Join(configDir, MetadataFileName);
        OverridesFile = Path.Join(configDir, OverridesFileName);
        UnrealDir = unrealDir;
        ConfigDir = configDir;
        ContentDir = Path.Join(unrealDir, "P3R", "Content");
        WeaponsDir = Path.Join(unrealDir, "P3R", "Content", "Weapons");
        xrd777Dir = Path.Join(unrealDir, "P3R", "Content", "Xrd777");
    }
    public string ModId { get; }
    public string ModDir { get; }
    public string ModTag { get; }
    public string MetadataFile { get; }
    public string OverridesFile { get; }
    public string UnrealDir { get; }
    public string ConfigDir { get; }
    public string ContentDir { get; }
    public string WeaponsDir { get; }
    public string xrd777Dir { get; }
};

public struct WeaponModMetadata
{
    [YamlMember(Description = "Tag used in mesh file names to disambiguate weapons from different mods.")]
    public string ModTag { get; set; }
}