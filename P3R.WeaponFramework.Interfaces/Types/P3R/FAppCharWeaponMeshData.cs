using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x48)]
public unsafe struct FAppCharWeaponMeshData : IEquatable<FAppCharWeaponMeshData>
{
    //[FieldOffset(0x00)] public FSoftObjectPtr<USkeletalMesh>* Mesh;
    [FieldOffset(0x00)] public TSoftObjectPtr<UObject>* Mesh;
    [FieldOffset(0x28)] public bool MultiEquip;

    public FAppCharWeaponMeshData(TSoftObjectPtr<UObject>* meshPtr)
    {
        Mesh = meshPtr;
    }

    public FAppCharWeaponMeshData(IUObjects uObjects, string name)
    {
        uObjects.FindObject(name, SetMesh);
    }

    public FAppCharWeaponMeshData(TSoftObjectPtr<UObject>* mesh, bool multiEquip) : this(mesh)
    {
        MultiEquip = multiEquip;
    }

    private unsafe void SetMesh(Emitter.UnrealObject obj)
    {
        TSoftObjectPtr<Emitter.UObject> ptr = new(obj.Self);
        Mesh = (TSoftObjectPtr<UObject>*)&ptr;
    }

    public bool Equals(FAppCharWeaponMeshData other)
    {
        return (nint)Mesh == (nint)other.Mesh;
    }
}