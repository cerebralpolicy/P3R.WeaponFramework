
using Ardalis.SmartEnum;

namespace P3R.WeaponFramework.Interfaces.Types;
[SmartEnumName(typeof(Character))]
public class Character(string name, WFEnumCollection<ShellType, Armature> shells, ushort value) : WFEnum<Character, ushort>(name, value)
{
    public static Character NONE { get; }
       = new(nameof(NONE), new(), 0);
    public static Character Player { get; } 
        = new(nameof(Player), new([ShellType.Player]), 1);
    public static Character Yukari { get; } 
        = new(nameof(Yukari), new([ShellType.Yukari]), 2);
    public static Character Stupei { get; } 
        = new(nameof(Stupei), new([ShellType.Stupei]), 3);
    public static Character Akihiko { get; } 
        = new(nameof(Akihiko), new([ShellType.Akihiko]), 4);
    public static Character Mitsuru { get; }
        = new(nameof(Mitsuru), new([ShellType.Mitsuru]), 5);
    public static Character Aigis { get; }
        = new(nameof(Aigis), new([ShellType.Aigis_SmallArms, ShellType.Aigis_LongArms]), 7);
    public static Character Ken {  get; }
        = new(nameof(Ken), new([ShellType.Ken]), 8);
    public static Character Koromaru { get; }
        = new(nameof(Koromaru), new([ShellType.Koromaru]), 9);
    public static Character Shnijiro { get; } 
        = new(nameof(Shnijiro), new([ShellType.Shinjiro]), 10);
    public static Character Metis { get; } 
        = new(nameof(Metis), new([ShellType.Metis]), 11);
    public static Character Aigis12 { get; } 
        = new(nameof(Aigis12), [ShellType.Aigis_SmallArms_Astrea, ShellType.Aigis_LongArms_Astrea], 12);

    public WFEnumCollection<ShellType, Armature> Shells { get; init; } = shells;

    public static implicit operator ECharacter(Character character) => Enum.Parse<ECharacter>(character.Name);
    public static implicit operator Character(ECharacter character) => FromValue((ushort)character);
    public static explicit operator int(Character character) => (int)character;

    public bool IsAigis() => this == Aigis || this == Aigis12;
    public int GetShellID(bool isAigisLongArms = false)
    {
        if (isAigisLongArms && IsAigis())
            return Shells[1].ShellModelID;
        return Shells[0].ShellModelID;
    }
    public List<string> GetShellPaths(bool isAigisLongArms = false)
    {
        if (isAigisLongArms && IsAigis())
            return Shells[1].GetShellPaths();
        return Shells[0].GetShellPaths();
    }

    public override int CompareTo(Character? other)
    {
        if (other == null)
            return 0;
        return 1;
    }

    public override bool Equals(Character? other)
    {
        if (other == null) return false;
        return Enum.Parse<ECharacter>(other.Name) == Enum.Parse<ECharacter>(this.Name);
    }
}
public static class Characters
{
    public static readonly ECharacter[] Armed =
    [
        ECharacter.Player,
        ECharacter.Yukari,
        ECharacter.Stupei,
        ECharacter.Akihiko,
        ECharacter.Mitsuru,
        ECharacter.Aigis,
        ECharacter.Ken,
        ECharacter.Koromaru,
        ECharacter.Shinjiro,
        ECharacter.Metis,
        ECharacter.Aigis12,
    ];
    public static readonly Character[] WFArmed =
    [
        Character.Player,
        Character.Yukari,
        Character.Stupei,
        Character.Akihiko,
        Character.Mitsuru,
        Character.Aigis,
        Character.Ken,
        Character.Koromaru,
        Character.Shnijiro,
        Character.Metis,
        Character.Aigis12,
    ];
    public static readonly Character[] VanillaOnly =
    [
        Character.Player,
        Character.Aigis,
        Character.Shnijiro,
    ];

    public static readonly Character[] AstreaOnly =
    [
        Character.Metis,
        Character.Aigis12,
    ];
    public static Character CharacterFromShell(this ShellType shellType)
     => (EShellType)shellType switch
     {
         EShellType.Player => Character.Player,
         EShellType.Yukari => Character.Yukari,
         EShellType.Stupei => Character.Stupei,
         EShellType.Akihiko => Character.Akihiko,
         EShellType.Mitsuru => Character.Mitsuru,
         EShellType.Aigis_SmallArms => Character.Aigis,
         EShellType.Aigis_LongArms => Character.Aigis,
         EShellType.Ken => Character.Ken,
         EShellType.Koromaru => Character.Koromaru,
         EShellType.Shinjiro => Character.Shnijiro,
         EShellType.Metis => Character.Metis,
         EShellType.Aigis_SmallArms_Astrea => Character.Aigis12,
         EShellType.Aigis_LongArms_Astrea => Character.Aigis12,
         _ => throw new NotImplementedException(),
     };
}
