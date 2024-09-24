using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Interfaces.Definitions;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Hooks.Weapons.Models;

internal class DefaultWeapon : Weapon
{
    public DefaultWeapon(ShellType shellType)
    {
        Character = shellType.CharacterFromShell();
        Config.Shell = shellType;
        //Config.Base.MeshPath = GetAssetFile(shellType.CharacterFromShell(), EWeaponModelSet.Base, WeaponAssetType.Base_Mesh);
        var shellPaths = shellType.GetShellPaths();
        Config.Model.MeshPath1 = shellPaths[0];
        if (shellType.RequiredMeshes == 2)
            Config.Model.MeshPath2 = shellPaths[1];
        ModelId = shellType.ShellModelID;
    }
}
