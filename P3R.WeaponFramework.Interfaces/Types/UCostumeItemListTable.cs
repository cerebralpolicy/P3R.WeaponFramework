using System.Collections;
using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x40)]
public unsafe struct UCostumeItemListTable
    : IEnumerable<FCostumeItemList>, IReadOnlyList<FCostumeItemList>
{
    //[FieldOffset(0x0000)] public UAppDataAsset baseObj;
    [FieldOffset(0x0030)] public TArray<FCostumeItemList> Data;

    public FCostumeItemList this[int index] => Data.AllocatorInstance[index];

    public readonly int Count => Data.Num;

    public readonly IEnumerator<FCostumeItemList> GetEnumerator() => new TArrayWrapper<FCostumeItemList>(Data).GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
