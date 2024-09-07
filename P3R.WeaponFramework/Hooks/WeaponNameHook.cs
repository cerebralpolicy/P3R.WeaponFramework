using P3R.WeaponFramework.Weapons;

namespace P3R.WeaponFramework.Hooks;

internal unsafe class WeaponNameHook
{
    public WeaponNameHook(Core core, WeaponRegistry registry)
    {
        core.FindObjectAsync("DatItemWeaponNameDataAsset", obj => 
        { 
            var nameTable = (UItemNameListTable*)obj.ToPointer();
            var nameEntries = nameTable->Data;
            var nameCount = nameTable->Data.arr_num;
            for (int i = 0; i < nameCount; i++)
            {
                var weapon = registry.Weapons.Values.FirstOrDefault(x => x.WeaponId == i);

                if (weapon?.Config.Name != null)
                {
                    nameTable->Data.allocator_instance[i] = weapon.Name.MakeFString();
                    core.Utils.Log($"Set name for Weapon Item ID: {weapon.WeaponItemId} || Name: {weapon.Name}", LogLevel.Debug);
                }
                else if (weapon?.Name != null)
                {
                    nameTable->Data.allocator_instance[i] = weapon.Name.MakeFString();
                    core.Utils.Log($"Set name for Weapon Item ID: {weapon.WeaponItemId} || Name: {weapon.Name}", LogLevel.Debug);
                }
                continue;
            }
            const string BLANK = "Weapon Framework Slot";
            for (int i = 513; i < NEWITEMSTART + 100; i++)
            {
                core.MemoryMethods.TArray_Insert(&nameEntries, BLANK.MakeFString(), i);
                continue;
            }
        });
/*        uObjects.FindObject("DatItemWeaponNameDataAsset", obj =>
        {
            var nameTable = (UItemNameListTable*)obj.Self;

            
        });*/
    }
}
