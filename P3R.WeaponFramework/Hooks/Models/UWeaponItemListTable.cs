using System.Collections;
using System.Runtime.InteropServices;

namespace P3R.WeaponFramework.Hooks.Models;

[StructLayout(LayoutKind.Explicit, Size = 0x40)]
public unsafe struct UWeaponItemListTable 
{
    //[FieldOffset(0x0000)] public UAppDataAsset baseObj;
    [FieldOffset(0x0030)] public TArray<FWeaponItemList> Data;

    public FWeaponItemList this[int index]
    {
        get { return Data.allocator_instance[index]; }
        set { Data.allocator_instance[index] = value; }
    }

    public readonly int Count => Data.arr_num;

}