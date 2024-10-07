namespace P3R.WeaponFramework.Weapons.Models;

internal class WeaponOverride
{
    public ECharacter Character { get; set; }
    public FEpisode Episode { get; set; }
    public int OriginalWeaponID { get; set; }
    public string NewWeaponName { get; set; } = string.Empty;
}

internal class WeaponOverrideSerialized
{
    public string Character { get; set; } = string.Empty;
    public string Episode { get; set; } = string.Empty;
    public string OriginalWeaponID { get; set; } = string.Empty;
    public string NewWeaponName { get; set; } = string.Empty;
}
