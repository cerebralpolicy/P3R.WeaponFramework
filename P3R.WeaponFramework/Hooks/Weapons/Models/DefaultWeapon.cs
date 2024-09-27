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
        //Config.Base.MeshPath = GetAssetFile(shellType.CharacterFromShell(), WeaponModelSet.Base, WeaponAssetType.Base_Mesh);
        var shellPaths = shellType.AsShell().ShellPaths;
        Config.Model.MeshPath1 = shellPaths[0];
        if (shellType.AsShell().Meshes == 2)
            Config.Model.MeshPath2 = shellPaths[1];
        ModelId = shellType.AsShell().Ids.First();
    }
}
