using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Sequential)]
public struct FWeakObjectPtr
{
    public int ObjectIndex;
    public int ObjectSerialNumber;
}
