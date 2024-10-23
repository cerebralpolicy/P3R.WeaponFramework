using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Hooks;


public unsafe class WeaponNameHook
{
    public WeaponNameHook(IUObjects uObjects, IUnreal unreal, WeaponRegistry registry, WeaponOverridesRegistry overrides)
    {
        uObjects.FindObject("DatItemWeaponNameDataAsset", obj =>
        {

            var nameTable = (UItemNameListTable*)obj.Self;

            var nameCount = nameTable->Data.Num;
            for (int i = 0; i < registry.Weapons.Count; i++)
            {
                var weapon = registry.GetActiveWeapons().FirstOrDefault(x => x.WeaponItemId == i);
                if (weapon?.Name != null && weapon != null)
                {

                    Log.Verbose($"Expected name: {weapon.Name}");
                    var newName = weapon.Name;
                    if (newName == "Unused")
                    {
                        newName = $"{newName} [{i:X3}]";
                    }
                    if (overrides.TryGetWeaponOverrideFrom(weapon.Character, i, out var newWeapon))
                    {
                        newName = newWeapon.Name ?? newName;
                    }
                    nameTable->Data.AllocatorInstance[i] = unreal.FString(newName);
                    Log.Debug($"Set name for Weapon Item ID: {weapon.WeaponItemId} || Name: {newName}");
                }
                continue;
            }
        });
    }
}
