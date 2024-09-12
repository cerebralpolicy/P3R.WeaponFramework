using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Hooks;

public record ShellWeapon(Character Character, EWeaponModelSet WeaponModelSet)
{
    public string MeshPath { get; } = AssetUtils.GetAssetFile(Character, WeaponModelSet, WeaponAssetType.Weapon_Mesh)!;
}