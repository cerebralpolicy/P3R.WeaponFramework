using P3R.WeaponFramework.Hooks;
using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Weapons
{
    internal class WeaponRegistry
    {
        private readonly WeaponArsenal arsenal;
        public WeaponRegistry()
        {
            Weapons = new();
            arsenal = new(this.Weapons);
        }
        public GameWeapons Weapons { get; }

        public Weapon[] GetActiveWeapons() =>
            this.Weapons.Where(IsActiveWeapon).ToArray();

        public bool TryGetWeapon(Character character, int weaponId, [NotNullWhen(true)] out Weapon? weapon)
        {
            weapon = Weapons.FirstOrDefault(x => IsRequestedWeapon(x, character, weaponId));
            return weapon != null;
        }

        public bool TryGetWeaponByItemId(int itemId, [NotNullWhen(true)] out Weapon? weapon)
        {
            var weaponItemId = Weapon.GetWeaponItemId(itemId);
            weapon = Weapons.FirstOrDefault(x => x.WeaponItemId == itemId && IsActiveWeapon(x));
            return weapon != null;
        }
        
        public void RegisterMod(string modId, string modDir)
        {
            var mod = new WeaponMod(modId, modDir);
            if (!Directory.Exists(mod.WeaponsDir))
            {
                return;
            }
            foreach (var character in Character.List)
            {
                var characterDir = Path.Join(mod.WeaponsDir, character.Name);
                if (!Directory.Exists(characterDir))
                {
                    continue;
                }

                //Build weapons from folders.
                foreach(var weaponDir in Directory.EnumerateDirectories(characterDir))
                {
                    try
                    {
                        arsenal.Create(mod, weaponDir, character);
                        if (character == Character.Aigis)
                        {
                            arsenal.Create(mod, weaponDir, Character.Aigis12);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"{ex.Message} - Failed to create weapon from folder.\nFolder: {weaponDir}");
                    }
                }
            }
        }

        private static bool IsRequestedWeapon(Weapon weapon, ECharacter character, int weaponId)
            => weapon.Character == character && weapon.WeaponId == weaponId && IsActiveWeapon(weapon);

        private static bool IsActiveWeapon(Weapon weapon) 
            => weapon.IsEnabled
            && weapon.Character != ECharacter.NONE;
       
    }
}
