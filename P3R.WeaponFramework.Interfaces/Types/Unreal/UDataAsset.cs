using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces.Types;


namespace P3R.WeaponFramework.Interfaces.Types;
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct UDataAsset
{
    [FieldOffset(0x0)] public UObject baseObj;
    [FieldOffset(0x28)] public UDataAsset* nativeClass;
}
