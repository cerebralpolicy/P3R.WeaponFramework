using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x48)]
public unsafe struct FAppCharWeaponMeshData : IEquatable<FAppCharWeaponMeshData>
{
    //[FieldOffset(0x00)] public FSoftObjectPtr<USkeletalMesh>* Mesh;
    [FieldOffset(0x00)] public FSoftObjectPtr* Mesh;
    [FieldOffset(0x28)] public bool MultiEquip;

    public FAppCharWeaponMeshData(FName meshName)
    {
        var objectPtr = new FSoftObjectPtr();
        Mesh = &objectPtr;
    }

    public bool Equals(FAppCharWeaponMeshData other)
    {
        return (nint)Mesh == (nint)other.Mesh;
    }
}