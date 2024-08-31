namespace P3R.WeaponFramework.Weapons.Models;

internal class Weapon
{ 
    private const string DEF_DESC = "[f 2 1]A weapon added with Weapon Framework.[n][e]";
    public Weapon(Character character, int modelId, int weaponId, string name, int attack, int accuracy, int strength = 0, int magic = 0, int endurance = 0, int agility = 0, int luck = 0, int price = 400, int sellPrice = 100)
    {
        Character = character;
        ModelId = modelId;
        WeaponId = weaponId;
        Name = name;
        WeaponStats = new(attack,accuracy, strength, magic, endurance, agility, luck, price, sellPrice);
    }
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

    public int WeaponItemId { get; private set; }
    public int WeaponId { get; set; }
    public bool IsEnabled { get; set; }
    // Import
    public Character Character { get; set; } = Character.NONE;
    public int ModelId { get; set; }
    public WeaponStats WeaponStats { get; set; }





    public string Name { get; set; } = "Missing Name";

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
