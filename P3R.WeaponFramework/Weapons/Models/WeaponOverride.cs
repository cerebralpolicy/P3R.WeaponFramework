namespace P3R.WeaponFramework.Weapons.Models;

internal class WeaponOverride
{
    public Character Character { get; set; }
    public int OriginalWeaponID { get; set; }
    public string NewWeaponName { get; set; } = string.Empty;
}
