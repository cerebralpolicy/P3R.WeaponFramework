using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework.Hooks.Weapons.Models;

internal class DefaultWeapon : Weapon
{
    public DefaultWeapon(ShellType shellType)
    {
        var shell = shellType.AsShell();
        if(!shellType.TryGetCharacterFromShell(out var character) || !character.HasValue || shell is null)
            return;
        Character = character.Value;
        Config.Shell = shellType;
        ModelId = shellType.ModelId();

        var paths = shell.BasePaths;

        Config.Model.MeshPath1 = paths[0];
        if (shell.Meshes > 1) 
            Config.Model.MeshPath2 = paths[1];
    }
}
