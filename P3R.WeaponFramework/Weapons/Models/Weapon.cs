using P3R.WeaponFramework.Hooks.Models;
using P3R.WeaponFramework.Interfaces;
using System.Text.Json.Serialization;

namespace P3R.WeaponFramework.Weapons.Models;

internal class Weapon: IWeapon, IEquatable<Weapon?>
{ 
    private const string DEF_DESC = "[f 2 1]A weapon added with Weapon Framework.[n][e]";
    
    public Weapon() { }

    public Weapon(int weaponItemId)
    {
        WeaponItemId = weaponItemId;
    }
    [JsonConstructor]
    public Weapon(Character character, int weaponItemId, string name, EquipFlag weaponType, int modelId, WeaponModelSet weaponModelId, WeaponStats stats)
    {
        Character = character;
        WeaponItemId = weaponItemId;
        Name = name;
        WeaponType = weaponType;
        ModelId = modelId;
        WeaponModelId = weaponModelId;
        Stats = stats;
    }

    public bool IsVanilla { get; set; } = true;
    public bool IsEnabled { get; set; }
    // Import
    [JsonPropertyOrder(0)]
    public Character Character { get; set; } = Character.NONE;
    [JsonPropertyOrder(1)]
    public int WeaponId { get; set; }
    [JsonPropertyOrder(2)]
    public string Name { get; set; } = "Missing Name";
    [JsonPropertyOrder(3)]
    public EquipFlag WeaponType { get; set; } = EquipFlag.NONE;
    [JsonPropertyOrder(4)]
    public int ModelId { get; set; }
    [JsonPropertyOrder(5)]
    public WeaponModelSet WeaponModelId { get; set; }
    [JsonPropertyOrder(6)]
    public WeaponStats Stats { get; set; }
    public int WeaponItemId { get; set; }


    public string Description { get; set; } = DEF_DESC;

    public WeaponConfig Config { get; set; } = new();

    public string? OwnerModId { get; set; }

    public void SetWeaponItemId(int weaponItemId)
    {
        this.WeaponItemId = weaponItemId;
        Log.Debug($"{this.Name} set to Weapon Item ID: {this.WeaponItemId}");
    }

    public static bool IsItemIdWeapon(int itemId) => itemId >= 0x7000 && itemId < 0x7000;

    public static int GetWeaponItemId(int itemId) => itemId - 0x7000;

    public static bool IsActive(Weapon weapon) => weapon.IsEnabled && weapon.Character != Character.NONE;

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
}
