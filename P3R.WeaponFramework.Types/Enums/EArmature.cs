using P3R.WeaponFramework.Enums;
using P3R.WeaponFramework.Weapons.Models;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Types;

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

public class Armature : WFEnumWrapper<Armature, EArmature>
{
    public const string IDENTITY = "Weapon Framework - Base Shells";

    private readonly string _basePath;
    private readonly string _shellPath;
    public Armature(EArmature enumValue) : base(enumValue)
    {
        _basePath = enumValue.GetArmatureBasePath();
        _shellPath = enumValue.GetArmatureShellPath();
    }

    public string BasePath => _basePath;
    public string ShellPath => _shellPath;
    public static implicit operator Armature(EArmature eArmature) => new Armature(eArmature);
}

public static class Armatures
{
    private enum ModelType
    {
        Main = 1,
        Left,
        Single,
    }
    public static string WeaponFolder(ECharacter character) => $"Wp{character.Format()}";
    public const int SHELL_BASE = 500;
    public static string GetWeaponBasePath(this EArmature armature, Weapon weapon)
    {
        if (BaseModels.Contains(weapon.ModelId))
        {
            return armature.GetArmatureBasePath();
        }
        else
        {
            var Name = armature.ToString();
            var Index = (int)armature % 10;
            var weapFolder = Name[0..(Name.Length - 3)];
            if (weapFolder == "Wp0012")
                weapFolder = "Wp0007";
            var id = ModelPairsInt[weapon.ModelId]; 
            if (weapFolder != WeaponFolder(ECharacter.Aigis) && weapFolder != WeaponFolder(ECharacter.Akihiko))
                return GetAssetPath($"/Game/Xrd777/Characters/Weapon/{weapFolder}/SK_{weapFolder}_{id:000}");
            var model = (ModelType)Index;
            if (weapFolder == WeaponFolder(ECharacter.Aigis))
            {
                int modelId = model switch
                {
                    ModelType.Main => id,
                    ModelType.Left => id + 200,
                    ModelType.Single => id,
                    _ => throw new NotImplementedException(),
                };
                return GetAssetPath($"/Game/Xrd777/Characters/Weapon/{weapFolder}/SK_{weapFolder}_{modelId:000}");
            }
            else
            {
                int modelId = model switch
                {
                    ModelType.Main => id,
                    ModelType.Left => id + 200,
                    _ => throw new NotImplementedException(),
                };
                return GetAssetPath($"/Game/Xrd777/Characters/Weapon/{weapFolder}/SK_{weapFolder}_{modelId:000}");
            }
        }
    }
    public static string GetArmatureBasePath(this EArmature armature)
    {
        var Name = armature.ToString();
        var Index = (int)armature % 10;
        var weapFolder = Name[0..(Name.Length - 3)];
        if (weapFolder == "Wp0012")
            weapFolder = "Wp0007";
        if (weapFolder != WeaponFolder(ECharacter.Aigis) && weapFolder != WeaponFolder(ECharacter.Akihiko))
            return GetAssetPath($"/Game/Xrd777/Characters/Weapon/{weapFolder}/SK_{weapFolder}_000");
        var model = (ModelType)Index;
        if (weapFolder == WeaponFolder(ECharacter.Aigis))
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
    public static string GetArmatureShellPath(this EArmature armature)
    {
        var Name = armature.ToString();
        var ShellId = SHELL_BASE + ((int)armature % 10);
        var weapFolder = Name[0..(Name.Length - 3)];
        if (weapFolder == "Wp0012")
            weapFolder = "Wp0007";
        return GetAssetPath($"/Game/Xrd777/Characters/Weapon/Shells/SK_{weapFolder}_{ShellId:000}");
    }
}