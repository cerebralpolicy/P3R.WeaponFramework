using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Types;
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct FGuid
{
    [FieldOffset(0x0)] public uint A;
    [FieldOffset(0x4)] public uint B;
    [FieldOffset(0x8)] public uint C;
    [FieldOffset(0xc)] public uint D;

    public override string ToString() => $"{A:X}-{B:X}-{C:X}-{D:X}";
}

[StructLayout(LayoutKind.Sequential)]
public struct FUniqueObjectGuid
{
    public FGuid Guid;
}

// For g_namePool
[StructLayout(LayoutKind.Explicit, Size = 0x8)]
public unsafe struct FName : IEquatable<FName>, IEquatable<Emitter.FName>, IMapHashable
{
    [FieldOffset(0x0)] public uint pool_location;
    [FieldOffset(0x4)] public uint field04;

    public FName(uint pool_location, uint field04)
    {
        this.pool_location = pool_location;
        this.field04 = field04;
    }

    public bool Equals(FName other) => pool_location == other.pool_location;


    public bool Equals(Emitter.FName other) => pool_location == other.pool_location;

    public static implicit operator FName(Emitter.FName name) => new(name.pool_location, name.unk1);
    public static implicit operator Emitter.FName(FName name) => new() { pool_location = name.pool_location, unk1 = name.field04 };

    public unsafe static implicit operator FName*(FName name) => &name;
    public unsafe static implicit operator FName(FName* name) => *name;
    public unsafe static implicit operator Emitter.FName*(FName name) => (Emitter.FName*)&name;
    public unsafe static implicit operator FName(Emitter.FName* name) => (FName*)&name;

    public uint GetTypeHash()
    {
        uint block = pool_location >> 0x10;
        uint offset = pool_location & 0xffff;
        return (block << 19) + block + (offset << 0x10) + offset + (offset >> 4) + field04;
    }

    public override bool Equals(object? obj) => obj is not null && obj is FName && Equals((FName)obj);

    public override int GetHashCode() => (int)GetTypeHash();
}
[StructLayout(LayoutKind.Sequential)]
public unsafe struct FString
{
    TArray<nint> Text;
    public FString(IUnreal unreal, string str)
    {
        Text.arr_max = str.Length + 1;
        Text.arr_num = Text.arr_max;
        Text.allocator_instance = (nint*)unreal.FMalloc(Text.arr_max * sizeof(nint), 0);
        var bytes = Encoding.Unicode.GetBytes(str + '\0');
        Marshal.Copy(bytes, 0, Text.arr_num, bytes.Length);
    }
    public FString(IMemoryMethods mem, string str)
    {
        Text.arr_max = str.Length + 1;
        Text.arr_num = Text.arr_max;
        Text.allocator_instance = (nint*)mem.FMemory_Malloc(Text.arr_max * sizeof(nint), 0);
        var bytes = Encoding.Unicode.GetBytes(str + '\0');
        Marshal.Copy(bytes, 0, Text.arr_num, bytes.Length);
    }

    public static implicit operator Emitter.FString(FString fStr)
    {
#pragma warning disable CS0618 // Type or member is obsolete - WF FStrings are FMalloc'd through the emitter
        var str = fStr.ToString();
        if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            throw new ArgumentNullException(nameof(str));
        return new Emitter.FString(str);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public readonly override string? ToString()
        => Marshal.PtrToStringUni((nint)Text.allocator_instance, Text.arr_num);

    public readonly void Dispose()
        => Marshal.FreeHGlobal((nint)Text.allocator_instance);

    public override int GetHashCode()
        => Text.allocator_instance->GetHashCode();
}
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct UAppDataAsset
{
    [FieldOffset(0x0)] public UDataAsset baseObj;
    [FieldOffset(0x30)] public UAppDataAsset* nativeClass;
}
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct UDataAsset
{
    [FieldOffset(0x0)] public UObject baseObj;
    [FieldOffset(0x28)] public UDataAsset* nativeClass;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TMap<KeyType, ValueType> : IEnumerable<TMapElement<KeyType, ValueType>>
    where KeyType : unmanaged
    where ValueType : unmanaged
{
    public TMapElement<KeyType, ValueType>* elements;
    public int mapNum;
    public int mapMax;

    public ValueType* TryGet(KeyType key)
    {
        if (mapNum == 0 || elements == null) return null;
        ValueType* value = null;
        for (int i = 0; i < mapNum; i++)
        {
            var currElem = &elements[i];
            if (currElem->Key.Equals(key))
            {
                value = &currElem->Value;
                break;
            }
        }
        return value;
    }

    public TMapElement<KeyType, ValueType>* TryGetElement(KeyType key)
    {
        if (mapNum == 0 || elements == null) return null;
        TMapElement<KeyType, ValueType>* value = null;
        for (int i = 0; i < mapNum; i++)
        {
            var currElem = &elements[i];
            if (currElem->Key.Equals(key))
            {
                value = currElem;
                break;
            }
        }

        return value;
    }

    public bool TryGet(KeyType key, out ValueType* value)
    {
        value = TryGet(key);
        return value != null;
    }

    public ValueType* GetByIndex(int idx)
    {
        if (idx < 0 || idx > mapNum) return null;
        return &elements[idx].Value;
    }

    public IEnumerator<TMapElement<KeyType, ValueType>> GetEnumerator()
    {
        var items = new List<TMapElement<KeyType, ValueType>>();
        for (int i = 0; i < this.mapNum; i++)
        {
            items.Add(elements[i]);
        }

        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TMapElement<KeyType, ValueType>
    where KeyType : unmanaged
    where ValueType : unmanaged
{
    public KeyType Key;
    public ValueType Value;
    public uint HashNextId;
    public uint HashIndex;
}

public interface IMapHashable
{
    public uint GetTypeHash();
}