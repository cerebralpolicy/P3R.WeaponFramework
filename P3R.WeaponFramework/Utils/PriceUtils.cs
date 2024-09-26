using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework.Utils;

public static class PriceUtils
{
    const double slope = 0.0156893071491;
    const double power = 1.44199142635;
    const double stDev = 12864.4951913;
    const double tolerance = 0.25;

    public static void VerifyPrices(this Weapon weapon)
    {
        if (weapon.Config.Stats == null)
        {
            weapon.SetPrices();
            return;
        }
        if (IsPriceValid(weapon.Config.Stats.Value))
            weapon.LoadConfigPrices();
        else
            weapon.SetConfigPrices();
    }
    private static bool IsPriceValid(WeaponStats stats)
    {
        var expectedPrice = stats.GetBuyPrice();
        var actualPrice = stats.Price;
        var window = tolerance * stDev;
        return actualPrice <= expectedPrice + window && actualPrice >= expectedPrice - window;
    }
    public static void SetConfigPrices(this WeaponConfig config)
    {
        var configStats = config.Stats;
        if (!configStats.HasValue)
            return;
        var stats = configStats.Value;
        stats.Price = stats.GetBuyPrice();
        stats.SellPrice = stats.GetSellPrice();
    }
    public static void SetConfigPrices(this Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));
        var stats = weapon.Stats;
        var configStats = weapon.Config.Stats!.Value;
        stats.Price = configStats.GetBuyPrice();
        stats.SellPrice = configStats.GetSellPrice();
    }
    public static void LoadConfigPrices(this Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));
        var stats = weapon.Stats;
        var configStats = weapon.Config.Stats!.Value;
        stats.Price = configStats.Price;
        stats.SellPrice = configStats.SellPrice;
    }
    public static void SetPrices(this Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));
        var stats = weapon.Stats;
        stats.Price = stats.GetBuyPrice();
        stats.SellPrice = stats.GetSellPrice();
    }
    public static int GetBuyPrice(int attack, int accuracy)
    {
        var composite = (double)attack * accuracy;
        var raw = Math.Pow(slope * composite, power);
        var result = Math.Floor(raw);
        return (int)(result - result % 20);
    }
    public static int GetSellPrice(int attack, int accuracy) => GetBuyPrice(attack, accuracy) / 4;
    private static int GetBuyPrice(this WeaponStats weaponStats) => GetBuyPrice(weaponStats.Attack, weaponStats.Accuracy);
    private static int GetSellPrice(this WeaponStats weaponStats) => GetSellPrice(weaponStats.Attack, weaponStats.Accuracy);
}
