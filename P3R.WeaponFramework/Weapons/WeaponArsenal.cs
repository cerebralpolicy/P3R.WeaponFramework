using static P3R.WeaponFramework.Interfaces.PriceUtils;
using P3R.WeaponFramework.Utils;
using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.ObjectsEmitter.Interfaces;

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
            var weapon = CreateOrFindWeapon(mod.ModId, character, config.Name ?? Path.GetFileName(weaponDir));
            if (weapon == null)
            {
                return null;
            }

            ApplyWeaponConfig(weapon, config);
            LoadWeaponFiles(mod, weapon, weaponDir);
            Log.Information($"Weapon created: {weapon.Character} || Weapon ID: {weapon.WeaponId}\nFolder: {weaponDir}");
            return weapon;
        }
        private void ApplyWeaponConfig(Weapon weapon, WeaponConfig config)
        {
            ModUtils.IfNotNull(config.Name, str => weapon.Name = str);
            ModUtils.IfNotNull(config.Base.MeshPath1, str => weapon.Config.Base!.MeshPath1 = str);
            ModUtils.IfNotNull(config.Mesh.MeshPath1, str => weapon.Config.Mesh!.MeshPath1 = str);
            ModUtils.IfNotNull(config.Mesh.MeshPath2, str => weapon.Config.Mesh!.MeshPath2 = str);
            ModUtils.IfNotNull(config.Stats, stats =>
            {
                weapon.Config.Stats = stats;
                weapon.VerifyPrices();
            });
        }

        private void LoadWeaponFiles(WeaponMod mod, Weapon weapon, string weaponDir)
        {
            
            // BASEMESH WILL ALWAYS BE A REF
//            SetWeaponFile(mod, Path.Join(weaponDir, "base-mesh.uasset"), path => weapon.Config.Base.MeshPath = path);
//            SetWeaponFile(mod, Path.Join(weaponDir, "base-anim.uasset"), path => weapon.Config.Base.MeshPath = path);

            SetWeaponFile(mod, Path.Join(weaponDir, "weapon-mesh2.uasset"), path => weapon.Config.Mesh!.MeshPath2 = path);
            SetWeaponFile(mod, Path.Join(weaponDir, "weapon-mesh.uasset"), path =>
            {
                weapon.Config.Mesh!.MeshPath1 = path;
                weapon.AddToDT();
            });
            SetWeaponFile(mod, Path.Join(weaponDir, "description.msg"), path => weapon.Description = File.ReadAllText(path), SetType.Full);
        }

        private unsafe void AddWeaponToBP(Weapon weapon)
        {
            if (weapon.Config.Base == null || weapon.Config.Mesh == null)
                return;
            var basePath = weapon.Config.Base.MeshPath1;
            var baseId = Path.GetFileNameWithoutExtension(basePath)?.Substring(3);
            
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
        public Weapon? CreateOrFindWeapon(string ownerId, Character character, string name)
        {
            var existingWeapon = weapons.Values.FirstOrDefault(x => x.Character == character && x.Name == name);
            if (existingWeapon != null)
            {
                return existingWeapon;
            }

            var newWeapon = weapons.Values.FirstOrDefault(x => x.Character == Character.NONE && x.WeaponItemId > DataUtils.MAX_VANILLA_ID);
            if (newWeapon != null)
            {
                newWeapon.Name = name;
                newWeapon.Character = character;
                newWeapon.IsVanilla = false;
                newWeapon.IsEnabled = true;
                newWeapon.OwnerModId = ownerId;
            }
            else
            {
                Log.Warning("No new weapon slots available.");
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
        private enum SetType
        {
            Relative,
            Full,
        }
    }
}