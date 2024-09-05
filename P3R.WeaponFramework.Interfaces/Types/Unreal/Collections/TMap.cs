using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;


#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

namespace P3R.WeaponFramework.Interfaces.Types;
//[StructLayout(LayoutKind.Explicit, Size = 0x50)] // inherits from TSortableMapBase
[StructLayout(LayoutKind.Sequential)]
public unsafe struct TMap<KeyType, ValueType>
    where KeyType : unmanaged, IEquatable<KeyType>
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
    public ValueType* GetByIndex(int idx)
    {
        if (idx < 0 || idx > mapNum) return null;
        return &elements[idx].Value;
    }
}
