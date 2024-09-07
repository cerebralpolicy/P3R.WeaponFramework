    using static P3R.WeaponFramework.Utils;
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
        private readonly Core core;
        public WeaponArsenal(GameWeapons weapons, Core core)
        {
            this.weapons = weapons;
            this.core = core;
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
            core.AddNewWeapon(weapon);       
            core.Utils.Log($"Weapon created: {weapon.Character} || Weapon ID: {weapon.WeaponItemId}\nFolder: {weaponDir}");
            return weapon;
        }

        private void ApplyWeaponConfig(Weapon weapon, WeaponConfig config)
        {
            StaticUtils.IfNotNull(config.Name, str => weapon.Config.Name = str);
            StaticUtils.IfNotNull(config.Base.MeshPath, str => weapon.Config.Base.MeshPath = str);
            StaticUtils.IfNotNull(config.Mesh.MeshPath, str => weapon.Config.Mesh.MeshPath = str);
            StaticUtils.IfNotNull(config.Stats, stats => weapon.Config.Stats = stats);
        }

        private Weapon? CreateOrFindWeapon(Character character, string name)
        {
            var existingWeapon = weapons.Values.FirstOrDefault(x => x.Name == name && x.Character == character);
            if (existingWeapon != null) 
            { 
                return existingWeapon;
            }
            var newWeapon = weapons.Values.FirstOrDefault(x => x.IsVanilla == false && x.WeaponItemId > 999); // FAILSAFE
            if (newWeapon != null)
            {
                newWeapon.Name = name;
                newWeapon.Character = character;
                newWeapon.IsEnabled = true;
            }
            else
            {
                core.Utils.Log("No available weapon slot.", LogLevel.Warning);
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
                core.Utils.Log("No available weapon slot.", LogLevel.Warning);
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
