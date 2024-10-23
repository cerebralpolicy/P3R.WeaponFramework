using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework.Utils;

public static class PriceUtils
{
    const double slope = 0.0156893071491;
    const double power = 1.44199142635;
    const double stDev = 12864.4951913;
    const double tolerance = 0.25;
    static double composite(this WeaponStats weaponStats) => weaponStats.Attack * weaponStats.Accuracy;
    static uint price(this WeaponStats weaponStats)
    {
        var m = slope;
        var e = power;
        var x = weaponStats.composite();
        var p = Math.Pow(x,e);
        var result = m * p;
        return (uint)result;
    }
    public static uint GetPrice(ushort attack, ushort accuracy)
    {
        var m = slope;
        var e = power;
        var x = attack * accuracy;
        var p = Math.Pow(x,e);
        var result = m * p;
        return (uint)result;
    }
    static uint sellPrice(this WeaponStats stats) => stats.price() / 4;
    public static void VerifyPrices(this Weapon weapon)
    {
        if (IsPriceValid(weapon.Stats))
            return;
        else
            weapon.SetPrices();
    }
    private static bool IsPriceValid(WeaponStats stats)
    {
        var expectedPrice = stats.price();
        var actualPrice = stats.Price;
        var window = tolerance * stDev;
        return actualPrice <= expectedPrice + window && actualPrice >= expectedPrice - window;
    }
    public static void SetConfigPrices(this WeaponConfig config)
    {
        var configStats = config.Stats;
        
        var stats = configStats!;
        stats.Price = stats.price();
        stats.SellPrice = stats.sellPrice();
    }
    public static void SetConfigPrices(this Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));
        var stats = weapon.Stats;
        var configStats = weapon.Config.Stats!;
        stats.Price = configStats.price();
        stats.SellPrice = configStats.sellPrice();
    }
    public static void LoadConfigPrices(this Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));
        var stats = weapon.Stats;
        var configStats = weapon.Config.Stats!;
        stats.Price = configStats.Price;
        stats.SellPrice = configStats.SellPrice;
    }
    public static void SetPrices(this Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));
        var stats = weapon.Stats;
        stats.Price = stats.price();
        stats.SellPrice = stats.sellPrice();
    }
}
internal static class StatUtils
{
    public static ushort NullableField(ushort? @ushort)
    {
        if (@ushort == null) return 0;
        else return @ushort.Value;
    }
    public static uint NullableField(uint? @uint)
    {
        if (@uint == null) return 0;
        else return @uint.Value;
    }
}