using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework.Hooks.Weapons.Models;

internal class DefaultWeapon : Weapon
{
    public DefaultWeapon(ShellType shellType)
    {
        if(!shellType.TryGetCharacterFromShell(out var character) || !character.HasValue)
            return;
        Character = character.Value;
        Config.Shell = shellType;
        ModelId = shellType.ModelId();
    }
}
