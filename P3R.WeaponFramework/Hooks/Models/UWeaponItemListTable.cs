using System.Collections;
using System.Runtime.InteropServices;

namespace P3R.WeaponFramework.Hooks.Models;

[StructLayout(LayoutKind.Explicit, Size = 0x40)]
public unsafe struct UWeaponItemListTable : IEnumerable<FWeaponItemList>, IReadOnlyList<FWeaponItemList>
{
    //[FieldOffset(0x0000)] public UAppDataAsset baseObj;
    [FieldOffset(0x0030)] public TArray<FWeaponItemList> Data;

    public FWeaponItemList this[int index] => Data.allocator_instance[index];

    public readonly int Count => Data.arr_num;

    public readonly IEnumerator<FWeaponItemList> GetEnumerator() => new TArrayWrapper<FWeaponItemList>(Data).GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}