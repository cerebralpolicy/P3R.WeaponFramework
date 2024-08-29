namespace P3R.WeaponFramework.Weapons;

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