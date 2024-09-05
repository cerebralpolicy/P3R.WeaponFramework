using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces.Types;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public unsafe struct FSoftObjectPath<T>
{
    [FieldOffset(0x0000)] public T AssetPathName;
    [FieldOffset(0x0008)] public FString SubPathString;
}
