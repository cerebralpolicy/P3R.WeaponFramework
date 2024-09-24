using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Weapons.Models;

public class Weapon: IWeapon, IEquatable<Weapon?>
{ 
    private const string DEF_DESC = "[f 2 1]A weapon added with Weapon Framework.[n][e]";
    #region ctors
    public Weapon() { }
    public Weapon(int weaponItemId)
    {
        WeaponItemId = weaponItemId;
    }
    [JsonConstructor]
    public Weapon(ECharacter character, int weaponItemId, string name, EquipFlag weaponType, int modelId, EWeaponModelSet weaponModelId, WeaponStats stats)
    {
        Character = character;
        WeaponItemId = weaponItemId;
        Name = name;
        WeaponType = weaponType;
        ModelId = modelId;
        WeaponModelId = weaponModelId;
        Stats = stats;
    }
    #endregion
    #region status
    public bool IsVanilla { get; set; } = true;
    public bool IsEnabled { get; set; }
    #endregion
    #region Weapon Table Entry
    // Import
    [JsonPropertyOrder(0)]
    public ECharacter Character { get; set; } = ECharacter.NONE;
    [JsonPropertyOrder(1)]
    public int WeaponId { get; set; }
    [JsonPropertyOrder(2)]
    public string? Name {
        get => Config.Name;
        set => Config.Name = value;
    }
    [JsonPropertyOrder(3)]
    public EquipFlag WeaponType { get; set; } = EquipFlag.NONE;
    [JsonPropertyOrder(4)]
    public int ModelId { get; set; }
    [JsonPropertyOrder(5)]
    public EWeaponModelSet WeaponModelId { get; set; }
    [JsonPropertyOrder(6)]
    public WeaponStats Stats { get; set; }
    #endregion
    #region Creation Variables
    public int WeaponItemId { get; set; }
    public string Description { get; set; } = DEF_DESC;
    public WeaponConfig Config { get; set; } = new();
    public string? OwnerModId { get; set; }
    public ShellType ShellTarget { get; set; } = ShellType.Player;
    public EpisodeFlag EpisodeFlag { get; set; } = EpisodeFlag.None;
    #endregion
    #region Utilities
    public void SetWeaponItemId(int weaponItemId)
    {
        this.WeaponItemId = weaponItemId;
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
    public bool TryGetPath(WeaponAssetType assetType,[NotNullWhen(true)] out string? path)
    {
        path = null;
        if (!TryGetPaths(out List<string>? paths)) { return false; }
        string? str = assetType switch
        {
            WeaponAssetType.Base_Mesh => paths.FirstOrDefault(),
            WeaponAssetType.Base_Mesh2 => paths.LastOrDefault(),
            _ => throw new NotImplementedException(),
        };
        if (str == null) { return false; }
        path = str;
        return true;
    }
    public bool ActivatePaths(IUnreal unreal, string idName) {  
        List<string> bases = ShellTarget.GetBasePaths();
        if (!TryGetPaths(out List<string>? paths)) { return false; }
        if (paths.Count != bases.Count) { return false; }
        for (int i = 0; i < bases.Count; i++)
        {
            var oldPath = bases[i];
            var newPath = paths[i];
            if (newPath == oldPath) { return false; }
            unreal.AssignFName(idName, oldPath, newPath);
            continue;
        }
        return true;
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
