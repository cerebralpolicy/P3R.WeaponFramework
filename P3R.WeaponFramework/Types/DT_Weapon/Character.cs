namespace P3R.WeaponFramework.Weapons;

internal static partial class WeaponExtensions
{
    public static WeaponType GetWeaponType(this Character character)
    {
        if (character == Character.FEMC)
            return WeaponType.Sword;
        else
        {
            var characterName = Enum.GetName(typeof(Character), character);
            var valid = Enum.TryParse(characterName, out WeaponType weaponType);
            if (valid)
                return weaponType;
            else
                return WeaponType.UNUSED;
        }
    }
}