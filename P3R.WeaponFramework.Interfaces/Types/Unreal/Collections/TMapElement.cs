using System.Runtime.InteropServices;


#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TMapElement<KeyType, ValueType>
    where KeyType : unmanaged, IEquatable<KeyType>
    where ValueType : unmanaged
{
    public KeyType Key;
    public ValueType Value;
    public uint HashNextId;
    public uint HashIndex;
}
