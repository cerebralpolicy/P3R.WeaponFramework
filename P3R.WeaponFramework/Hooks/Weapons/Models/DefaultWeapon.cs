using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework.Hooks.Weapons.Models;

internal class DefaultWeapon : Weapon
{
    public DefaultWeapon(ShellTypeWrapper shellType)
    {
        if(!shellType.TryGetCharacterFromShell(out var character))
            return;
        Character = character;
        Config.Shell = shellType;
        //Config.Base.MeshPath = GetAssetFile(shellType.CharacterFromShell(), WeaponModelSet.Base, WeaponAssetType.Base_Mesh);
        var shellPaths = shellType.GetShellPaths();
        Config.Model.MeshPath1 = shellPaths[0];
        if (shellType.GetRequiredMeshes() == 2)
            Config.Model.MeshPath2 = shellPaths[1];
        ModelId = shellType.ShellTableBaseModelId;
    }
}
