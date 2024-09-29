using P3R.WeaponFramework.Enums;
using P3R.WeaponFramework.Weapons.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Unreal.ObjectsEmitter.Interfaces;
using YamlDotNet.Core.Tokens;
using static P3R.WeaponFramework.Types.Characters;

namespace P3R.WeaponFramework.Types;

public enum ShellType
{
    None = 0,
    Unassigned = 1,
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

public struct ArmaturePaths
{
    private string _basePath;
    private string _shellPath;

    public ArmaturePaths(Armature armature)
    {
        _basePath = armature.BasePath;
        _shellPath = armature.ShellPath;
    }
    public string BasePath => _basePath;
    public string ShellPath => _shellPath;
    public static implicit operator ArmaturePaths(Armature armature) => new(armature);
}

public class ShellData : Tuple<ICollection<ArmaturePaths>, ICollection<PathInfo>, ICollection<int>>
{
    public ShellData(ICollection<ArmaturePaths> item1, ICollection<PathInfo> item2, ICollection<int> item3) : base(item1, item2, item3)
    {
    }
    public List<string> GetBasePaths(Weapon weapon)
    {
        var paths = GetBasePaths();
        if (BaseModels.Contains(weapon.ModelId))
            return paths;
        int expectedId(int i) => 200*i+ModelPairsInt[weapon.ModelId];
        int i = 0;
        var newPaths = new List<string>();
        foreach (var path in paths)
        {
            if (!path.EndsWith($"{expectedId(i):000}"))
            {
                newPaths.Add(path.Substring(0, path.Length - 3) + $"{expectedId(i):000}");
            }
            else
            {
                newPaths.Add(path);
            }
            i++;
        }
        return newPaths;    
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
public class Shell : WFEnumWrapper<Shell, ShellType>
{
    private ArmaturePaths toPaths(Armature armature) => new(armature); 
    private readonly List<Armature> armatures = [];
    private List<int> modelIds;

    private bool vanilla;
    private bool astrea;
    private bool cancels;

    public Shell(ShellType enumValue,ICollection<EArmature> armatures, List<int> modelIds, bool vanilla = true, bool astrea = true, bool cancels = false) : base(enumValue)
    {
        foreach (var armature in armatures)
        {
            this.armatures.Add(armature);
        }
        this.modelIds = modelIds;
        this.vanilla = vanilla;
        this.astrea = astrea;
        this.cancels = cancels;
    }
    public int ShellModelId => modelIds.First();
    public int Meshes => this.armatures.Count;
    public List<Armature> Armatures => armatures;
    public List<ArmaturePaths> Paths => armatures
        .ConvertAll(new Converter<Armature, ArmaturePaths>(toPaths));
    public List<string> BasePaths => Paths
        .Select(p => p.BasePath)
        .ToList();
    public List<string> ShellPaths => Paths
        .Select(p => p.ShellPath)
        .ToList();
    public List<string> WeaponPaths(Weapon weapon)
    {
        var modelSet = weapon.SetFromWeapon();
        if (BaseModels.Contains(weapon.ModelId))
        {
            return BasePaths;
        }
        else
        {
            var suffix = (int)modelSet;
            var m = BasePaths;
            m.ForEach(p => { _ = p.Remove(p.Length - 3).Concat($"{(suffix + m.IndexOf(p)*200):000}"); }) ;
            return m;
        }

    }
    public List<int> ModelIds => modelIds;
    public bool Vanilla => this.vanilla;
    public bool Astrea => this.astrea;
    public bool Cancels => this.cancels;
}
public class ShellDatabase : KeyedCollection<ShellType, Shell>
{
    public Shell this[Weapon weapon] => shellFromWeapon(weapon);
    protected override ShellType GetKeyForItem(Shell item) => item.EnumValue;
    public int GetShellModelId(Weapon weapon) => this[weapon].ModelIds.Min();

    public int GetShellModelId(ShellType shell) => this[shell].ModelIds.Min();
    private Shell shellFromWeapon(Weapon weapon) => shellsOfEpisode(weapon.IsAstrea)
        .shellsWithModel(weapon.ModelId)
        .First();
    private List<ShellType> shellsOfList(List<Shell> list) => list
        .Select(shell => shell.EnumValue)
        .ToList();
    private List<Shell> shellsOfEpisode(bool astrea = false) =>
        astrea ? shellsAstrea : shellsVanilla;
    private List<Shell> shellsWithArmature(List<Shell> list, Armature armature) => list
        .Where(x => x.Armatures.Contains(armature))
        .ToList();
    private List<Shell> shellsWithArmature(Armature armature) => Items
        .Where(x => x.Armatures.Contains(armature))
        .ToList();
    
    private List<Shell> shellsWithModel(int modelId) => Items
        .Where(x => x.ModelIds.Contains(modelId))
        .ToList();
    private List<Shell> shellsAstrea => Items
        .Where(x => x.Astrea)
        .ToList();
    private List<Shell> shellsVanilla => Items
        .Where (x => x.Vanilla)
        .ToList();

}

public static class ShellExtensions
{
    public static ShellDatabase ShellLookup => [
        new (ShellType.None, [],[], false, false, true),
        new (ShellType.Unassigned, [],[], false, false, true),
        new (ShellType.Player, [EArmature.Wp0001_01], [10, 11, 12, 13, 14, 15, 16, 17, 18, 19], astrea: false),
        new (ShellType.Yukari, [EArmature.Wp0002_01], [20, 21, 22, 23, 24, 25, 26, 27, 28]),
        new (ShellType.Stupei, [EArmature.Wp0003_01], [30, 31, 32, 33, 34, 35, 36, 37, 38, 39]),
        new (ShellType.Akihiko, [EArmature.Wp0004_01, EArmature.Wp0004_02], [40, 41, 42, 43, 44, 45, 46, 47, 48]),
        new (ShellType.Mitsuru, [EArmature.Wp0005_01], [50, 51, 52, 53, 54, 55, 56, 57]),
        new (ShellType.Aigis_SmallArms, [EArmature.Wp0007_01, EArmature.Wp0007_02], [326, 327]),
        new (ShellType.Aigis_LongArms, [EArmature.Wp0007_03], [584, 585, 586, 587, 588, 589]),
        new (ShellType.Ken, [EArmature.Wp0008_01], [80, 81, 82, 83, 84, 85, 86, 87, 88, 89]),
        new (ShellType.Koromaru, [EArmature.Wp0009_01], [90, 91, 92, 93, 94, 95, 96, 97]),
        new (ShellType.Shinjiro, [EArmature.Wp0010_01], [100, 101, 102, 103, 104, 105], astrea: false),
        new (ShellType.Metis, [EArmature.Wp0011_01], [100, 101, 102, 103, 104, 105, 106], vanilla: false),
        ];

    public static int ModelId(this ShellType shellType) => ShellLookup.GetShellModelId(shellType);
    public static List<Shell> shellsWithModel(this List<Shell> list, int modelId) => list
        .Where(x => x.ModelIds.Contains(modelId))
        .ToList();
    public static List<string> BasePaths(this ShellType shellType) => ShellLookup[shellType].Paths.Select(p => p.BasePath).ToList();
    public static List<string> ShellPaths(this ShellType shellType) => ShellLookup[shellType].Paths.Select(p => p.ShellPath).ToList();
    public static ShellType GetShellType(this Weapon weapon) => ShellLookup[weapon].EnumValue;
    public static Shell AsShell(this ShellType type) => ShellLookup[type];
    public static bool TryGetCharacterFromShell(this ShellType shell, [NotNullWhen(true)] out ECharacter? character)
    {
        character = CharacterFromShell(shell);
        if (!character.HasValue)
            return false;
        return true;
    }

    public static ECharacter CharacterFromShell(ShellType shell) => Lookup.FirstOfList(Lookup.HasShell(shell));
    public static int RequiredMeshes(this ShellType type) => ShellLookup[type].Meshes;

}