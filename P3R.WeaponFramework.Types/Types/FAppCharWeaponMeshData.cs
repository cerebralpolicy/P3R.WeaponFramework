using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace P3R.WeaponFramework.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x48)]
public unsafe struct FAppCharWeaponMeshData : IEquatable<FAppCharWeaponMeshData>
{
    //[FieldOffset(0x00)] public FSoftObjectPtr<USkeletalMesh>* Mesh;
    [FieldOffset(0x00)] public TSoftObjectPtr<USkeletalMesh> Mesh;
    [FieldOffset(0x28)] public bool MultiEquip;

    public override readonly bool Equals(object? obj)
    {
        return obj is FAppCharWeaponMeshData data && Equals(data);
    }
    public readonly bool Equals(FAppCharWeaponMeshData other)
        => EqualityComparer<TSoftObjectPtr<USkeletalMesh>>.Default.Equals(Mesh, other.Mesh) &&
               MultiEquip == other.MultiEquip;

    public static bool operator ==(FAppCharWeaponMeshData left, FAppCharWeaponMeshData right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FAppCharWeaponMeshData left, FAppCharWeaponMeshData right)
    {
        return !(left == right);
    }

    public override readonly int GetHashCode()
    {
        return Mesh.GetHashCode() + MultiEquip.GetHashCode();
    }
}