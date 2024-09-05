using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces.Types;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x48)]
public unsafe struct FAppCharWeaponMeshData 
{
    //[FieldOffset(0x00)] public FSoftObjectPtr<USkeletalMesh>* Mesh;
    [FieldOffset(0x00)] public FSoftObjectPtr<FName>* Mesh;
    [FieldOffset(0x28)] public bool MultiEquip;
}