using P3R.WeaponFramework.Core;
using P3R.WeaponFramework.Weapons.Models;
using Reloaded.Hooks.Definitions.Structs;
using System.Diagnostics.CodeAnalysis;

namespace P3R.WeaponFramework.Weapons
{
    public class WeaponRegistry
    {
        private readonly WeaponArsenal arsenal;
        public WeaponRegistry(GameWeapons weapons)
        {
            Weapons = weapons;
            arsenal = new(weapons);
        }
        public WeaponRegistry(EpisodeHook episodeHook)
        {
            Weapons = new(episodeHook);
            arsenal = new(this.Weapons);
        }
        public GameWeapons Weapons { get; }

        public Weapon[] GetActiveWeapons() =>
            this.Weapons.Where(IsActiveWeapon).ToArray();

        public bool TryGetWeapon(ECharacter character, int weaponId, [NotNullWhen(true)] out Weapon? weapon)
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
            var validChars = Characters.Armed;
            Log.Debug($"{validChars.Count}");
            foreach (var character in Characters.Lookup.Armed)
            {
                var characterDir = Path.Join(mod.WeaponsDir, character.ToString());
                if (!Directory.Exists(characterDir))
                {
                    continue;
                }

                //Build weapons from folders.
                foreach (var weaponDir in Directory.EnumerateDirectories(characterDir))
                {
                    try
                    {
                        arsenal.Create(mod, weaponDir, character);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"{ex.Message}\n{ex.Source}\n Failed to create weapon from folder.\nFolder: {weaponDir}");
                    }
                }
            }
            if (!Directory.Exists(mod.OverrideDir))
            {
                return;
            }
            foreach (var proxyFile in Directory.EnumerateFiles(mod.OverrideDir))
            {
                try
                {
                    
                }
                catch (Exception ex)
                {
                    Log.Error($"{ex.Message}\n{ex.Source}\n Failed to create weapon from folder.\nFolder: {proxyFile}");

                }
            }
        }

        private static bool IsRequestedWeapon(Weapon weapon, ECharacter character, int weaponId)
            => weapon.Character == character && weapon.WeaponId == weaponId && IsActiveWeapon(weapon);

        private static bool IsActiveWeapon(Weapon weapon)
            => weapon.IsEnabled;

    }
}
