using System.Collections;
using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Interfaces.Types;
[StructLayout(LayoutKind.Explicit, Size = 0x40)]
public unsafe struct UWeaponItemListTable : IEnumerable<FWeaponItemList>, IReadOnlyList<FWeaponItemList>
{
    //[FieldOffset(0x0000)] public UAppDataAsset baseObj;
    [FieldOffset(0x0030)] public TArray<FWeaponItemList> Data;
    //[FieldOffset(0x0038)] public TArray<FWeaponItemList> Data;

    public FWeaponItemList this[int index] => Data.AllocatorInstance[index];

    public readonly int Count => Data.Num;

    public readonly IEnumerator<FWeaponItemList> GetEnumerator() => new TArrayWrapper<FWeaponItemList>(Data).GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}