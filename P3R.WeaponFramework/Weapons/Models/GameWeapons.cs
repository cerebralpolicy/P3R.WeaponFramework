using P3R.WeaponFramework.Types;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace P3R.WeaponFramework.Weapons.Models;

internal class GameWeapons: IReadOnlyList<Weapon>
{
    public const int BASE_MOD_WEAP_ID = 1000;
    private const int NUM_MOD_WEAPS = 100;
    private readonly List<Weapon> weapons = [];
    
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
            weapons.AddRange(charWeapons.Value);
        }
        // Enable all existing weapons.
        foreach (var weapon in weapons)
        {
            weapon.IsEnabled = true;
        }
        for (int i = 0; i < NUM_MOD_WEAPS; i++)
        {
            var weaponId = BASE_MOD_WEAP_ID + i;
            var weapon = new Weapon(weaponId);
            weapons.Add(weapon);
        }
    }

    public Weapon this[int index] => weapons[index];

    public int Count => weapons.Count;

    public IEnumerator<Weapon> GetEnumerator() => weapons.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => weapons.GetEnumerator();
}
