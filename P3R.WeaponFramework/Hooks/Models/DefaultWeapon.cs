using P3R.WeaponFramework.Interfaces.Definitions;
using P3R.WeaponFramework.Weapons;
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
        Base.MeshPath1 = AssetUtils.GetAssetFile(character, EWeaponModelSet.Base, WeaponAssetType.Base_Mesh);
        Base.AnimPath = AssetUtils.GetAssetFile(character, EWeaponModelSet.Base, WeaponAssetType.Base_Anim);
        Mesh.MeshPath1 = AssetUtils.GetAssetFile(character, EWeaponModelSet.Base, WeaponAssetType.Weapon_Mesh);
    }
}
