using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct UAppDataAsset
{
    [FieldOffset(0x0)] public UDataAsset baseObj;
    [FieldOffset(0x30)] public UAppDataAsset* nativeClass;
}