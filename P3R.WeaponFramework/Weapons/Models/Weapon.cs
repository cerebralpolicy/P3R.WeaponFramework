namespace P3R.WeaponFramework.Weapons.Models;

internal class Weapon
{ 
    private const string DEF_DESC = "[f 2 1]A weapon added with Weapon Framework.[n][e]";
    
    public Weapon(Character character, int modelId, int weaponId, string name)
    {
        Character = character;
        ModelId = modelId;
        WeaponId = weaponId;
        Name = name;
    }
    public Weapon(Character character, int modelId, string name)
    {
        Character = character;
        ModelId = modelId;
        Name = name;
        if (ModelId < 256)
            WeaponId = modelId % 10;
    }
    public Weapon(Character character, int modelId, int weaponId)
    {
        Character = character;
        ModelId = modelId;
        WeaponId = weaponId;
    }
    public Weapon(Character character, int modelId)
    {
        Character = character;
        ModelId = modelId;
    }
    public Weapon(int modelId)
    {
        ModelId = modelId;
    }

    public Weapon(Character character, string name, int modelId, int weaponId, WeaponStats weaponStats)
    {
        Character = character;
        Name = name;
        ModelId = modelId;
        WeaponId = weaponId;
        WeaponStats = weaponStats;
    }

    public int WeaponItemId { get; private set; }
    public bool IsEnabled { get; set; }
    // Import
    public Character Character { get; set; } = Character.NONE;
    public string Name { get; set; } = "Missing Name";
    public int ModelId { get; set; }
    public int WeaponId { get; set; }

    public WeaponStats WeaponStats { get; set; }

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
}
