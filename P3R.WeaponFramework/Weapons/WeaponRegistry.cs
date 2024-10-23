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

        private Dictionary<WeaponMod, int> modWeapCounter = [];
        public List<FWeaponItemList> ModifedWeapons { get; set; } = [];

        public Weapon[] GetActiveWeapons() =>
            this.Weapons.Where(IsActiveWeapon).ToArray();

        public Weapon[] GetUnusedWeapons() =>
            this.Weapons.Where(IsUnusedWeapon).ToArray();

        public bool TryGetPathsOfModelId(int modelId, [NotNullWhen(true)] out List<string>? paths)
        {
            paths = null;
            var firstWeapon = this.GetActiveWeapons().FirstOrDefault(x => x.ModelId == modelId);
            firstWeapon?.TryGetPaths(out paths);
            return paths != null;
        }

        public bool TryGetUnusedWeapon(ECharacter character, [NotNullWhen(true)] out Weapon? unusedWeapon)
        {
            unusedWeapon = GetUnusedWeapons().FirstOrDefault();
            return unusedWeapon != null;

        }

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
        public void RegisterMod(WeaponMod mod)
        {
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
                if (!modWeapCounter.ContainsKey(mod))
                {
                    modWeapCounter.Add(mod, 1);
                }
                //Build weapons from folders.
                foreach (var weaponDir in Directory.EnumerateDirectories(characterDir))
                {
                    try
                    {
                        arsenal.Create(mod, weaponDir, character, modWeapCounter[mod]);
                        modWeapCounter[mod]++;
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"{ex.Message}\n{ex.Source}\n Failed to create weapon from folder.\nFolder: {weaponDir}");
                    }
                }
            }
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
                if (!modWeapCounter.ContainsKey(mod))
                {
                    modWeapCounter.Add(mod, 1);
                }
                //Build weapons from folders.
                foreach (var weaponDir in Directory.EnumerateDirectories(characterDir))
                {
                    try
                    {
                        arsenal.Create(mod, weaponDir, character, modWeapCounter[mod]);
                        modWeapCounter[mod]++;
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"{ex.Message}\n{ex.Source}\n Failed to create weapon from folder.\nFolder: {weaponDir}");
                    }
                }
            }
        }

        private static bool IsRequestedWeapon(Weapon weapon, ECharacter character, int weaponId)
            => weapon.Character == character && weapon.WeaponId == weaponId && IsActiveWeapon(weapon);

        private static bool IsUnusedWeapon(Weapon weapon)
            => weapon.IsEnabled && (weapon.IsUnused && !weapon.IsModded) && weapon.WeaponId != 0;
        private static bool IsActiveWeapon(Weapon weapon)
            => weapon.IsEnabled;

    }
}
