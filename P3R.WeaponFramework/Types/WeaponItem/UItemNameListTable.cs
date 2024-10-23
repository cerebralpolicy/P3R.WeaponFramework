using System.Collections;
using System.Runtime.InteropServices;

namespace P3R.WeaponFramework.Types;
[StructLayout(LayoutKind.Explicit, Size = 0x40)]
public unsafe struct UItemNameListTable : IEnumerable<Emitter.FString>, IReadOnlyList<Emitter.FString>
{
    [FieldOffset(0x0000)] public UAppDataAsset baseObj;
    [FieldOffset(0x0030)] public Emitter.TArray<Emitter.FString> Data;
    //[FieldOffset(0x0038)] public TArray<Emitter.FString> Data;

    public Emitter.FString this[int index] => Data.AllocatorInstance[index];

    public readonly int Count => Data.Num;

    public readonly IEnumerator<Emitter.FString> GetEnumerator() => new TArrayWrapper<Emitter.FString>(Data).GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Enumerable wrapper for TArray.
/// </summary>
public unsafe class TArrayWrapper<T> : IEnumerable<T>, IEnumerator<T>
    where T : unmanaged
{
    private readonly Emitter.TArray<T> array;
    private int pos = 0;

    public T Current => this.array.AllocatorInstance[pos];

    object IEnumerator.Current => this.Current;

    public TArrayWrapper(Emitter.TArray<T> array)
    {
        this.array = array;
    }

    public bool MoveNext() => ++this.pos < this.array.Num;

    public void Reset() => this.pos = 0;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public IEnumerator<T> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => this;
}
