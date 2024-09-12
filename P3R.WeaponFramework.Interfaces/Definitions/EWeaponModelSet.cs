namespace P3R.WeaponFramework.Interfaces.Types;

public enum EWeaponModelSet
{
    SEES = 0,
    Base = 1,
    Tier1A = 10,
    Tier1B = 11,
    Tier2A = 20,
    Tier2B = 21,
    GimmickA = 50,
    GimmickB = 51,
    LegendaryA = 60,
    LegendaryB = 61,
    LegendaryC = 62,
}
public static class WeaponModelExtensions
{
    public static int GetOtherModelID(this EWeaponModelSet weaponModelSet) => (int)weaponModelSet + 200;
}
