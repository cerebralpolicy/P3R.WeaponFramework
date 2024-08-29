using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Types;
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct UDataAsset
{
    [FieldOffset(0x0)] public UObject baseObj;
    [FieldOffset(0x28)] public UDataAsset* nativeClass;
}
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct UAppDataAsset
{
    [FieldOffset (0x0)] public UDataAsset baseObj;
    [FieldOffset (0x30)] public UAppDataAsset* nativeClass;
}