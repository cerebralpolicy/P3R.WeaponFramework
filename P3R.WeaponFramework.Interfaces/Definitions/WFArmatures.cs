namespace P3R.WeaponFramework.Interfaces.Types;

public enum EArmature
{
    None,
    /// <summary>
    /// Player - 1H Sword
    /// </summary>
    Wp0001_01 = 11,
    /// <summary>
    /// Yukari - Bow
    /// </summary>
    Wp0002_01 = 21,
    /// <summary>
    /// Stupei - 2H Sword
    /// </summary>
    Wp0003_01 = 31,
    /// <summary>
    /// Akihiko - Right Hand
    /// </summary>
    Wp0004_01 = 41,
    /// <summary>
    /// Akihiko - Left Hand
    /// </summary>
    Wp0004_02 = 42,
    /// <summary>
    /// Mitsuru - Saber
    /// </summary>
    Wp0005_01 = 51,
    /// <summary>
    /// Aigis - Right Hand Small Arms
    /// </summary>
    Wp0007_01 = 71,
    /// <summary>
    /// Aigis - Left Hand Small Arms
    /// </summary>
    Wp0007_02 = 72,
    /// <summary>
    /// Aigis - Long Arms
    /// </summary>
    Wp0007_03 = 73,
    /// <summary>
    /// Ken - Spears
    /// </summary>
    Wp0008_01 = 81,
    /// <summary>
    /// Koromaru - Knives
    /// </summary>
    Wp0009_01 = 91,
    /// <summary>
    /// Shinjiro - Blunt Objects
    /// </summary>
    Wp0010_01 = 101,
    /// <summary>
    /// Metis - BluntO bjects
    /// </summary>
    Wp0011_01 = 111,
    /// <summary>
    /// <inheritdoc cref="Wp0007_01"/> (Episode Aigis)
    /// </summary>
    Wp0012_01 = 121,
    /// <summary>
    /// <inheritdoc cref="Wp0007_02"/> (Episode Aigis)
    /// </summary>
    Wp0012_02 = 122,
    /// <summary>
    /// <inheritdoc cref="Wp0007_03"/> (Episode Aigis)
    /// </summary>
    Wp0012_03 = 123,
}

public enum EArmatureType
{
    Player,
    Yukari,
    Stupei,
    Akihiko,
    Mitsuru,
    Aigis_SmallArms,
    Aigis_LongArms,
    Ken,
    Koromaru,
    Shinjiro,
    Metis,
    Aigis_SmallArms_Astrea,
    Aigis_LongArms_Astrea,
}

public class Armature : WFEnum<Armature, int, FAppCharWeaponMeshData>, IEquatable<Armature>, IComparable<Armature>
{
    public static Armature Wp0001_01 { get; } = new Armature(nameof(Wp0001_01), 1);
    public static Armature Wp0002_01 { get; } = new Armature(nameof(Wp0002_01), 11);
    public static Armature Wp0003_01 { get; } = new Armature(nameof(Wp0003_01), 31);
    public static Armature Wp0004_01 { get; } = new Armature(nameof(Wp0004_01), 41);
    public static Armature Wp0004_02 { get; } = new Armature(nameof(Wp0004_02), 42);
    public static Armature Wp0005_01 { get; } = new Armature(nameof(Wp0005_01), 51);
    public static Armature Wp0007_01 { get; } = new Armature(nameof(Wp0007_01), 71);
    public static Armature Wp0007_02 { get; } = new Armature(nameof(Wp0007_02), 72);
    public static Armature Wp0007_03 { get; } = new Armature(nameof(Wp0007_03), 73);
    public static Armature Wp0008_01 { get; } = new Armature(nameof(Wp0008_01), 81);
    public static Armature Wp0009_01 { get; } = new Armature(nameof(Wp0008_01), 91);
    public static Armature Wp0010_01 { get; } = new Armature(nameof(Wp0008_01), 101);
    public static Armature Wp0011_01 { get; } = new Armature(nameof(Wp0008_01), 111);
    public static Armature Wp0012_01 { get; } = new Armature(nameof(Wp0012_01), 121);
    public static Armature Wp0012_02 { get; } = new Armature(nameof(Wp0012_02), 122);
    public static Armature Wp0012_03 { get; } = new Armature(nameof(Wp0012_03), 123);
    public Armature(string name, int value) : base(name, value)
    {

    }
    public static implicit operator Armature(EArmature weaponArmature) => FromValue((int)weaponArmature);
    public static implicit operator EArmature(Armature armature) => Enum.Parse<EArmature>(armature.Name);

    public unsafe bool AddMesh(int id, string meshPath, bool multi, string? tableName = null)
    {
        // Create a new soft object pointer
        var tSoftObj = memAPI.GetSoftPointer<UObject>(meshPath);
        var meshData = new FAppCharWeaponMeshData(tSoftObj, multi);
        return AddMesh(id, meshData, tableName ?? Name);
    }
    public unsafe bool AddMesh(int id, FAppCharWeaponMeshData meshData, string tableName)
        => memAPI.TMap_Insert(tableName, id, meshData, tableName);
    public int CompareTo(Armature? other)
    {
        if (other == null)
            return 0;
        return 1;
    }

    public bool Equals(Armature? other)
    {
        if (other == null) return false;
        return Value == other.Value;
    }
}
public class ArmatureType : WFEnumBase<ArmatureType, WFEnumCollection<Armature>>
{
    public static ArmatureType Player { get; } = new(nameof(Player), new([Armature.Wp0001_01]));
    public static ArmatureType Yukari { get; } = new(nameof(Yukari), new([Armature.Wp0002_01]));
    public static ArmatureType Stupei { get; } = new(nameof(Stupei), new([Armature.Wp0003_01]));
    public static ArmatureType Akihiko { get; } = new(nameof(Akihiko), new([Armature.Wp0004_01, Armature.Wp0004_02]));
    public static ArmatureType Mitsuru { get; } = new(nameof(Mitsuru), new([Armature.Wp0005_01]));
    public static ArmatureType Aigis_SmallArms { get; } = new(nameof(Aigis_SmallArms), new([Armature.Wp0007_01, Armature.Wp0007_02]));
    public static ArmatureType Aigis_LongArms { get; } = new(nameof(Aigis_LongArms), new([Armature.Wp0007_03]));
    public static ArmatureType Ken { get; } = new(nameof(Ken), new([Armature.Wp0008_01]));
    public static ArmatureType Koromaru { get; } = new(nameof(Koromaru), new([Armature.Wp0009_01]));
    public static ArmatureType Shinjiro { get; } = new(nameof(Shinjiro), new([Armature.Wp0010_01]));
    public static ArmatureType Metis { get; } = new(nameof(Metis), new([Armature.Wp0011_01]));
    public static ArmatureType Aigis_SmallArms_Astrea { get; } = new(nameof(Aigis_SmallArms_Astrea), new ([Armature.Wp0012_01, Armature.Wp0012_02]));
    public static ArmatureType Aigis_LongArms_Astrea { get; } = new(nameof(Aigis_LongArms_Astrea), new([Armature.Wp0012_03]));
    public ArmatureType(string name, WFEnumCollection<Armature> value) : base(name, value)
    {
    }
    public int RequiredMeshes => Value.Count;
    public static implicit operator ArmatureType(EArmatureType armatureType) => FromName(Enum.GetName<EArmatureType>(armatureType));
    public static implicit operator EArmatureType(ArmatureType armatureType) => Enum.Parse<EArmatureType>(armatureType.Name);
}
