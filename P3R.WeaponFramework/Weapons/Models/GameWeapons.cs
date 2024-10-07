using P3R.WeaponFramework.Core;
using P3R.WeaponFramework.Exceptions;
using P3R.WeaponFramework.Hooks;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

namespace P3R.WeaponFramework.Weapons.Models;
 
public unsafe class GameWeapons : IReadOnlyCollection<Weapon>
{
    public EpisodeHook EpisodeHook;
    public List<Weapon> Weapons => EpisodeHook.Weapons;
    public string EpisodeName => EpisodeHook.GetCurrentEpisode().Name;

    public GameWeapons(EpisodeHook episodeHook)
    {
        EpisodeHook = episodeHook;
    }
    public int Count => Weapons.Count;


    public bool TryGetFirstWeaponOfPredicate(Func<Weapon,bool> predicate, [NotNullWhen(true)] out Weapon? weapon)
    {
        weapon = null;
        weapon = Weapons.FirstOrDefault(predicate!,null);
        if (weapon == null)
        {
            //Log.Verbose(ThrowHelper.NoMatchingPredicateMessage(EpisodeName, predicate!));
            return false;
        }
        Log.Debug($"Weapon found at {weapon.WeaponItemId}");
        return true;
    }
    public bool TryGetFirstAssignableWeapon([NotNullWhen(true)] out Weapon? weapon)
    {
        Func<Weapon,bool> predicate = (x => x.IsModded == true);
        weapon = null;
        if (!Weapons.Any(x => x.IsModded == true))
        {
            Log.Error(new NoEmptySlotException(), ThrowHelper.NoEmptySlotMessage(EpisodeName));
            return false;
        }
        weapon = Weapons.FirstOrDefault(predicate!,null);
        if (weapon == null)
        {       
            Log.Error(new NoMatchingWeaponException(), ThrowHelper.NoMatchingPredicateMessage(EpisodeName, predicate!));
            return false;
        }
        Log.Debug($"Weapon slot found at {weapon.WeaponItemId}");
        return true;
    }
    public bool TryGetWeaponByItemId(int itemId, [NotNullWhen(true)] out Weapon? weapon)
    {
        weapon = Weapons.FirstOrDefault(x => x.WeaponItemId == itemId);
        return weapon != null;
    }

    public IEnumerator<Weapon> GetEnumerator() => Weapons.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Weapons.GetEnumerator();
}
