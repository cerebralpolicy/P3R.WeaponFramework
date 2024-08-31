using System.Collections;
using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Interfaces.Types;
[StructLayout(LayoutKind.Explicit, Size = 0x40)]
public unsafe struct UItemNameListTable : IEnumerable<FString>, IReadOnlyList<FString>
{
    //[FieldOffset(0x0000)] public UAppDataAsset baseObj;
    [FieldOffset(0x0030)] public TArray<FString> Data;
    //[FieldOffset(0x0038)] public TArray<FString> Data;

    public FString this[int index] => Data.AllocatorInstance[index];

    public readonly int Count => Data.Num;

    public readonly IEnumerator<FString> GetEnumerator() => new TArrayWrapper<FString>(Data).GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}