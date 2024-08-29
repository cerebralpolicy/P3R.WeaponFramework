namespace P3R.WeaponFramework.Weapons.Models;

internal class Weapon
{ 
    private const string DEF_DESC = "[uf 0 5 65278][uf 2 1]Weapon added with Weapon Framework.[n][e]";

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
        if (ModelId < 256)
            WeaponId = modelId % 10;
    }
    public Weapon(int modelId)
    {
        ModelId = modelId;
        if (ModelId < 256)
            WeaponId = modelId % 10;
            var charId = modelId / 10;
            Character = (Character)Math.Floor((double)charId);
    }

    public int WeaponItemId { get; private set; }

    public int WeaponId { get; private set; }
    public int ModelId { get; set; }

    public bool IsEnabled { get; set; }

    public Character Character { get; set; } = Character.NONE;

    public string Name { get; set; } = "Missing Name";

    public string Description { get; set; } = DEF_DESC;

    public WeaponConfig Config { get; set; } = new();

    public string? OwnerModId { get; set; }

    public void SetWeaponItemId(int weaponItemId)
    {
        this.WeaponItemId = weaponItemId;
        Log.Debug($"{this.Name} set to Weapon Item ID: {this.WeaponItemId}");
    }

    public static bool IsItemIdWeapon(int itemId) => itemId >= 0x0000 && itemId < 0x1000;

    public static int GetWeaponItemId(int itemId) => itemId;

    public static bool IsActive(Weapon weapon) => weapon.IsEnabled && weapon.Character != Character.NONE;
}
