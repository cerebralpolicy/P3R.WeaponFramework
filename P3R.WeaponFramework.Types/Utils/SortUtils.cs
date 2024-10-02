using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework.Utils;

public static class SortUtils
{
    public static int GetSortNumber(WeaponStats stats, bool isAstrea)
    {
        return isAstrea ? stats.Attack * 5 : stats.Attack;
    }
    public static int GetSortNumber(this Weapon weapon)
    {
        return weapon.IsAstrea ? weapon.Stats.Attack * 5 : weapon.Stats.Attack;
    }
}