
using Ardalis.SmartEnum;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Interfaces.Types;

public class Armature : WFEnum<Armature, int>, IEquatable<Armature>, IComparable<Armature>
{
    public const string IDENTITY = "Weapon Framework - Base Shells";
    public const int SHELL_BASE = 500;
    /// <value>Property <c>OriginalId</c> indicates the original model suffix.</value>
    public int OriginalId;
    /// <value>Property <c>ShellId</c> indicates the model suffix of the default shell.</value>
    public int ShellId;
    /// <summary>
    /// <list type="table">
    /// <item><b>Original Path:</b> <c>/Game/Xrd777/Characters/Weapon/Wp0001/SK_Wp0001_000</c></item>
    /// <item><b>Shell Path:</b> <c>/Game/Xrd777/Characters/Weapon/Shells/SK_Wp0001_501</c></item>
    /// </list>
    /// </summary>
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
    public static Armature Wp0009_01 { get; } = new Armature(nameof(Wp0009_01), 91);
    public static Armature Wp0010_01 { get; } = new Armature(nameof(Wp0010_01), 101);
    public static Armature Wp0011_01 { get; } = new Armature(nameof(Wp0011_01), 111);
    public static Armature Wp0012_01 { get; } = new Armature(nameof(Wp0012_01), 121);
    public static Armature Wp0012_02 { get; } = new Armature(nameof(Wp0012_02), 122);
    public static Armature Wp0012_03 { get; } = new Armature(nameof(Wp0012_03), 123);
    public Armature(string name, int value) : base(name, value)
    {
        OriginalId = 0;
        int[] model_200 = [42];
        int[] model_210 = [72, 122];
        var offset = (value % 10);
        if (model_200.Contains(value))
            OriginalId = 200;
        else if (model_210.Contains(value))
            OriginalId = 210;
        ShellId = SHELL_BASE + offset;
    }
    public static implicit operator Armature(EArmature weaponArmature) => FromValue((int)weaponArmature);
    public static implicit operator EArmature(Armature armature) => Enum.Parse<EArmature>(armature.Name);
    public override string ToString()
    {
        return GetArmatureShellPath()!;
    }
    public string GetArmatureBasePath()
    {
        var weapFolder = Name[0..(Name.Length - 3)];
        if (weapFolder == "Wp0012")
            weapFolder = "Wp0007";
        return GetAssetPath($"/Game/Xrd777/Characters/Weapon/{weapFolder}/SK_{weapFolder}_{OriginalId:000}");
    }
    public string GetArmatureShellPath()
    {
        var weapFolder = Name[0..(Name.Length-3)];
        if (weapFolder == "Wp0012")
            weapFolder = "Wp0007";
        return GetAssetPath($"/Game/Xrd777/Characters/Weapon/Shells/SK_{weapFolder}_{ShellId:000}");
    }
    internal unsafe static bool TryGetFName(IUnreal unreal, string name)
    {
        var fname = unreal.FName(name, Emitter.EFindName.FName_Find);
        if (fname == null)
            return false;
        return true;
    }
    public unsafe bool InitShellRedirections(IUnreal unreal)
    {
        var baseModel = GetArmatureBasePath();
        var shellModel = GetArmatureShellPath();
        if (TryGetFName(unreal, baseModel))
        {
            unreal.AssignFName(IDENTITY, baseModel, shellModel);
            return true;
        }
        return false;
    }
    public unsafe bool AssignNewRedirect(IUnreal unreal, string newPath)
    {
        var baseModel = GetArmatureBasePath()!;
        if (TryGetFName(unreal, baseModel))
        {
            unreal.AssignFName(IDENTITY, baseModel, newPath);
            return true;
        }
        return false;
    }
    public override int CompareTo(Armature? other)
    {
        if (other == null)
            return 0;
        return 1;
    }

    public override bool Equals(Armature? other)
    {
        if (other == null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Armature);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
public static class Armatures
{
    public static readonly Armature[] All =
    [
        Armature.Wp0001_01,
        Armature.Wp0002_01,
        Armature.Wp0003_01,
        Armature.Wp0004_01,
        Armature.Wp0004_02,
        Armature.Wp0005_01,
        Armature.Wp0007_01,
        Armature.Wp0007_02,
        Armature.Wp0007_03,
        Armature.Wp0008_01,
        Armature.Wp0009_01,
        Armature.Wp0010_01,
        Armature.Wp0011_01,
        Armature.Wp0012_01,
        Armature.Wp0012_02,
        Armature.Wp0012_03,
    ];
    public static readonly Armature[] PairedArmatures =
    [
        Armature.Wp0004_01,
        Armature.Wp0004_02,
        Armature.Wp0007_01,
        Armature.Wp0007_02,
        Armature.Wp0012_02,
        Armature.Wp0012_03,
    ];
    public static bool IsPaired(this Armature self)
        => PairedArmatures.Contains(self);
}
