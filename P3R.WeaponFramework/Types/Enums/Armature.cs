using P3R.WeaponFramework.Enums;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Types;

public enum Armature
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
    /// Metis - Blunt Objects
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

public class ArmatureWrapper : WFEnumWrapper<ArmatureWrapper, Armature>
{
    public const string IDENTITY = "Weapon Framework - Base Shells";

    /// <summary>
    /// <inheritdoc cref="Armature.Wp0001_01"/>
    /// </summary>
    public static ArmatureWrapper Wp0001_01 { get; } = new(Armature.Wp0001_01);
    /// <summary>
    /// Yukari - Bow
    /// </summary>
    public static ArmatureWrapper Wp0002_01 { get; } = new (Armature.Wp0002_01);
    /// <summary>
    /// Stupei - 2H Sword
    /// </summary>
    public static ArmatureWrapper Wp0003_01 { get; } = new (Armature.Wp0003_01);
    /// <summary>
    /// Akihiko - Right Hand
    /// </summary>
    public static ArmatureWrapper Wp0004_01 { get; } = new (Armature.Wp0004_01);
    /// <summary>
    /// Akihiko - Left Hand
    /// </summary>
    public static ArmatureWrapper Wp0004_02 { get; } = new(Armature.Wp0004_02);
    /// <summary>
    /// Mitsuru - Saber
    /// </summary>
    public static ArmatureWrapper Wp0005_01 { get; } = new (Armature.Wp0005_01);
    /// <summary>
    /// Aigis - Right Hand Small Arms
    /// </summary>
    public static ArmatureWrapper Wp0007_01 { get; } = new (Armature.Wp0007_01);
    /// <summary>
    /// Aigis - Left Hand Small Arms
    /// </summary>
    public static ArmatureWrapper Wp0007_02 { get; } = new (Armature.Wp0007_02);
    /// <summary>
    /// Aigis - Long Arms
    /// </summary>
    public static ArmatureWrapper Wp0007_03 { get; } = new (Armature.Wp0007_03);
    /// <summary>
    /// Ken - Spears
    /// </summary>
    public static ArmatureWrapper Wp0008_01 { get; } = new (Armature.Wp0008_01);
    /// <summary>
    /// Koromaru - Knives
    /// </summary>
    public static ArmatureWrapper Wp0009_01 { get; } = new (Armature.Wp0009_01);
    /// <summary>
    /// Shinjiro - Blunt Objects
    /// </summary>
    public static ArmatureWrapper Wp0010_01 { get; } = new (Armature.Wp0010_01);
    /// <summary>
    /// Metis - Blunt Objects
    /// </summary>
    public static ArmatureWrapper Wp0011_01 { get; } = new (Armature.Wp0011_01);
    private readonly string _basePath;
    private readonly string _shellPath;
    public ArmatureWrapper(Armature enumValue) : base(enumValue)
    {
        _basePath = enumValue.GetArmatureBasePath();
        _shellPath = enumValue.GetArmatureShellPath();
    }

    public string BasePath => _basePath;
    public string ShellPath => _shellPath;

    internal unsafe static bool TryGetFName(IUnreal unreal, string name)
    {
        var fname = unreal.FName(name, EFindName.FName_Find);
        if (fname == null)
            return false;
        return true;
    }
    public unsafe bool InitShellRedirect(IUnreal unreal)
    {
        var baseModel = _basePath;
        var shellModel = _shellPath;
        if (TryGetFName(unreal, baseModel))
        {
            unreal.AssignFName(IDENTITY, baseModel, shellModel);
            return true;
        }
        return false;
    }
    public unsafe bool AssignNewRedirect(string weaponModName, IUnreal unreal, string newPath)
    {
        var baseModel = _basePath;
        if (TryGetFName(unreal, baseModel))
        {
            unreal.AssignFName(weaponModName, baseModel, newPath);
            return true;
        }
        return false;
    }
}

public static class Armatures
{
    private enum ModelType
    {
        Main = 1,
        Left,
        Single,
    }
    public static string WeaponFolder(Character character) => $"Wp{character.Format()}";
    public const int SHELL_BASE = 500;
    public static string GetArmatureBasePath(this Armature armature)
    {
        var Name = armature.ToString();
        var Index = (int)armature % 10;
        var weapFolder = Name[0..(Name.Length - 3)];
        if (weapFolder == "Wp0012")
            weapFolder = "Wp0007";
        if (weapFolder != WeaponFolder(Character.Aigis) && weapFolder != WeaponFolder(Character.Akihiko))
            return GetAssetPath($"/Game/Xrd777/Characters/Weapon/{weapFolder}/SK_{weapFolder}_000");
        var model = (ModelType)Index;
        if (weapFolder == WeaponFolder(Character.Aigis))
        {
            int modelId = model switch
            {
                ModelType.Main => 11,
                ModelType.Left => 211,
                ModelType.Single => 0,
                _ => throw new NotImplementedException(),
            };
            return GetAssetPath($"/Game/Xrd777/Characters/Weapon/{weapFolder}/SK_{weapFolder}_{modelId:000}");
        }
        else
        {
            int modelId = model switch
            {
                ModelType.Main => 0,
                ModelType.Left => 200,
                _ => throw new NotImplementedException(),
            };
            return GetAssetPath($"/Game/Xrd777/Characters/Weapon/{weapFolder}/SK_{weapFolder}_{modelId:000}");
        }
    }
    public static string GetArmatureShellPath(this Armature armature)
    {
        var Name = armature.ToString();
        var ShellId = SHELL_BASE + ((int)armature % 10);
        var weapFolder = Name[0..(Name.Length - 3)];
        if (weapFolder == "Wp0012")
            weapFolder = "Wp0007";
        return GetAssetPath($"/Game/Xrd777/Characters/Weapon/Shells/SK_{weapFolder}_{ShellId:000}");
    }
}