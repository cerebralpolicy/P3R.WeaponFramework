using P3R.WeaponFramework.Hooks.Weapons.Models;
using P3R.WeaponFramework.Types.Collections;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Hooks.Weapons
{

    internal unsafe class WeaponTableService
    {
        private readonly IUnreal unreal;
        private readonly WeaponRegistry weapons;
        private DefaultWeapons defaultWeapons;
        private DataTable<FAppCharWeaponTableRow>? table;

        private List<int> RegisteredModelIDs = [];

        public WeaponTableService(IDataTables dt, IUnreal unreal, WeaponRegistry weapons)
        {
            this.unreal = unreal;
            this.weapons = weapons;
            this.defaultWeapons = new();
            dt.FindDataTable<FAppCharWeaponTableRow>("DT_Weapon", table =>
            {
                this.defaultWeapons = new();
                this.table = table;
                this.UpdateWeaponTable();
            });
        }

        private void UpdateWeaponTable()
        {
            if (this.table == null)
                return;
            foreach (var weapon in weapons.Weapons.Weapons)
            {
                if (!RegisteredModelIDs.Contains(weapon.ModelId) && !weapon.IsModded)
                    this.UpdateWeapon(weapon);
            }
        }

        public void SetWeaponData(int modelId, Weapon weapon)
        {
            var key = weapon.ToMeshId();
            var shellTarget = weapon.ShellTarget;
            var armatures = weapon.ShellTarget.AsShell().Armatures;
            weapon.TryGetPaths(out var paths);
            if (paths == null) return;
            if (armatures.Equivalent(paths, out var count))
            {
                for (var i = 0; i < count; i++)
                {
                    var path = paths[i];
                    if (path == null) continue;
                    var armature = armatures[i].EnumValue;
                    var armatureRow = this.GetArmatureRow(armature);
                    if (armatureRow == null) continue;
                    if (armatureRow->Data.TryGet(key, out var weaponMeshData))
                    {
                        SetArmatureAsset(weaponMeshData, shellTarget, i, path);
                    }
                }
            }
        }

        private void UpdateWeapon(Weapon weapon)
        {
            var key = weapon.ToMeshId();
            var shellTarget = weapon.ShellTarget;
            var armatures = weapon.ShellTarget.AsShell().Armatures;
            weapon.TryGetPaths(out var paths);
            if (paths == null) return;
            if (armatures.Equivalent(paths, out var count))
            {
                for (var i = 0; i < count; i++)
                {
                    var path = paths[i];
                    if (path == null) continue;
                    var armature = armatures[i].EnumValue;
                    var armatureRow = this.GetArmatureRow(armature);
                    if (armatureRow == null) continue;
                    if (armatureRow->Data.TryGet(key, out var weaponMeshData))
                    {
                        SetArmatureAsset(weaponMeshData, shellTarget, i, path);
                    }
                }
            }
        }
        private unsafe void SetArmatureAsset(FAppCharWeaponMeshData* weaponData, ShellType shell, int pathIndex, string? newAssetFile)
        {
            var assetFile = newAssetFile ?? this.GetDefaultAsset(shell, pathIndex);
            if(assetFile == null)
            {
                return;
            }
            var assetPath = AssetUtils.GetUnrealAssetPath(assetFile);
            var assetFName = assetFile != "None" ? *this.unreal.FName(assetPath) : *this.unreal.FName("None");

            weaponData->Mesh.baseObj.baseObj.ObjectId.AssetPathName = assetFName;
            weaponData->Mesh.baseObj.baseObj.WeakPtr = new();
        }

        private string? GetDefaultAsset(ShellType shell, int index)
        {
            if (!this.defaultWeapons[shell].TryGetPaths(out var paths))
                return null;
            return paths[index];
        }
        public FAppCharWeaponTableRow* GetArmatureRow(EArmature armature) => table!.Rows.First(x => x.Name == armature.ToString()).Self;
    }
}
