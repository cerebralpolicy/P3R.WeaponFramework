using P3R.WeaponFramework.Enums;
using P3R.WeaponFramework.Weapons.Models;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Types;

public enum ShellType
{
    None = 0,
    Player = 10,
    Yukari = 20,
    Stupei = 30,
    Akihiko = 40,
    Mitsuru = 50,
    Aigis_SmallArms = 326,
    Aigis_LongArms = 584,
    Ken = 80,
    Koromaru = 90,
    Shinjiro = 100,
    Metis = 110,
}

public class ShellTypeWrapper : WFEnumWrapper<ShellTypeWrapper, ShellType>
{
    public static ShellTypeWrapper None { get; } = new(ShellType.None);
    public static ShellTypeWrapper Player { get; } = new( ShellType.Player, [ArmatureWrapper.Wp0001_01], [10, 11, 12, 13, 14, 15, 16, 17, 18, 19]);
    public static ShellTypeWrapper Yukari { get; } = new( ShellType.Yukari, [ArmatureWrapper.Wp0002_01], [20, 21, 22, 23, 24, 25, 26, 27, 28]);
    public static ShellTypeWrapper Stupei { get; } = new( ShellType.Stupei, [ArmatureWrapper.Wp0003_01], [30, 31, 32, 33, 34, 35, 36, 37, 38, 39]);
    public static ShellTypeWrapper Akihiko { get; } = new( ShellType.Akihiko, [ArmatureWrapper.Wp0004_01, ArmatureWrapper.Wp0004_02], [40, 41, 42, 43, 44, 45, 46, 47, 48]);
    public static ShellTypeWrapper Mitsuru { get; } = new( ShellType.Mitsuru, [ArmatureWrapper.Wp0005_01], [50, 51, 52, 53, 54, 55, 56, 57]);
    public static ShellTypeWrapper Aigis_SmallArms { get; } = new( ShellType.Aigis_SmallArms, [ArmatureWrapper.Wp0007_01, ArmatureWrapper.Wp0007_02], [326, 327]);
    public static ShellTypeWrapper Aigis_LongArms { get; }  = new(ShellType.Aigis_LongArms, [ArmatureWrapper.Wp0007_03], [584, 585, 586, 587, 588, 589]);
    public static ShellTypeWrapper Ken { get; } = new( ShellType.Ken, [ArmatureWrapper.Wp0008_01], [80, 81, 82, 83, 84, 85, 86, 87, 88, 89]);
    public static ShellTypeWrapper Koromaru { get; } = new( ShellType.Koromaru, [ArmatureWrapper.Wp0009_01], [90, 91, 92, 93, 94, 95, 96, 97]);
    public static ShellTypeWrapper Shinjiro { get; } = new( ShellType.Shinjiro, [ArmatureWrapper.Wp0010_01], [100, 101, 102, 103, 104, 105]);
    public static ShellTypeWrapper Metis { get; } = new( ShellType.Metis, [ArmatureWrapper.Wp0011_01], [100, 101, 102, 103, 104, 105, 106]);
    public int ShellTableBaseModelId { get; }
    public List<ArmatureWrapper> Armatures { get; }
    public List<int> ShellTableModelIds { get; }
    public ShellTypeWrapper(ShellType enumValue, List<ArmatureWrapper> armatures, List<int> modelIds) : base(enumValue)
    {
        Armatures = armatures;
        ShellTableModelIds = modelIds;
        ShellTableBaseModelId = modelIds.First();
    }
    public ShellTypeWrapper(ShellType enumValue) : base(enumValue)
    {
        ShellTableModelIds = new List<int>();
        ShellTableBaseModelId = 0;
        Armatures = [];
    }
    public bool Init(IUnreal unreal)
    {
        List<bool> results = [];
        foreach (var armature in Armatures)
        {
            results.Add(armature.InitShellRedirect(unreal));
        }
        return results.All(x => true);
    }
    public bool Apply(Weapon weapon, IUnreal unreal)
    {

        var modOwner = weapon.OwnerModId;
        var shell = FromName(weapon.ShellTarget.ToString());
        if (shell.ShellTableBaseModelId != ShellTableBaseModelId)
            return false;
        weapon.TryGetPaths(out var paths);
        if (paths == null || modOwner == null)
            return false;
        if (paths.Count != shell.Armatures.Count)
            return false;
        for (int i = 0; i < paths.Count; i++)
        {
            shell.Armatures[i].AssignNewRedirect(modOwner, unreal, paths[i]);
            continue;
        }
        return true;
    }
    public static implicit operator ShellTypeWrapper(ShellType shellType) => FromName(shellType.ToString());
    public static implicit operator ShellType(ShellTypeWrapper shellType) => Enum.Parse<ShellType>(shellType.Name);

    public int GetRequiredMeshes() => Armatures.Count;

    public static ShellTypeWrapper FromWeapon(Weapon weapon)
    {
        if (weapon.Character == Character.Metis)
            return Metis;
        else if (weapon.Character == Character.Shinjiro)
            return Shinjiro;
        else
        {
            return List.First(x => x.ShellTableModelIds.Contains(weapon.ModelId));
        }
    }

    public List<string> GetBasePaths()
    {
        var paths = new List<string>();
        foreach (var armature in Armatures)
        {
            paths.Add(armature.BasePath);
        }
        return paths;
    }
    public List<string> GetShellPaths()
    {
        var paths = new List<string>();
        foreach (var armature in Armatures)
        {
            paths.Add(armature.ShellPath);
        }
        return paths;
    }
}

public static class ShellExtensions
{
    public static ShellTypeWrapper GetWrapper(this ShellType shellType, bool isAstrea) => GetWrapper((int)shellType, isAstrea);
    public static ShellTypeWrapper GetWrapper(int modelId, bool isAstrea)
    {
        if (modelId >= 584)
            return ShellTypeWrapper.Aigis_LongArms;
        else if (modelId >= 326)
            return ShellTypeWrapper.Aigis_SmallArms;
        else if (modelId >= 100)
        {
            if (isAstrea)
                return ShellTypeWrapper.Metis;
            else
                return ShellTypeWrapper.Shinjiro;
        }
        else if (modelId >= 90)
            return ShellTypeWrapper.Koromaru;
        else if (modelId >= 80)
            return ShellTypeWrapper.Ken;
        else if (modelId >= 50)
            return ShellTypeWrapper.Mitsuru;
        else if (modelId >= 40)
            return ShellTypeWrapper.Akihiko;
        else if (modelId >= 30)
            return ShellTypeWrapper.Stupei;
        else if (modelId >= 20)
            return ShellTypeWrapper.Yukari;
        else if (modelId >= 10)
            return ShellTypeWrapper.Player;
        else
            return ShellTypeWrapper.None;
    }
    public static bool TryGetCharacterFromShell(this ShellTypeWrapper shell, [NotNullWhen(true)] out CharacterWrapper? character)
    {
        character = CharacterFromShell(shell);
        if (character is null)
            return false;
        return character is CharacterWrapper;
    }
    public static CharacterWrapper? CharacterFromShell(ShellTypeWrapper wrapper) => Characters.WFArmed.Find(x => x.Shells.Contains(wrapper));
    public static int RequiredMeshes(this ShellType type) => ShellTypeWrapper.FromEnum(type).Armatures.Count;
   
}