using P3R.WeaponFramework.Research.Types;
using Project.Utils;

namespace P3R.WeaponFramework.Research;

public static partial class JsonExport
{
    public const uint NullWeap = 0;
    public static void SerializeTArrayWrapper(TArrayWrapper<FWeaponItemList> fWeaponItems, string file)
    {
        var filteredItems = fWeaponItems.Where(w => w.EquipID != NullWeap);
        List<Weapon> weapons = [];
        foreach (var item in filteredItems)
        {
            var thisWeap = new Weapon(item);
            weapons.Append(thisWeap);
            Log.Information($"Processed {thisWeap.Name} [ID: {weapons.Count}]");
        }
    }
    public static void SerializeTArrayWrapper<T>(TArrayWrapper<T> array, string file) where T : unmanaged
    {

    }
}