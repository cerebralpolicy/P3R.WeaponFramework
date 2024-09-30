using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using P3R.WeaponFramework.Utils;
using System.Text;
namespace P3R.WeaponFramework.Weapons.Models;


[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public class Weapon : IEquatable<Weapon?>, IWeapon
{
    private const string DEF_DESC = "[uf 0 5 65278][uf 2 1]A weapon added with Weapon Framework.[n][e]";
    #region ctors
    public Weapon() {
        ShellTarget = ShellType.None;
    }
    public Weapon(int weaponItemId)
    {
        ShellTarget = ShellType.None;
        WeaponItemId = weaponItemId;
    }
    [JsonConstructor]
    public Weapon(ECharacter character, bool isVanilla, bool isAstrea, int weaponId, string? name, ShellType shellTarget, int modelId, WeaponStats stats)
    {
        Character = character;
        IsVanilla = isVanilla;
        IsAstrea = isAstrea;
        WeaponId = weaponId;
        Name = name;
        ShellTarget = shellTarget;
        ModelId = modelId;
        Stats = stats;
        SortNum = SortUtils.GetSortNumber(stats, isAstrea);
    }


    #endregion
    #region status
    [JsonIgnore]
    public bool IsEnabled { get; set; }
    [JsonIgnore]
    public bool IsModded { get; set; }
    #endregion
    #region Weapon Table Entry
    // Import
    [JsonPropertyOrder(0)]
    [JsonPropertyName(nameof(Character))]
    public ECharacter Character { get; set; } = ECharacter.NONE;
    [JsonPropertyOrder(1)]
    [JsonPropertyName(nameof(IsVanilla))]
    public bool IsVanilla { get; set; } = true;
    [JsonPropertyOrder(2)]
    [JsonPropertyName(nameof(IsAstrea))]
    public bool IsAstrea { get; set; } = false;
    [JsonPropertyOrder(3)]
    [JsonPropertyName(nameof(WeaponId))]
    public int WeaponId { get; set; }
    [JsonPropertyOrder(4)]
    [JsonPropertyName(nameof(Name))]
    public string? Name
    {
        get => Config.Name;
        set => Config.Name = value;
    }
    [JsonPropertyOrder(5)]
    [JsonPropertyName(nameof(ShellTarget))]
    public ShellType ShellTarget { get; set; }
    [JsonPropertyOrder(6)]
    [JsonPropertyName(nameof(ModelId))]
    public int ModelId { get; set; }
    [JsonPropertyOrder(7)]
    [JsonPropertyName(nameof(Stats))]
    public WeaponStats Stats { get; set; }
    #endregion
    #region Creation Variables
    [JsonIgnore]
    public int WeaponItemId { get; set; }
    [JsonIgnore]
    public string Description { get; set; } = DEF_DESC;
    [JsonIgnore]
    public WeaponConfig Config { get; set; } = new();
    [JsonIgnore]
    public string? OwnerModId { get; set; }
    [JsonIgnore]
    public int SortNum { get; set; }
    #endregion
    #region Utilities
    public void SetWeaponItemId(int weaponItemId)
    {
        this.WeaponItemId = weaponItemId;
        if (this.Character == ECharacter.NONE || this.Name == "Unused")
            return;
        Log.Debug($"Set Weapon Item ID: {this.Character} || {this.Name} || {this.WeaponItemId}");
    }
    private List<string> GetPaths()
    {
        List<string> strings = [];
        ModUtils.IfNotNull(Config.Model.MeshPath1, path =>
        { strings.Add(path!); });
        ModUtils.IfNotNull(Config.Model.MeshPath2, path =>
        { strings.Add(path!); });
        return strings;
    }
    public bool TryGetPaths([NotNullWhen(true)] out List<string>? paths)
    {
        List<string> strings = GetPaths();
        var check = strings.Count > 0;
        paths = check ? strings : null;
        return check;
    }
    private void PopulatePaths()
    {
        var suffix = ModelPairsInt[ModelId];
        var path = GetVanillaAssetFile(Character, suffix);
        Config.Model.MeshPath1 = path;
        if (DualModels.Contains(ModelId))
        {
            suffix += 200;
            var path2 = GetVanillaAssetFile(Character, suffix);
            //Log.Debug($"Mesh Paths {Name} || Right Mesh Path: {path} || Left Mesh Path: {path2}");
            Config.Model.MeshPath2 = path2;
        }
    }
    
    public void InitAtlusWeapon()
    {
        var sb = new StringBuilder();
        sb.Append($"Activating weapon {WeaponId:X3}");
        IsEnabled = true;
        if (Name == "Unused" || Character == ECharacter.Fuuka || Character == ECharacter.NONE)
        {
            sb.Append(" || Unused.");
            Log.Verbose(sb.ToString());
            return; 
        }
        sb.Append($" || {this}");
        Log.Verbose(sb.ToString());
        PopulatePaths();
    }

    public static bool IsItemIdWeapon(int itemId) => itemId >= 0x7000 && itemId < 0x8000;

    public static int GetWeaponItemId(int itemId) => itemId - 0x7000;

    public static bool IsActive(Weapon weapon) => weapon.IsEnabled && weapon.Character != ECharacter.NONE;
    #endregion
    #region Comparisons
    public override bool Equals(object? obj)
    {
        return Equals(obj as Weapon);
    }

    public bool Equals(Weapon? other)
    {
        return other is not null &&
               Character == other.Character &&
               Stats.Equals(other.Stats);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Character, Stats);
    }

    public bool Equals(WeaponItem? other)
    {
        return other is not null &&
               Character == other.Character &&
               Stats.Equals(other.Stats);
    }

    public override string? ToString()
    {
        var sb = new StringBuilder();
        void ForEachPath(List<string> paths)
        {
            foreach (var path in paths)
            {
                sb.AppendLine($"\t{path}");
            }
        }
        sb.Append($"Character: {Character} || Weapon: {Name} || SortNum: {SortNum}\nShell Target: {ShellTarget} || ModelId: {ModelId}\n");
        if (BaseModels.Contains(this.ModelId))
        {
            sb.AppendLine($"Base Paths:");
            ForEachPath(ShellExtensions.ShellLookup[ShellTarget].BasePaths);
            sb.AppendLine($"Shell Paths:");
            ForEachPath(ShellExtensions.ShellLookup[ShellTarget].ShellPaths);
        }
        else
        {
            sb.AppendLine($"Base Paths:");
            ForEachPath(ShellExtensions.ShellLookup[ShellTarget].WeaponPaths(this));
        }

        return sb.ToString();
    }

    public static bool operator ==(Weapon? left, Weapon? right)
    {
        return EqualityComparer<Weapon>.Default.Equals(left, right);
    }

    public static bool operator !=(Weapon? left, Weapon? right)
    {
        return !(left == right);
    }
    #endregion
}
