using P3R.WeaponFramework.Weapons;

namespace P3R.WeaponFramework.Hooks;

internal unsafe class WeaponNameHook
{
    public WeaponNameHook(Core core, WeaponRegistry registry)
    {
        core.FindObjectAsync("DatItemWeapoNAmeDataAsset", obj => 
        { 
            var nameTable = (UItemNameListTable*)obj.ToPointer();
            for (int i = 0; i < nameTable->Data.arr_num; i++)
            {
                var weapon = registry.Weapons.Values.FirstOrDefault(x => x.WeaponId == i);

                

                if (weapon?.Config.Name != null)
                {
                    nameTable->Data.allocator_instance[i] = core.MakeFString(weapon.Name);
                    core.Utils.Log($"Set name for Weapon Item ID: {weapon.WeaponItemId} || Name: {weapon.Name}", LogLevel.Debug);
                }
                else if (weapon?.Name != null)
                {
                    nameTable->Data.allocator_instance[i] = core.MakeFString(weapon.Name);
                    core.Utils.Log($"Set name for Weapon Item ID: {weapon.WeaponItemId} || Name: {weapon.Name}", LogLevel.Debug);
                }
            }
        });
/*        uObjects.FindObject("DatItemWeaponNameDataAsset", obj =>
        {
            var nameTable = (UItemNameListTable*)obj.Self;

            
        });*/
    }
}
