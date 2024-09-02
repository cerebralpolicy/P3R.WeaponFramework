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
            var config = GetWeaponConfig(weaponDir);
            var weapon = this.CreateOrFindWeapon(character, config.Name ?? Path.GetFileName(weaponDir));
            if (weapon == null)
            {
                return null;
            }

            weapon.OwnerModId = mod.ModId;

            ApplyWeaponConfig(weapon, config);
            LoadWeaponFiles(mod, weapon, weaponDir);
            Log.Information($"Weapon created: {weapon.Character} || Weapon ID: {weapon.WeaponId}\nFolder: {weaponDir}");
            return weapon;
        }

        private void ApplyWeaponConfig(Weapon weapon, WeaponConfig config)
        {
            ModUtils.IfNotNull(config.Name, str => weapon.Config.Name = str);
            ModUtils.IfNotNull(config.Base.MeshPath, str => weapon.Config.Base.MeshPath = str);
            ModUtils.IfNotNull(config.Mesh.MeshPath, str => weapon.Config.Mesh.MeshPath = str);
            ModUtils.IfNotNull(config.Stats, stats => weapon.Config.Stats = stats);
        }

        private Weapon? CreateOrFindWeapon(Character character, string name)
        {
            var existingWeapon = weapons.Values.FirstOrDefault(x => x.Name == name && x.Character == character);
            if (existingWeapon != null) 
            { 
                return existingWeapon;
            }
            var newWeapon = weapons.Values.FirstOrDefault(x => x.IsVanilla == false && x.WeaponItemId > 999);
            if (newWeapon != null)
            {
                newWeapon.Name = name;
                newWeapon.Character = character;
                newWeapon.IsEnabled = true;
            }
            else
            {
                Log.Warning("No available weapon slot.");
            }
            return newWeapon;
        }

        private static WeaponConfig GetWeaponConfig(string weaponDir)
        {
            var configFile = Path.Join(weaponDir, "config.yaml");
            if (File.Exists(configFile))
            {
                return YamlSerializer.DeserializeFile<WeaponConfig>(configFile);
            }

            return new();
        }
        private void LoadWeaponFiles(WeaponMod mod, Weapon weapon, string weaponDir)
        {
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
            var weapon = weapons.Values.FirstOrDefault(x => x.Character == Character.NONE);
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
