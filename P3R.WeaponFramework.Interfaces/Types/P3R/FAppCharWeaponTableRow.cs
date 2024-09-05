using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x144)]
public unsafe struct FAppCharWeaponTableRow
{
    [FieldOffset(0x008)] public TMap<int,FAppCharWeaponMeshData> Data;
    [FieldOffset(0x058)] public TSoftClassProperty<UObject> Anim;
    //[FieldOffset(0x080)] public TArray<FAppCharWeaponAnimAssetTypeData> AnimAsset;
}
