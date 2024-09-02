using P3R.WeaponFramework.Weapons;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Hooks;

internal unsafe class WeaponNameHook
{
    public WeaponNameHook(IUObjects uObjects, WeaponRegistry registry)
    {
        uObjects.FindObject("DatItemWeaponNameDataAsset", obj =>
        {
            var nameTable = (UItemNameListTable*)obj.Self;

            for (int i = 0; i < nameTable->Data.Num; i++)
            {
                var weapon = registry.Weapons.Values.FirstOrDefault(x => x.WeaponId == i);

                if (weapon?.Config.Name != null)
                {
                    nameTable->Data.AllocatorInstance[i] = new FString(weapon.Config.Name);
                    Log.Debug($"Set name for Weapon Item ID: {weapon.WeaponItemId} || Name: {weapon.Config.Name}");
                }
                else if (weapon?.Name != null)
                {
                    nameTable->Data.AllocatorInstance[i] = new FString(weapon.Name);
                    Log.Debug($"Set name for Weapon Item ID: {weapon.WeaponItemId} || Name: {weapon.Name}");
                }
            }
        });
    }
}
