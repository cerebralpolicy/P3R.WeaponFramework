using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Weapons.Values.Where(IsActiveWeapon).ToArray();

        public bool TryGetWeaponById(int id, [NotNullWhen(true)] out Weapon? weapon)
        {
            Weapons.TryGetValue(id, out weapon);
            if (weapon != null && !weapon.IsEnabled)
                weapon = null;
            return weapon != null;
        }

        public bool TryGetWeaponByItemId(int itemId, [NotNullWhen(true)] out Weapon? weapon)
        {
            var weaponItemId = Weapon.GetWeaponItemId(itemId);
            weapon = Weapons.Values.FirstOrDefault(x => x.WeaponItemId == itemId && IsActiveWeapon(x));
            return weapon != null;
        }

        public void RegisterMod(string modId, string modDir)
        {
            var mod = new WeaponMod(modId, modDir);
            if (!Directory.Exists(mod.WeaponsDir))
            {
                return;
            }
            foreach (var character in Enum.GetValues<Character>())
            {
                var characterDir = Path.Join(mod.WeaponsDir, character.ToString());
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
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Failed to create weapon from folder.\nFolder: {weaponDir}");
                    }
                }
            }
        }

        private static bool IsActiveWeapon(Weapon weapon) 
            => weapon.IsEnabled
            && weapon.Character != Character.NONE;
    }
}
