using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Weapons;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Hooks;


internal unsafe class WeaponNameHook
{

    public WeaponNameHook(IUObjects uObjects, IWFUnreal unreal, WeaponRegistry registry)
    {
        uObjects.FindObject("DatItemWeaponNameDataAsset", obj => 
        { 
            var nameTable = (UItemNameListTable*)obj.Self;
            var nameEntries = nameTable->Data;
            var nameCount = nameTable->Data.arr_num;
            for (int i = 0; i < nameCount; i++)
            {
                var weapon = registry.Weapons.FirstOrDefault(x => x.WeaponItemId == i);

                if (weapon?.Name != null)
                {
                    nameTable->Data.allocator_instance[i] = unreal.FString(weapon.Name);
                    Log.Debug($"Set name for Weapon Item ID: {weapon.WeaponItemId} || Name: {weapon.Name}");
                }
                continue;
            }
        });
/*        uObjects.FindObject("DatItemWeaponNameDataAsset", obj =>
        {
            var nameTable = (UItemNameListTable*)obj.Self;

            
        });*/
    }
}
