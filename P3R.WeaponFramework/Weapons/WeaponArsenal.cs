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
            return weapon;
        }
        private void ApplyWeaponConfig(Weapon weapon, WeaponConfig config)
        {
            ModUtils.IfNotNull(config.Name, str => weapon.Name = str);
            ModUtils.IfNotNull(config.Model.MeshPath1, str => weapon.Config.Model!.MeshPath1 = str);
            ModUtils.IfNotNull(config.Model.MeshPath2, str => weapon.Config.Model!.MeshPath2 = str);
            ModUtils.IfNotNull(config.Stats, stats =>
            {
                weapon.Config.Stats = stats;
                weapon.VerifyPrices();
            });
            ModUtils.IfNotNull(config.Shell, shell =>
            {
                weapon.Config.Shell = shell;
                var shellType = ShellType.FromValue(shell);
                weapon.ShellTarget = shellType;
                weapon.ModelId = shellType.ShellModelID;        
            });
        }

        private void LoadWeaponFiles(WeaponMod mod, Weapon weapon, string weaponDir)
        {
            
            // BASEMESH WILL ALWAYS BE A REF
//            SetWeaponFile(mod, Path.Join(weaponDir, "base-mesh.uasset"), path => weapon.Config.Base.MeshPath = path);
//            SetWeaponFile(mod, Path.Join(weaponDir, "base-anim.uasset"), path => weapon.Config.Base.MeshPath = path);

            SetWeaponFile(mod, Path.Join(weaponDir, "weapon-mesh2.uasset"), path => weapon.Config.Model!.MeshPath2 = path);
            SetWeaponFile(mod, Path.Join(weaponDir, "weapon-mesh.uasset"), path => weapon.Config.Model!.MeshPath1 = path);
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
        public Weapon? CreateOrFindWeapon(string ownerId, Character character, string name)
        {
            var existingWeapon = weapons.FirstOrDefault(x => x.Character == character && x.Name == name);
            if (existingWeapon != null)
            {
                return existingWeapon;
            }

            var newWeapon = weapons.FirstOrDefault(x => x.Character == Character.NONE && x.WeaponItemId > DataUtils.MAX_VANILLA_ID);
            if (newWeapon != null)
            {
                EpisodeFlag flag = EpisodeFlag.Both;
                if (Characters.AstreaOnly.Contains(character))
                {
                    flag = EpisodeFlag.Astrea;
                }
                else if (Characters.VanillaOnly.Contains(character))
                {
                    flag = EpisodeFlag.Vanilla;
                }
                newWeapon.Name = name;
                newWeapon.Character = character;
                newWeapon.IsVanilla = false;
                newWeapon.IsEnabled = true;
                newWeapon.OwnerModId = ownerId;
                newWeapon.EpisodeFlag = flag;
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