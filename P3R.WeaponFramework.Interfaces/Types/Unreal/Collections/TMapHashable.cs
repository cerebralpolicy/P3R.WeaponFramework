#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

namespace P3R.WeaponFramework.Interfaces.Types;

public unsafe class TMapHashable<KeyType, ValueType>
    where KeyType : unmanaged, IEquatable<KeyType>, IMapHashable
    where ValueType : unmanaged
{
    // Each instance assumes that values are fixed at particular addresses from init onwards
    public TArray<TMapElementHashable<KeyType, ValueType>>* Elements;
    public int** Hashes;
    public uint* HashSize;
    public TMapHashable(nint ptr, nint hashArrOffset, nint hashSizeOffset) // Address of start of TMap struct (e.g &class_instance->func_map)
    {
        Elements = (TArray<TMapElementHashable<KeyType, ValueType>>*)ptr;
        Hashes = (int**)(ptr + hashArrOffset);
        HashSize = (uint*)(ptr + hashSizeOffset);
    }

    public ValueType* GetByIndex(int idx)
    {
        if (idx < 0 || idx > Elements->arr_num) return null;
        return &Elements->allocator_instance[idx].Value;
    }

    public ValueType* TryGetLinear(KeyType key)
    {
        if (Elements->arr_num == 0 || Elements->allocator_instance == null) return null;
        ValueType* value = null;
        for (int i = 0; i < Elements->arr_num; i++)
        {
            var currElem = &Elements->allocator_instance[i];
            if (currElem->Key.Equals(key))
            {
                value = &currElem->Value;
                break;
            }
        }
        return value;
    }

    public ValueType* TryGetByHash(KeyType key)
    {
        ValueType* value = null;
        // Hash alloc doesn't exist for single element maps,
        // so fallback to linear search
        if (*Hashes == null) return TryGetLinear(key);
        var elementTarget = (*Hashes)[key.GetTypeHash() & (*HashSize - 1)];
        while (elementTarget != -1)
        {
            if (Elements->allocator_instance[elementTarget].Key.Equals(key))
            {
                value = &Elements->allocator_instance[elementTarget].Value;
                break;
            }
            elementTarget = Elements->allocator_instance[elementTarget].HashNextId;
        }
        return value;
    }
}