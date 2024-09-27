using P3R.WeaponFramework.Enums;
using P3R.WeaponFramework.Weapons.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Unreal.ObjectsEmitter.Interfaces;
using static P3R.WeaponFramework.Types.Shell;

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
public static class ShellExtensions
{
    public static ShellDB ShellLookup { get; } = [
        new (ShellType.Player, [Armature.Wp0001_01], [10, 11, 12, 13, 14, 15, 16, 17, 18, 19], astrea: false),
        new (ShellType.Yukari, [Armature.Wp0002_01], [20, 21, 22, 23, 24, 25, 26, 27, 28]),
        new (ShellType.Stupei, [Armature.Wp0003_01], [30, 31, 32, 33, 34, 35, 36, 37, 38, 39]),
        new (ShellType.Akihiko, [Armature.Wp0004_01, Armature.Wp0004_02], [40, 41, 42, 43, 44, 45, 46, 47, 48]),
        new (ShellType.Mitsuru, [Armature.Wp0005_01], [50, 51, 52, 53, 54, 55, 56, 57]),
        new (ShellType.Aigis_SmallArms, [Armature.Wp0007_01, Armature.Wp0007_02], [326, 327]),
        new (ShellType.Aigis_LongArms, [Armature.Wp0007_03], [584, 585, 586, 587, 588, 589]),
        new (ShellType.Ken, [Armature.Wp0008_01], [80, 81, 82, 83, 84, 85, 86, 87, 88, 89]),
        new (ShellType.Koromaru, [Armature.Wp0009_01], [90, 91, 92, 93, 94, 95, 96, 97]),
        new (ShellType.Shinjiro, [Armature.Wp0010_01], [100, 101, 102, 103, 104, 105], astrea: false),
        new (ShellType.Metis, [Armature.Wp0011_01], [100, 101, 102, 103, 104, 105, 106], vanilla: false),
        ];
    public static ShellType GetShellType(this Weapon weapon) => ShellLookup[weapon].Key;
    public static Shell AsShell(this ShellType type) => ShellLookup[type];
    public static bool TryGetCharacterFromShell(this ShellType shell, [NotNullWhen(true)] out Character? character)
    {
        character = CharacterFromShell(shell);
        if (!character.HasValue)
            return false;
        return true;
    }
    public static Character? CharacterFromShell(ShellType shell) => Characters.Lookup.First(x => x.Value.Contains(shell)).Key;
    public static int RequiredMeshes(this ShellType type) => ShellLookup[type].Meshes;
   
}
public struct ArmatureInfo
{
    public Armature Self;
    public string ArmatureBasePath;
    public string ArmatureShellPath;

    public ArmatureInfo(Armature self)
    {
        Self = self;
        ArmatureBasePath = self.GetArmatureBasePath();
        ArmatureShellPath = self.GetArmatureShellPath();
    }

    public static implicit operator ArmatureInfo(Armature armature) => new ArmatureInfo(armature);
}
public struct PathInfo
{
    public string BasePath;
    public string ShellPath;

    public PathInfo(ArmatureInfo info)
    {
        BasePath = info.ArmatureBasePath;
        ShellPath = info.ArmatureShellPath;
    }

    public PathInfo(string basePath, string shellPath)
    {
        BasePath = basePath;
        ShellPath = shellPath;
    }
}

public class ShellData : Tuple<ICollection<ArmatureInfo>, ICollection<PathInfo>, ICollection<int>>
{
    public ShellData(ICollection<ArmatureInfo> item1, ICollection<PathInfo> item2, ICollection<int> item3) : base(item1, item2, item3)
    {
    }
    public List<string> GetBasePaths()
    {
        List<string> paths = [];
        foreach (var item in Item2)
        {
            paths.Add(item.BasePath);
        }
        return paths;
    }
    public List<string> GetShellPaths()
    {
        List<string> paths = [];
        foreach (var item in Item2)
        {
            paths.Add(item.ShellPath);
        }
        return paths;
    }
}
public struct Shell
{
    private ShellType key;
    private ShellData value;
    private bool vanilla;
    private bool astrea;
    public Shell(ShellType shellType, ICollection<ArmatureInfo> armatures, ICollection<int> ids, bool vanilla = true, bool astrea = true) : this()
    {
        this.key = shellType;
        ICollection<PathInfo> pathInfos = [];
        foreach (var armature in armatures)
        {
            pathInfos.Add(new PathInfo(armature));
        }
        this.value = new(armatures, pathInfos, ids);
        this.vanilla = vanilla;
        this.astrea = astrea;
    }
    public int Meshes => value.Item2.Count;
    public ShellType Key => key;
    public ShellData Value => value;
    public List<string> BasePaths => Value.GetBasePaths();
    public List<string> ShellPaths => Value.GetShellPaths();
    public List<int> Ids => [.. Value.Item3];
    public readonly bool IsAstrea => astrea;
    public readonly bool IsVanilla => vanilla;
}
public class ShellDB : KeyedCollection<ShellType, Shell>
{
    public Shell this[Weapon weapon] => this[GetShellFromWeapon(weapon)];
    public ShellType GetShellFromWeapon(Weapon weapon) => Items.First(x => x.Ids.Contains(weapon.ModelId) && x.IsAstrea == weapon.IsAstrea && x.IsVanilla == weapon.IsVanilla).Key;
    protected override ShellType GetKeyForItem(Shell item) => Items.First(x => x.Equals(item)).Key;
}