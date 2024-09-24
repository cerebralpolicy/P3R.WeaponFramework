using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Interfaces.Types;
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct FGuid
{
    [FieldOffset(0x0)] public uint A;
    [FieldOffset(0x4)] public uint B;
    [FieldOffset(0x8)] public uint C;
    [FieldOffset(0xc)] public uint D;

    public override string ToString() => $"{A:X}-{B:X}-{C:X}-{D:X}";
}

[StructLayout(LayoutKind.Sequential)]
public struct FUniqueObjectGuid
{
    public FGuid Guid;
}

// For g_namePool
[StructLayout(LayoutKind.Explicit, Size = 0x8)]
public unsafe struct FName : IEquatable<FName>, IEquatable<Native.FName>, IEquatable<Emitter.FName>, IMapHashable
{
    [FieldOffset(0x0)] public uint pool_location;
    [FieldOffset(0x4)] public uint field04;

    public FName(uint pool_location, uint field04)
    {
        this.pool_location = pool_location;
        this.field04 = field04;
    }

    public bool Equals(FName other) => pool_location == other.pool_location;

    public bool Equals(Native.FName other) => pool_location == other.pool_location;

    public bool Equals(Emitter.FName other) => pool_location == other.pool_location;

    public static implicit operator API.FName(Native.FName name) => new(name.pool_location, name.field04);
    public static implicit operator API.FName(Emitter.FName name) => new(name.pool_location, name.unk1);
    public static implicit operator Native.FName(API.FName name) => new() { pool_location = name.pool_location, field04 = name.field04 };
    public static implicit operator Emitter.FName(API.FName name) => new() { pool_location = name.pool_location, unk1 = name.field04 };

    public unsafe static implicit operator FName*(FName name) => &name;
    public unsafe static implicit operator FName(FName* name) => *name;
    public unsafe static implicit operator Native.FName*(FName name) => (Native.FName*)&name;
    public unsafe static implicit operator Emitter.FName*(FName name) => (Emitter.FName*)&name;
    public unsafe static implicit operator FName(Native.FName* name) => (FName*)&name;
    public unsafe static implicit operator FName(Emitter.FName* name) => (FName*)&name;

    public uint GetTypeHash()
    {
        uint block = pool_location >> 0x10;
        uint offset = pool_location & 0xffff;
        return (block << 19) + block + (offset << 0x10) + offset + (offset >> 4) + field04;
    }
}
[StructLayout(LayoutKind.Sequential)]
public unsafe struct FString
{
    TArray<nint> Text;
    public FString(IUnreal unreal, string str)
    {
        Text.arr_max = str.Length + 1;
        Text.arr_num = Text.arr_max;
        Text.allocator_instance = (nint*)unreal.FMalloc(Text.arr_max * sizeof(nint), 0);
        var bytes = Encoding.Unicode.GetBytes(str + '\0');
        Marshal.Copy(bytes, 0, Text.arr_num, bytes.Length);
    }
    public FString(IMemoryMethods mem, string str)
    {
        Text.arr_max = str.Length + 1;
        Text.arr_num = Text.arr_max;
        Text.allocator_instance = (nint*)mem.FMemory_Malloc(Text.arr_max * sizeof(nint), 0);
        var bytes = Encoding.Unicode.GetBytes(str + '\0');
        Marshal.Copy(bytes, 0, Text.arr_num, bytes.Length);
    }
    public FString(string str)
    {
        Text.arr_max = str.Length + 1;
        Text.arr_num = Text.arr_max;
        Text.allocator_instance = (nint*)memAPI.WFMemory_Malloc(Text.arr_max * sizeof(nint), 0);
        var bytes = Encoding.Unicode.GetBytes(str + '\0');
        Marshal.Copy(bytes, 0, Text.arr_num, bytes.Length);

    }

