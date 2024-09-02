using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Hooks.Models;

internal class DefaultWeapon : WeaponConfig
{
    public DefaultWeapon(Character character)
    {
        Base.MeshPath = AssetUtils.GetAssetFile(character, WeaponModelSet.Base, WeaponAssetType.Base_Mesh);
        Base.AnimPath = AssetUtils.GetAssetFile(character, WeaponModelSet.Base, WeaponAssetType.Base_Anim);
        Mesh.MeshPath = AssetUtils.GetAssetFile(character, WeaponModelSet.Base, WeaponAssetType.Weapon_Mesh);
    }
}
