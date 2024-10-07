using P3R.WeaponFramework.Weapons;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Hooks;


public unsafe class WeaponNameHook
{
    public UItemNameListTable* table;
    public WeaponNameHook(IUObjects uObjects, IUnreal unreal, WeaponRegistry registry)
    {
        uObjects.FindObject("DatItemWeaponNameDataAsset", obj =>
        {

            var nameTable = (UItemNameListTable*)obj.Self;
            table = nameTable;
            var nameEntries = nameTable->Data;
            void SetName(int index, string name = "Unused")
            {
                nameTable->Data.AllocatorInstance[index] = unreal.FString(name);
            }
            var firstNameCount = nameTable->Data.Num;
            var weapCount = registry.Weapons.Count;
            var entriesToAdd = 100;
            Log.Debug($"Generating {entriesToAdd} additional name slots.");
            for (int i = 0; i < entriesToAdd; i++)
            {
                var idx = firstNameCount + i;
                SetName(idx);
            }

            var nameCount = nameTable->Data.Num;
            for (int i = 0; i < registry.Weapons.Count; i++)
            {
                var weapon = registry.Weapons.FirstOrDefault(x => x.WeaponItemId == i);

                if (weapon?.Name != null)
                {
                    nameTable->Data.AllocatorInstance[i] = unreal.FString(weapon.Name);
                    Log.Debug($"Set name for Weapon Item ID: {weapon.WeaponItemId} || Name: {weapon.Name}");
                }
                continue;
            }
        });
    }
}
