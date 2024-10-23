using P3R.WeaponFramework.Weapons.Models;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace P3R.WeaponFramework.Hooks.Weapons.Models;

internal class DefaultWeapons : IReadOnlyDictionary<ShellType, Weapon>
{
    private readonly Dictionary<ShellType, Weapon> weapons = [];

    public DefaultWeapons()
    {
        foreach (var chara in Characters.Lookup.Where(x => x.IsArmed))
        {
            foreach (var shell in chara.ShellTypes)
            {
                weapons.Add(shell, new DefaultWeapon(shell));
            }
        }
    }

    public Weapon this[ShellType key] => ((IReadOnlyDictionary<ShellType, Weapon>)weapons)[key];

    public IEnumerable<ShellType> Keys => ((IReadOnlyDictionary<ShellType, Weapon>)weapons).Keys;

    public IEnumerable<Weapon> Values => ((IReadOnlyDictionary<ShellType, Weapon>)weapons).Values;

    public int Count => ((IReadOnlyCollection<KeyValuePair<ShellType, Weapon>>)weapons).Count;

    public bool ContainsKey(ShellType key)
    {
        return ((IReadOnlyDictionary<ShellType, Weapon>)weapons).ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<ShellType, Weapon>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<ShellType, Weapon>>)weapons).GetEnumerator();
    }

    public bool TryGetValue(ShellType key, [MaybeNullWhen(false)] out Weapon value)
    {
        return ((IReadOnlyDictionary<ShellType, Weapon>)weapons).TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)weapons).GetEnumerator();
    }
}