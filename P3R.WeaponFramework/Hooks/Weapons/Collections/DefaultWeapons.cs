using P3R.WeaponFramework.Hooks.Weapons.Models;
using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Hooks.Weapons.Collections;

internal class DefaultWeapons : IReadOnlyDictionary<ShellType, Weapon>
{
    private readonly Dictionary<ShellType, Weapon> weapons = [];

    public DefaultWeapons()
    {
        foreach (var character in Characters.WFArmed)
        {
            foreach (var shell in character.Shells)
            {
                weapons.Add(shell, new DefaultWeapon(shell));
            }
        }
    }


    public Dictionary<ShellType, Weapon> characterWeapons(Character character) => weapons.Where(x => x.Value.Character == character).ToDictionary();

    public Weapon this[ShellType key] => weapons[key];

    public IEnumerable<ShellType> Keys => weapons.Keys;

    public IEnumerable<Weapon> Values => weapons.Values;

    public int Count => weapons.Count;

    public bool ContainsKey(ShellType key) => weapons.ContainsKey(key);

    public IEnumerator<KeyValuePair<ShellType, Weapon>> GetEnumerator() => weapons.GetEnumerator();

    public bool TryGetValue(ShellType key, [MaybeNullWhen(false)] out Weapon value) => weapons.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => weapons.GetEnumerator();
}
