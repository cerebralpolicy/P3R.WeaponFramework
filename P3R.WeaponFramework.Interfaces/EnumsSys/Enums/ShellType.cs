
using Ardalis.SmartEnum;
using Unreal.ObjectsEmitter.Interfaces;
using YamlDotNet.Serialization;

namespace P3R.WeaponFramework.Interfaces.Types;
[YamlSerializable]
[SmartEnumName(typeof(ShellType))]
public class ShellType : WFEnum<ShellType, WFEnumCollection<Armature>>, IEquatable<ShellType>
{
    public int ShellModelID { get; set; }
    public int WeaponType { get; set; }
    public string Description { get; set; }
    public static ShellType Player { get; } = new(nameof(Player), new([Armature.Wp0001_01]), 10, 1, "One handed swords used by the protagonist.");
    public static ShellType Yukari { get; } = new(nameof(Yukari), new([Armature.Wp0002_01]), 20, 1, "Bows used by Yukari.");
    public static ShellType Stupei { get; } = new(nameof(Stupei), new([Armature.Wp0003_01]), 30, 1, "Two handed swords used by Junpei");
    public static ShellType Akihiko { get; } = new(nameof(Akihiko), new([Armature.Wp0004_01, Armature.Wp0004_02]), 40, 1, "Gloves/Knuckles used by Akihiko [NEEDS 2 MODELS]");
    public static ShellType Mitsuru { get; } = new(nameof(Mitsuru), new([Armature.Wp0005_01]), 50, 1, "Sabres/Rapiers used by Mitsuru.");
    public static ShellType Aigis_SmallArms { get; } = new(nameof(Aigis_SmallArms), new([Armature.Wp0007_01, Armature.Wp0007_02]), 326, 1, "Aigis' FN 5.7x24mm finger cannons - that's what makes the most sense. [NEEDS 2 MODELS]");
    public static ShellType Aigis_LongArms { get; } = new(nameof(Aigis_LongArms), new([Armature.Wp0007_03]), 584, 2, "Aigis' long/heavy firearms.");
    public static ShellType Ken { get; } = new(nameof(Ken), new([Armature.Wp0008_01]), 80, 1, "Spears/Nagitanas used by Ken.");
    public static ShellType Koromaru { get; } = new(nameof(Koromaru), new([Armature.Wp0009_01]), 90, 1, "Whatever will fit in Koro's mouth, go wild.");
    public static ShellType Shinjiro { get; } = new(nameof(Shinjiro), new([Armature.Wp0010_01]), 100, 1, "Shinji's arsenal, brought to you by MC Hammer.");
    public static ShellType Metis { get; } = new(nameof(Metis), new([Armature.Wp0011_01]), 110, 1, "Axes and handmedowns from Shinji, now utilized by Metis");
    public static ShellType Aigis_SmallArms_Astrea { get; } = new(nameof(Aigis_SmallArms_Astrea), new ([Armature.Wp0012_01, Armature.Wp0012_02]), 326, 1, "Aigis' FN 5.7x24mm finger cannons - animations have changed to allow player control. [NEEDS 2 MODELS]");
    public static ShellType Aigis_LongArms_Astrea { get; } = new(nameof(Aigis_LongArms_Astrea), new([Armature.Wp0012_03]), 584, 2, "Aigis' long/heavy firearms.");
    public ShellType(string name, WFEnumCollection<Armature> value, int shellID, int weaponType, string desc) : base(name, value)
    {
        ShellModelID = shellID;
        WeaponType = weaponType;
        Description = desc;
    }
    public int RequiredMeshes => Value.Count;
    public static ShellType FromValue(EShellType shellType) => shellType;
    public static ShellType? FromValue(string shellName) => List.FirstOrDefault(type => type?.Name == shellName, null);
    public static ShellType? FromValue(int shell)
        => List.FirstOrDefault(type => type?.ShellModelID == shell, null);
    public static ShellType? FromWeapon(IWeapon weapon) => weapon.WeaponToShell();

    public override int CompareTo(ShellType? other)
    {
        if (other == null)
            return 0;
        return 1;
    }

    public override bool Equals(ShellType? other)
    {
        if (other == null) return false;
        return Value == other.Value;
    }

    public static implicit operator ShellType(EShellType shellType) => FromName(Enum.GetName(shellType));
    public static implicit operator EShellType(ShellType shellType) => Enum.Parse<EShellType>(shellType.Name);
    public static implicit operator string(ShellType shellType) => shellType.Name;
    public static implicit operator ShellType(string shellName) => FromName(shellName);

    public void ResetShells(IUnreal unreal)
    {
        foreach (var shell in Value)
            shell.InitShellRedirections(unreal);
    }
    public Dictionary<string, string> PathPairs => GetPathPairs(); 
    internal Dictionary<string, string> GetPathPairs()
    {
        var basePaths = GetBasePaths();
        var shellPaths = GetShellPaths();
        Dictionary<string, string> pairs = [];
        for (int i = 0; i <= basePaths.Count; i++)
        {
            pairs[basePaths[i]] = shellPaths[i];
            continue;
        }
        return pairs;
    }
    public List<string> GetBasePaths()
    {
        List<string> strings = [];
        var shells = Value;
        foreach (var sh in shells)
        {
            strings.Add(sh.GetArmatureBasePath()!);
        }
        return strings;
    }
    public List<string> GetShellPaths()
    {
        List<string> strings = [];
        var shells = Value;
        foreach (var sh in shells)
        {
            strings.Add(sh.GetArmatureShellPath()!);
        }
        return strings;
    } 
}