    public static implicit operator Native.FString(FString fStr)
    {
        return new Native.FString()
        {
            text = fStr.Text,
        };
    }
    public static implicit operator Emitter.FString(FString fStr)
    {
#pragma warning disable CS0618 // Type or member is obsolete - WF FStrings are FMalloc'd through the emitter
        var str = fStr.ToString();
        if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            throw new ArgumentNullException(nameof(str));
        return new Emitter.FString(str);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public readonly override string? ToString()
        => Marshal.PtrToStringUni((nint)Text.allocator_instance, Text.arr_num);

    public readonly void Dispose()
        => Marshal.FreeHGlobal((nint)Text.allocator_instance);

    public override int GetHashCode()
        => Text.allocator_instance->GetHashCode();
}
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct UAppDataAsset
{
    [FieldOffset(0x0)] public UDataAsset baseObj;
    [FieldOffset(0x30)] public UAppDataAsset* nativeClass;
}
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct UDataAsset
{
    [FieldOffset(0x0)] public UObject baseObj;
    [FieldOffset(0x28)] public UDataAsset* nativeClass;
}

[StructLayout(LayoutKind.Explicit, Size = 0x3A0)]
public unsafe struct USkeletalMesh
{
    //[FieldOffset(0x0000)] public UStreamableRenderAsset baseObj;
    //[FieldOffset(0x0080)] public USkeleton* Skeleton;
    //[FieldOffset(0x0088)] public FBoxSphereBounds ImportedBounds;
    //[FieldOffset(0x00A4)] public FBoxSphereBounds ExtendedBounds;
    //[FieldOffset(0x00C0)] public FVector PositiveBoundsExtension;
    //[FieldOffset(0x00CC)] public FVector NegativeBoundsExtension;
    //[FieldOffset(0x00D8)] public TArray<FSkeletalMaterial> Materials;
    //[FieldOffset(0x00E8)] public TArray<FBoneMirrorInfo> SkelMirrorTable;
    //[FieldOffset(0x00F8)] public TArray<FSkeletalMeshLODInfo> LODInfo;
    //[FieldOffset(0x0158)] public FPerPlatformInt MinLOD;
    //[FieldOffset(0x015C)] public FPerPlatformBool DisableBelowMinLodStripping;
    //[FieldOffset(0x015D)] public EAxis SkelMirrorAxis;
    //[FieldOffset(0x015E)] public EAxis SkelMirrorFlipAxis;
    //[FieldOffset(0x015F)] public byte bUseFullPrecisionUVs;
    //[FieldOffset(0x015F)] public byte bUseHighPrecisionTangentBasis;
    //[FieldOffset(0x015F)] public byte bHasBeenSimplified;
    //[FieldOffset(0x015F)] public byte bHasVertexColors;
    //[FieldOffset(0x015F)] public byte bEnablePerPolyCollision;
    //[FieldOffset(0x0160)] public UBodySetup* BodySetup;
    //[FieldOffset(0x0168)] public UPhysicsAsset* PhysicsAsset;
    //[FieldOffset(0x0170)] public UPhysicsAsset* ShadowPhysicsAsset;
    //[FieldOffset(0x0178)] public TArray<IntPtr> NodeMappingData;
    //[FieldOffset(0x0188)] public byte bSupportRayTracing;
    //[FieldOffset(0x0190)] public TArray<IntPtr> MorphTargets;
    //[FieldOffset(0x0318)] public TSubclassOf<UAnimInstance> PostProcessAnimBlueprint;
    //[FieldOffset(0x0320)] public TArray<IntPtr> MeshClothingAssets;
    //[FieldOffset(0x0330)] public FSkeletalMeshSamplingInfo SamplingInfo;
    //[FieldOffset(0x0360)] public TArray<IntPtr> AssetUserData;
    //[FieldOffset(0x0370)] public TArray<IntPtr> Sockets;
    //[FieldOffset(0x0390)] public TArray<FSkinWeightProfileInfo> SkinWeightProfiles;
}

