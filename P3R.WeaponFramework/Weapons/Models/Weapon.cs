using System.Text.Json.Serialization;

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
    public Weapon(Character character, int weaponItemId, string name, EquipFlag weaponType, int modelId, EWeaponModelSet weaponModelId, WeaponStats stats)
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
    public Character Character { get; set; } = Character.NONE;
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
    public EArmatureType? TargetArmature => Config.Armature;
    #endregion
    #region Utilities
    public bool AddToDT()
    {
        const string DT = "DT_WeaponFramework";
        if (TargetArmature == null)
            return false;
        ArmatureType type = TargetArmature;
        if (type == null) return false;
        var req = type.RequiredMeshes;
        var meshes = Config.Mesh;
        if (meshes == null || meshes.MeshPath1 == null) return false;
        // If the armature type requires multiple meshes, add to both of the corresponding DTs
        if (req == 2 && meshes.MeshPath2 != null)
        {
                   type.Value[1].AddMesh(WeaponItemId, meshes.MeshPath2, true, DT);
            return type.Value[0].AddMesh(WeaponItemId, meshes.MeshPath1, true, DT);
        }
        // Otherwise only add the first mesh
        return type.Value.First().AddMesh(WeaponItemId, meshes.MeshPath1, false, DT);
    }
    public void SetWeaponItemId(int weaponItemId)
    {
        this.WeaponItemId = weaponItemId;
    }

    public static bool IsItemIdWeapon(int itemId) => itemId >= 0x7000 && itemId < 0x8000;

    public static int GetWeaponItemId(int itemId) => itemId - 0x7000;

    public static bool IsActive(Weapon weapon) => weapon.IsEnabled && weapon.Character != Character.NONE;
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