public static class ShellTypes
{
    public static ShellType[] shells =
        [
            ShellType.Player,
            ShellType.Yukari,
            ShellType.Stupei,
            ShellType.Akihiko,
            ShellType.Mitsuru,
            ShellType.Aigis_SmallArms,
            ShellType.Aigis_LongArms,
            ShellType.Ken,
            ShellType.Koromaru,
            ShellType.Shinjiro,
            ShellType.Metis,
            ShellType.Aigis_SmallArms_Astrea,
            ShellType.Aigis_LongArms_Astrea,
        ];
    public static bool IsMultiModel(this IWeapon weapon) => weapon.ShellTarget != null && weapon.ShellTarget.IsMultiModel();
    public static bool IsMultiModel(this ShellType shellType) => MultiModelShells.Contains(shellType);
    private static ShellType[] MultiModelShells =
        [
            ShellType.Akihiko,
            ShellType.Aigis_SmallArms,
            ShellType.Aigis_SmallArms_Astrea,
        ];
}

public static class ShellGroups
{
    public static ShellType? WeaponToShell(this IWeapon weapon)
    {
        if (weapon.EpisodeFlag.HasFlag(EpisodeFlag.Astrea))
            return AstreaTableIds.GetShell(weapon.ModelId);
        else
            return VanillaTableIds.GetShell(weapon.ModelId);
    }
    
    public static IdCollection VanillaTableIds = new()
    {
        Player = [10, 11, 12, 13, 14, 15, 16, 17, 18, 19],
        Yukari = [20, 21, 22, 23, 24, 25, 26, 27, 28],
        Stupei = [30, 31, 32, 33, 34, 35, 36, 37, 38, 39],
        Akihiko = [40, 41, 42, 43, 44, 45, 46, 47, 48],
        Mitsuru = [50, 51, 52, 53, 54, 55, 56, 57],
        Aigis_SmallArms = [326, 327],
        Aigis_LongArms =             [584, 585, 586, 587, 588, 589],
        Ken = [80, 81, 82, 83, 84, 85, 86, 87, 88, 89],
        Koromaru = [90, 91, 92, 93, 94, 95, 96, 97],
        Shinjiro = [100, 101, 102, 103, 104, 105],
    };
    public static IdCollection AstreaTableIds = new()
    {
        AigisShell1 = ShellType.Aigis_SmallArms_Astrea,
        AigisShell2 = ShellType.Aigis_LongArms_Astrea,
        Player = [10, 11, 12, 13, 14, 15, 16, 17, 18, 19],
        Yukari = [20, 21, 22, 23, 24, 25, 26, 27, 28],
        Stupei = [30, 31, 32, 33, 34, 35, 36, 37, 38, 39],
        Akihiko = [40, 41, 42, 43, 44, 45, 46, 47, 48],
        Mitsuru = [50, 51, 52, 53, 54, 55, 56, 57],
        Aigis_SmallArms = [326, 327],
        Aigis_LongArms =             [584, 585, 586, 587, 588, 589],
        Ken = [80, 81, 82, 83, 84, 85, 86, 87, 88, 89],
        Koromaru = [90, 91, 92, 93, 94, 95, 96, 97],
        Metis = [100, 101, 102, 103, 104, 105, 106],
    };

    public class IdCollection
    {
        public ShellType AigisShell1 = ShellType.Aigis_SmallArms;
        public ShellType AigisShell2 = ShellType.Aigis_LongArms;
        public ShellType? GetShell(int modelId)
        {
            var match = Lists.Find(list => list.Contains(modelId));
            if (match == Player)
                return ShellType.Player;
            if (match == Yukari)
                return ShellType.Yukari;
            if (match == Stupei)
                return ShellType.Stupei;
            if (match == Akihiko)
                return ShellType.Akihiko;
            if (match == Mitsuru)
                return ShellType.Mitsuru;
            if (match == Aigis_SmallArms)
                return AigisShell1;
            if (match == Aigis_LongArms)
                return AigisShell2;
            if (match == Ken)
                return ShellType.Ken;
            if (match == Koromaru)
                return ShellType.Koromaru;
            if (match == Shinjiro)
                return ShellType.Shinjiro;
            if (match == Metis)
                return ShellType.Metis;
            return null;
        }

        private List<List<int>> AllLists => [Player,Yukari,Stupei,Akihiko,Mitsuru,Aigis_SmallArms,Aigis_LongArms,Ken,Koromaru,Shinjiro,Metis];
        public List<List<int>> Lists => AllLists.Where(list => list.Count > 0).ToList();
        public List<int> Player { get; set; } = [];
        public List<int> Yukari { get; set; } = [];
        public List<int> Stupei { get; set; } = [];
        public List<int> Akihiko { get; set; } = [];
        public List<int> Mitsuru { get; set; } = [];
        public List<int> Aigis_SmallArms { get; set; } = [];
        public List<int> Aigis_LongArms { get; set; } = [];
        public List<int> Ken { get; set; } = [];
        public List<int> Koromaru { get; set; } = [];
        public List<int> Shinjiro { get; set; } = [];
        public List<int> Metis {  get; set; } = [];
    }
    public readonly struct ModelIDTuple(int tableID, int blueprintID)
    {
        public int TableID { get; init; } = tableID;
        public int BlueprintID { get; init; } = blueprintID;
    }
}