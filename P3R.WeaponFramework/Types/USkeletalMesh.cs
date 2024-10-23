using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Types;
public enum EBoneTranslationRetargetingMode
{
    Animation = 0,
    Skeleton = 1,
    AnimationScaled = 2,
    AnimationRelative = 3,
    OrientAndScale = 4,
};

[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct FBoneNode
{
    [FieldOffset(0x0000)] public FName Name;
    [FieldOffset(0x0008)] public int ParentIndex;
    [FieldOffset(0x000C)] public EBoneTranslationRetargetingMode TranslationRetargetingMode;
}

[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public unsafe struct FVirtualBone
{
    [FieldOffset(0x0000)] public FName SourceBoneName;
    [FieldOffset(0x0008)] public FName TargetBoneName;
    [FieldOffset(0x0010)] public FName VirtualBoneName;
}

[StructLayout(LayoutKind.Explicit, Size = 0x50)]
public unsafe struct FSmartNameContainer
{
}

[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public unsafe struct FAnimSlotGroup
{
    [FieldOffset(0x0000)] public FName GroupName;
    [FieldOffset(0x0008)] public TArray<FName> SlotNames;
}

[StructLayout(LayoutKind.Explicit, Size = 0x390)]
public unsafe struct USkeleton
{
    [FieldOffset(0x0000)] public UObject baseObj;
    [FieldOffset(0x0038)] public TArray<FBoneNode> BoneTree;
    [FieldOffset(0x0048)] public TArray<FTransform> RefLocalPoses;
    [FieldOffset(0x0170)] public FGuid VirtualBoneGuid;
    [FieldOffset(0x0180)] public TArray<FVirtualBone> VirtualBones;
    [FieldOffset(0x0190)] public TArray<IntPtr> Sockets;
    [FieldOffset(0x01F0)] public FSmartNameContainer SmartNames;
    [FieldOffset(0x0270)] public TArray<IntPtr> BlendProfiles;
    [FieldOffset(0x0280)] public TArray<FAnimSlotGroup> SlotGroups;
    [FieldOffset(0x0380)] public TArray<IntPtr> AssetUserData;
}
[StructLayout(LayoutKind.Explicit, Size = 0x60)]
public unsafe struct UStreamableRenderAsset
{
    [FieldOffset(0x0000)] public UObject baseObj;
    [FieldOffset(0x0040)] public double ForceMipLevelsToBeResidentTimestamp;
    [FieldOffset(0x0048)] public int NumCinematicMipLevels;
    [FieldOffset(0x004C)] public int StreamingIndex;
    [FieldOffset(0x0050)] public int CachedCombinedLODBias;
    [FieldOffset(0x0054)] public byte NeverStream;
    [FieldOffset(0x0054)] public byte bGlobalForceMipLevelsToBeResident;
    [FieldOffset(0x0054)] public byte bHasStreamingUpdatePending;
    [FieldOffset(0x0054)] public byte bForceMiplevelsToBeResident;
    [FieldOffset(0x0054)] public byte bIgnoreStreamingMipBias;
    [FieldOffset(0x0054)] public byte bUseCinematicMipLevels;
}
[StructLayout(LayoutKind.Explicit, Size = 0x1C)]
public unsafe struct FBoxSphereBounds
{
    [FieldOffset(0x0000)] public FVector Origin;
    [FieldOffset(0x000C)] public FVector BoxExtent;
    [FieldOffset(0x0018)] public float SphereRadius;
}
[StructLayout(LayoutKind.Explicit, Size = 0x3A0, Pack = 0x8)]
public unsafe struct USkeletalMesh
{
    [FieldOffset(0x0000)] public UStreamableRenderAsset baseObj;
    [FieldOffset(0x0080)] public USkeleton* Skeleton;
    [FieldOffset(0x0088)] public FBoxSphereBounds ImportedBounds;
    [FieldOffset(0x00A4)] public FBoxSphereBounds ExtendedBounds;
    [FieldOffset(0x00C0)] public FVector PositiveBoundsExtension;
    [FieldOffset(0x00CC)] public FVector NegativeBoundsExtension;
    [FieldOffset(0x00D8)] public TArray<FSkeletalMaterial> Materials;
    //[FieldOffset(0x00D8)] public TArray<IntPtr> Materials;
    //    [FieldOffset(0x00E8)] public TArray<FBoneMirrorInfo> SkelMirrorTable;
    [FieldOffset(0x00F8)] public TArray<FSkeletalMeshLODInfo> LODInfo;
    //[FieldOffset(0x0158)] public FPerPlatformInt MinLOD;
    //[FieldOffset(0x015C)] public FPerPlatformBool DisableBelowMinLodStripping;
    //[FieldOffset(0x015D)] public EAxis SkelMirrorAxis;
    //[FieldOffset(0x015E)] public EAxis SkelMirrorFlipAxis;
    [FieldOffset(0x015F)] public byte bUseFullPrecisionUVs;
    [FieldOffset(0x015F)] public byte bUseHighPrecisionTangentBasis;
    [FieldOffset(0x015F)] public byte bHasBeenSimplified;
    [FieldOffset(0x015F)] public byte bHasVertexColors;
    [FieldOffset(0x015F)] public byte bEnablePerPolyCollision;
    //[FieldOffset(0x0160)] public UBodySetup* BodySetup;
    //[FieldOffset(0x0168)] public UPhysicsAsset* PhysicsAsset;
    //[FieldOffset(0x0170)] public UPhysicsAsset* ShadowPhysicsAsset;
    [FieldOffset(0x0178)] public TArray<IntPtr> NodeMappingData;
    [FieldOffset(0x0188)] public byte bSupportRayTracing;
    [FieldOffset(0x0190)] public TArray<IntPtr> MorphTargets;
    //[FieldOffset(0x0318)] public TSubclassOf<UAnimInstance> PostProcessAnimBlueprint;
    [FieldOffset(0x0320)] public TArray<IntPtr> MeshClothingAssets;
    //[FieldOffset(0x0330)] public FSkeletalMeshSamplingInfo SamplingInfo;
    [FieldOffset(0x0360)] public TArray<IntPtr> AssetUserData;
    [FieldOffset(0x0370)] public TArray<IntPtr> Sockets;
    //[FieldOffset(0x0390)] public TArray<FSkinWeightProfileInfo> SkinWeightProfiles;
    public bool TryReplaceData(USkeletalMesh? nullableOther)
    {
        try
        {
            ReplaceData(nullableOther);
            return true;
        }
        catch (Exception e)
        {
            Log.Error(e, e.InnerException!.Message);
            return false;
        }
    }
    public void ReplaceData(USkeletalMesh* other)
    {
        baseObj = other->baseObj;
        Skeleton = other->Skeleton;
        ImportedBounds = other->ImportedBounds;
        ExtendedBounds = other->ExtendedBounds;
        PositiveBoundsExtension = other->PositiveBoundsExtension;
        NegativeBoundsExtension = other->NegativeBoundsExtension;
        bUseFullPrecisionUVs = other->bUseFullPrecisionUVs;
        bUseHighPrecisionTangentBasis = other->bUseHighPrecisionTangentBasis;
        bHasBeenSimplified = other->bHasBeenSimplified;
        bHasVertexColors = other->bHasVertexColors;
        bEnablePerPolyCollision = other->bEnablePerPolyCollision;
        NodeMappingData = other->NodeMappingData;
        bSupportRayTracing = other->bSupportRayTracing;
        MorphTargets = other->MorphTargets;
        MeshClothingAssets = other->MeshClothingAssets;
        AssetUserData = other->AssetUserData;
        Sockets = other->Sockets;
    }
    public void ReplaceData(USkeletalMesh other)
    {
        baseObj = other.baseObj;
        Skeleton = other.Skeleton;
        ImportedBounds = other.ImportedBounds;
        ExtendedBounds = other.ExtendedBounds;
        PositiveBoundsExtension = other.PositiveBoundsExtension;
        NegativeBoundsExtension = other.NegativeBoundsExtension;
        bUseFullPrecisionUVs = other.bUseFullPrecisionUVs;
        bUseHighPrecisionTangentBasis = other.bUseHighPrecisionTangentBasis;
        bHasBeenSimplified = other.bHasBeenSimplified;
        bHasVertexColors = other.bHasVertexColors;
        bEnablePerPolyCollision = other.bEnablePerPolyCollision;
        NodeMappingData = other.NodeMappingData;
        bSupportRayTracing = other.bSupportRayTracing;
        MorphTargets = other.MorphTargets;
        MeshClothingAssets = other.MeshClothingAssets;
        AssetUserData = other.AssetUserData;
        Sockets = other.Sockets;
    }
    public void ReplaceData(USkeletalMesh? nullableOther)
    {
        if (nullableOther == null)
            return;
        var other = nullableOther.Value;
        baseObj = other.baseObj;
        Skeleton = other.Skeleton;
        ImportedBounds = other.ImportedBounds;
        ExtendedBounds = other.ExtendedBounds;
        PositiveBoundsExtension = other.PositiveBoundsExtension;
        NegativeBoundsExtension = other.NegativeBoundsExtension;
        bUseFullPrecisionUVs = other.bUseFullPrecisionUVs;
        bUseHighPrecisionTangentBasis = other.bUseHighPrecisionTangentBasis;
        bHasBeenSimplified = other.bHasBeenSimplified;
        bHasVertexColors = other.bHasVertexColors;
        bEnablePerPolyCollision = other.bEnablePerPolyCollision;
        NodeMappingData = other.NodeMappingData;
        bSupportRayTracing = other.bSupportRayTracing;
        MorphTargets = other.MorphTargets;
        MeshClothingAssets = other.MeshClothingAssets;
        AssetUserData = other.AssetUserData;
        Sockets = other.Sockets;
    }
    public static USkeletalMesh Blank
    {
        get {
            return
                new USkeletalMesh()
                {
                    baseObj = new(),
                    Skeleton = null,
                    ImportedBounds = new(),
                    ExtendedBounds = new(),
                    PositiveBoundsExtension = new(),
                    NegativeBoundsExtension = new(),
                    bUseFullPrecisionUVs = new(),
                    bUseHighPrecisionTangentBasis = new(),
                    bHasBeenSimplified = new(),
                    bHasVertexColors = new(),
                    bEnablePerPolyCollision = new(),
                    NodeMappingData = new(),
                    bSupportRayTracing = new(),
                    MorphTargets = new(),
                    MeshClothingAssets = new(),
                    AssetUserData = new(),
                    Sockets = new(),
                };
        }
    }
}
[StructLayout(LayoutKind.Explicit, Size = 0x34)]
public unsafe struct FSkeletalMaterial
{
    [FieldOffset(0x00)] public UMaterialInterface* Material;
    [FieldOffset(0x08)] public FName MaterialSlotName;
    [FieldOffset(0x10)] public FName ImportedMaterialSlotName;
    [FieldOffset(0x20)] public FMeshUVChannelInfo UVChannelData;
}
[StructLayout(LayoutKind.Explicit, Size = 0x14)]
public unsafe struct FMeshUVChannelInfo
{
    [FieldOffset(0x00)] public byte bIntialized;
    [FieldOffset(0x01)] public byte bOverrideDensities;
    [FieldOffset(0x04)] public float LocalUVDensities;
}
[StructLayout(LayoutKind.Explicit, Size = 0xB8)]
public unsafe struct FSkeletalMeshLODInfo
{
    [FieldOffset(0x00)] public FPerPlatformFloat ScreenSize;
    [FieldOffset(0x04)] public float LODHysteresis;
    [FieldOffset(0x08)] public TArray<int> LODMaterialMap;
    [FieldOffset(0x18)] public FSkeletalMeshBuildSettings BuildSettings;
    //[FieldOffset(0x2C)] public FSkeletalMeshOptimizationSettings ReductionSettings;
    //[FieldOffset(0x68)] public TArray<FBoneReference> BonesToRemove;
    //[FieldOffset(0x78)] public TArray<FBoneReference> BonesToPrioritize;
    [FieldOffset(0x88)] public float WeightOfPrioritization;
    //[FieldOffset(0x90)] public UAnimSequence BakePose;
    //[FieldOffset(0x98)] public UAnimSequence BakePoseOverride;
    [FieldOffset(0xA0)] public FString SourceImportFilename;
    //[FieldOffset(0xB0)] public ESkinCacheUsage SkinCacheUsage;
    [FieldOffset(0xB1)] public byte bHasBeenSimplified;
    [FieldOffset(0xB1)] public byte bHasPerLODVertexColors;
    [FieldOffset(0xB1)] public byte bAllowCPUAccess;
    [FieldOffset(0xB1)] public byte bSupportUniformlyDistributedSampling;
}
[StructLayout(LayoutKind.Explicit, Size = 0x4)]
public unsafe struct FPerPlatformFloat
{
    [FieldOffset(0x0)] public float Default;
}
[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public unsafe struct FSkeletalMeshBuildSettings
{
    [FieldOffset(0x00)] public byte bRecomputeNormals;
    [FieldOffset(0x00)] public byte bRecomputeTangents;
    [FieldOffset(0x00)] public byte bUseMikkTSpace;
    [FieldOffset(0x00)] public byte bComputeWeightedNormals;
    [FieldOffset(0x00)] public byte bRemoveDegenerates; //like you belong on a cross.
    [FieldOffset(0x00)] public byte bUseHighPrecisionTangentBasis;
    [FieldOffset(0x00)] public byte bUseFullPrecisionUVs;
    [FieldOffset(0x00)] public byte bBuildAdjacencyBuffer;
    [FieldOffset(0x04)] public float ThresholdPosition;
    [FieldOffset(0x08)] public float ThresholdTangentNormal;
    [FieldOffset(0x0C)] public float ThresholdUV;
    [FieldOffset(0x10)] public float MorphThresholdPosition;
}