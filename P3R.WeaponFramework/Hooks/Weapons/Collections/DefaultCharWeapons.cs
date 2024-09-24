using P3R.WeaponFramework.Weapons.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Hooks.Weapons.Collections
{
    internal class DefaultCharWeapons : IReadOnlyDictionary<Character, Dictionary<ShellType, Weapon>>
    {
        private readonly Dictionary<Character, Dictionary<ShellType, Weapon>> charWeapons = [];

        public DefaultCharWeapons(DefaultWeapons defaultWeapons)
        {
            foreach (var character in Characters.WFArmed)
            {
                var weaps = defaultWeapons.characterWeapons(character);
                charWeapons.Add(character, weaps);
            }
        }

        public Dictionary<ShellType, Weapon> this[Character key] => charWeapons[key];

        public IEnumerable<Character> Keys => charWeapons.Keys;

        public IEnumerable<Dictionary<ShellType, Weapon>> Values => charWeapons.Values;

        public int Count => charWeapons.Count;

        public bool ContainsKey(Character key) => charWeapons.ContainsKey(key);

        public IEnumerator<KeyValuePair<Character, Dictionary<ShellType, Weapon>>> GetEnumerator() => charWeapons.GetEnumerator();

        public bool TryGetValue(Character key, [MaybeNullWhen(false)] out Dictionary<ShellType, Weapon> value) => charWeapons.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
