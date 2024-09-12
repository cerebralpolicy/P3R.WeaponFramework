using P3R.WeaponFramework.Types;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace P3R.WeaponFramework.Weapons.Models;

internal class GameWeapons: IReadOnlyDictionary<int, Weapon>
{
    public const int BASE_MOD_WEAP_ID = 1000;
    private const int NUM_MOD_WEAPS = 10;
    private readonly WeaponIdTable<Weapon> weapons = [];
//    private readonly Dictionary<int, Weapon> weapons = [];
    private readonly CategorizedWeaponTable<Weapon> categorizedWeapons = [];
//    private readonly Dictionary<Character, Dictionary<int,Weapon>> categorizedWeapons = [];

    public GameWeapons()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "P3R.WeaponFramework.Resources.Weapons.json";
        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        // Import vanilla weapons
        var gameWeapons = JsonSerializer.Deserialize<Dictionary<Character,Weapon[]>>(json)!;
        foreach (var charWeapons in gameWeapons)
        {
            var charDict = new WeaponIdTable<Weapon>();
            foreach (var weapon in charWeapons.Value)
            {
                //Log.Debug($"Weapon: {weapon.Name}");
                var id = weapon.WeaponId;
                weapons.Add(id, weapon);
                charDict.Add(id, weapon);
            }
            categorizedWeapons.Add(charWeapons.Key, charDict);            
        }
        // Enable all existing weapons.
        foreach (var weapon in weapons.Values)
        {
            weapon.IsEnabled = true;
        }

        for (int i = 0; i < NUM_MOD_WEAPS; i++)
        {
            var weaponItemId = BASE_MOD_WEAP_ID + i;
            Log.Debug($"New slot {weaponItemId}");
            var weapon = new Weapon(weaponItemId) 
            { 
                IsVanilla = false,
                WeaponId = weaponItemId,
                ModelId = weaponItemId,
            };
            if (weapon is null)
                break;
            weapons.Add(weaponItemId, weapon);
            continue;
        }
    }
    public Weapon this[int key] => throw new NotImplementedException();

    public IEnumerable<int> Keys => weapons.Keys;

    public IEnumerable<Weapon> Values => weapons.Values;

    public int Count => weapons.Count;

    public bool ContainsKey(int key) => weapons.ContainsKey(key);

    public bool TryGetValue(int key, [MaybeNullWhen(false)] out Weapon value)
    {
        if (ContainsKey(key))
        {
            value = weapons[key];
            return true;
        }
        else
        value = null;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => weapons.GetEnumerator();

    IEnumerator<KeyValuePair<int, Weapon>> IEnumerable<KeyValuePair<int, Weapon>>.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
