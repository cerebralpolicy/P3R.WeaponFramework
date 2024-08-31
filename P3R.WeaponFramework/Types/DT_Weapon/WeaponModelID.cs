namespace P3R.WeaponFramework.Weapons.Models;

 public enum WeaponModelID
{
    SEES,
    Base,
    Tier1A,
    Tier1B,
    Tier2A,
    Tier2B,
    GimmickA,
    GimmickB,
    LegendaryA,
    LegendaryB,
}
public enum WeaponModelSet
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
}

internal static partial class WeaponExtensions
{
    public static WeaponModelSet ToWeapModelSet(this WeaponModelID modelID)
    {
        var modelAlias = Enum.GetName(typeof(WeaponModelID), modelID);
        var valid = Enum.TryParse(modelAlias, out WeaponModelSet weaponModel);
        if (valid)
            return weaponModel;
        else
            return WeaponModelSet.Base;
    }
}