using P3R.WeaponFramework.Utils;
using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Weapons
{
    internal class WeaponArsenal
    {
        private readonly GameWeapons weapons;

        public WeaponArsenal(GameWeapons weapons)
        {
            this.weapons = weapons;
        }

        public Weapon? Create(WeaponMod mod, string weaponDir, Character character) 
        {
            var weapon = GetAvaliableWeapon();
            if (weapon == null)
            {
                return null;
            }

            weapon.Name = Path.GetFileName(weaponDir);
            weapon.IsEnabled = true;
            weapon.OwnerModId = mod.ModId;
            weapon.Character = character;

            ProcessWeapon(mod, weapon, weaponDir);
            Log.Information($"Weapon created: {weapon.Character} || Weapon ID: {weapon.WeaponId}\nFolder: {weaponDir}");
            return weapon;
        }


        private void ProcessWeapon(WeaponMod mod, Weapon weapon, string weaponDir)
        {
            LoadWeaponData(mod,weapon, weaponDir);
        }

        private void LoadWeaponData(WeaponMod mod, Weapon weapon, string weaponDir)
        {
            SetWeaponFile(mod, Path.Join(weaponDir, "config.yaml"), path => 
            { 
                var config = YamlSerializer.DeserializeFile<WeaponConfig>(path);
                if (config.Name != null) weapon.Config.Name = config.Name;
                if (config.Base.MeshPath != null) weapon.Config.Base.MeshPath = config.Base.MeshPath;
                if (config.Mesh.MeshPath != null) weapon.Config.Mesh.MeshPath = config.Mesh.MeshPath;
                if (config.Stats != null)
                {
                    weapon.Config.Stats = config.Stats;
                    weapon.WeaponStats = weapon.Config.Stats.Value;
                }

            }, SetType.Full);
            SetWeaponFile(mod, Path.Join(weaponDir, "base-mesh.uasset"), path => weapon.Config.Base.MeshPath = path);
            SetWeaponFile(mod, Path.Join(weaponDir, "base-anim.uasset"), path => weapon.Config.Base.MeshPath = path);

            SetWeaponFile(mod, Path.Join(weaponDir, "weapon-mesh.uasset"), path => weapon.Config.Mesh.MeshPath = path);

            SetWeaponFile(mod, Path.Join(weaponDir, "description.msg"), path => weapon.Description = File.ReadAllText(path), SetType.Full);
        }
        private static void SetWeaponFile(WeaponMod mod, string modFile, Action<string> setFile, SetType type = SetType.Relative)
        {
            if (File.Exists(modFile))
            {
                if (type == SetType.Relative)
                {
                    setFile(Path.GetRelativePath(mod.ContentDir, modFile));
                }
                else
                {
                    setFile(modFile);
                }
            }
        }
        private Weapon? GetAvaliableWeapon()
        {
            var weapon = weapons.FirstOrDefault(x => x.Character == Character.NONE);
            if (weapon == null)
            {
                Log.Warning("No available weapon slot.");
            }

            return weapon;
        }
        private enum SetType
        {
            Relative,
            Full,
        }
    }
}
