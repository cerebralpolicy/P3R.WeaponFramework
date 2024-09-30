using P3R.WeaponFramework.Core;
using P3R.WeaponFramework.Hooks;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

namespace P3R.WeaponFramework.Weapons.Models;
 
internal unsafe class GameWeapons : EpisodeHookBase, IReadOnlyCollection<Weapon>
{
    
    public GameWeapons() : base()
    {

    }
    public int Count => Weapons.Count;

    public bool TryGetWeaponByItemId(int itemId, [NotNullWhen(true)] out Weapon? weapon)
    {
        weapon = Weapons.FirstOrDefault(x => x.WeaponItemId == itemId);
        return weapon != null;
    }

    public IEnumerator<Weapon> GetEnumerator() => Weapons.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Weapons.GetEnumerator();
}
