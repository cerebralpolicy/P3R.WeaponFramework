using System.Runtime.InteropServices;


#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Sequential, Size = 0x10)]
public unsafe struct TArray<T> where T : unmanaged
{
    public T* allocator_instance;
    public int arr_num;
    public int arr_max;

    public T* GetRef(int index) // for arrays of type TArray<FValueType>
    {
        if (index < 0 || index >= arr_num) return null;
        return &allocator_instance[index];
    }

    public V* Get<V>(int index) where V : unmanaged // for arrays of type TArray<FValueType*>
    {
        if (index < 0 || index >= arr_num) return null;
        return *(V**)&allocator_instance[index];
    }
}
