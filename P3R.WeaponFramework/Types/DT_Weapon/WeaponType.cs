namespace P3R.WeaponFramework.Weapons;

public enum WeaponType
{
    UNUSED = 0,
    Sword = 2,
    Bow = 4,
    /// <summary>
    /// LongSword
    /// </summary>
    LSword = 8,
    /// <summary>
    /// Knuckles
    /// </summary>
    Knuckl = 16,
    Rapier = 32,
    Armas = 64,
    Spear = 128,
    Dagger = 256,
    Blunt = 512,
    Halbrd = 1024, // Tenatively calling Metis' weapons 'Halberds' because Halberds are cool as fuck, deal with it.
}

internal static partial class WeaponExtensions
{
    const string Prefix = "IT_WEA_";
    public static WeaponType GetWeaponType(this string ItemDefString)
    {
        var subString = ItemDefString[(Prefix.Length + 1)..];
        var typeString = subString.Split('_', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        var valid = Enum.TryParse(typeString, true, out WeaponType weaponType);
        if (valid)
            return weaponType;
        else
            return WeaponType.UNUSED;
    }
}