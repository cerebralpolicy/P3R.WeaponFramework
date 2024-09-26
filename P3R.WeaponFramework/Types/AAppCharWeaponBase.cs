using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x340)]
public unsafe struct AAppCharWeaponBase // : AAppActor
{
    [FieldOffset(0x000)] public AAppActor* BaseObj;

    [FieldOffset(0x278)] public Character PlayerId;
    //[FieldOffset(0x280)] public USceneComponent* Root;
    //[FieldOffset(0x288)] public SkeletalMeshComponent* Mesh;
    [FieldOffset(0x290)] public FAppCharWeaponTableRow WeaponTbl;
    [FieldOffset(0x320)] public FName AttachSocketName;
    //[FieldOffset(0x328)] public int HideMaterialId;
    //[FieldOffset(0x330)] public UAppCharWeaponAnimDataAsset* AnimPack;
}
