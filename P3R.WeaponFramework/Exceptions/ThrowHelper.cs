using P3R.WeaponFramework.Weapons.Models;
using Reloaded.Memory.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Exceptions;

public static class ThrowHelper
{
    public static string NoMatchingPredicateMessage(string weaponList, Func<Weapon?,bool> predicate) => $"No {weaponList.ToLowerInvariantFast()} weapon in satisifies the condition {predicate}.";
    public static string NoMatchingPredicateMessage(string weaponList, Predicate<Weapon> predicate) => $"No {weaponList.ToLowerInvariantFast()} weapon in satisifies the condition {predicate}.";
    public static void ThrowNoMatchingPredicate(string weaponList, Predicate<Weapon> predicate)
        => throw new NoMatchingWeaponException(NoMatchingPredicateMessage(weaponList, predicate));
    public static void ThrowNoMatchingWeapon(Weapon weapon, string weaponList)
    {
        var weapName = weapon.Name;
        var message = $"{weapName} does not exist in {weaponList}.";
        throw new NoMatchingWeaponException(message);
    }
    public static string NoEmptySlotMessage(string weaponList) => $"{weaponList} does not have any assignable slots.";
    public static void ThrowNoEmptySlot(string weaponList)
        => throw new NoEmptySlotException($"{weaponList} does not have any assignable slots.");
    public static void ThrowEmptyWeaponList(string weaponList)
        => throw new EmptyWeaponListException($"No weapons can be found in {weaponList}");
}
