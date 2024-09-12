using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Sequential)]
public struct FWeakObjectPtr
{
    public int ObjectIndex;
    public int ObjectSerialNumber;

    public unsafe FWeakObjectPtr(object obj) : this()
    {
        var ptrToObj = &obj;
        var ptr = (nint)ptrToObj;
        ObjectIndex = (int)ptr;
    }
}
