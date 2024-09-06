using System.Collections;
using System.Runtime.InteropServices;

namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x144)]
public unsafe struct FAppCharWeaponTableRow: IEnumerable<TMapElement<int, FAppCharWeaponMeshData>>
{
    [FieldOffset(0x008)] public TMap<int,FAppCharWeaponMeshData> Data;
    [FieldOffset(0x058)] public TSoftClassProperty<UObject> Anim;
    //[FieldOffset(0x080)] public TArray<FAppCharWeaponAnimAssetTypeData> AnimAsset;


    public TMapElement<int, FAppCharWeaponMeshData> this[int index] => Data.elements[index];
    public readonly int Count => Data.mapNum;
    public IEnumerator<TMapElement<int, FAppCharWeaponMeshData>> GetEnumerator() => new TMapWrapper<int, FAppCharWeaponMeshData>(Data).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public unsafe class TMapWrapper<KeyType, ValueType> : IEnumerable<TMapElement<KeyType, ValueType>>, IEnumerator<TMapElement<KeyType, ValueType>>
    where ValueType : unmanaged, IEquatable<ValueType>
    where KeyType : unmanaged, IEquatable<KeyType>
{
    private readonly TMap<KeyType, ValueType> map;
    private int pos = 0;
    public TMapElement<KeyType, ValueType> Current => this.map.elements[pos];

    object IEnumerator.Current => this.Current;

    public TMapWrapper(TMap<KeyType, ValueType> map)
    {
        this.map = map;
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public IEnumerator<TMapElement<KeyType, ValueType>> GetEnumerator() => this;

    public bool MoveNext() => ++this.pos < this.map.mapNum;

    public void Reset() => this.pos = 0;

    IEnumerator IEnumerable.GetEnumerator() => this;
}
