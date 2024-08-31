using P3R.WeaponFramework.Interfaces;

namespace P3R.WeaponFramework.Weapons;

internal class WeaponOverridesRegistry(WeaponRegistry weapons): IWeaponApi
{
    private readonly WeaponRegistry weapons = weapons;

    public void AddOverridesFile(string path)
    {
        throw new NotImplementedException();
    }
}
