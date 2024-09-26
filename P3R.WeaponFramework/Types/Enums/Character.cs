using P3R.WeaponFramework.Enums;
using System.Collections;

namespace P3R.WeaponFramework.Types;

public enum Character : ushort
{
    NONE,
    Player,
    Yukari,
    Stupei,
    Akihiko,
    Mitsuru,
    Fuuka,
    Aigis,
    Ken,
    Koromaru,
    Shinjiro,
    Metis,
    AigisReal,
}

public class CharacterWrapper : WFEnumWrapper<CharacterWrapper, ushort, Character>
{
    public static CharacterWrapper NONE{ get; } = new (0);
    public static CharacterWrapper Player{ get; } = new(Character.Player, [ShellTypeWrapper.Player]);
    public static CharacterWrapper Yukari{ get; } = new(Character.Yukari, [ShellTypeWrapper.Yukari]);
    public static CharacterWrapper Stupei{ get; } = new(Character.Stupei, [ShellTypeWrapper.Stupei]);
    public static CharacterWrapper Akihiko{ get; } = new(Character.Akihiko, [ShellTypeWrapper.Akihiko]);
    public static CharacterWrapper Mitsuru{ get; } = new(Character.Mitsuru, [ShellTypeWrapper.Mitsuru]);
    public static CharacterWrapper Fuuka{ get; } = new(Character.Fuuka);
    public static CharacterWrapper Aigis{ get; } = new(Character.Aigis, [ShellTypeWrapper.Aigis_SmallArms, ShellTypeWrapper.Aigis_LongArms]);
    public static CharacterWrapper Ken{ get; } = new(Character.Ken, [ShellTypeWrapper.Ken]);
    public static CharacterWrapper Koromaru{ get; } = new(Character.Koromaru, [ShellTypeWrapper.Koromaru]);
    public static CharacterWrapper Shinjiro{ get; } = new(Character.Shinjiro, [ShellTypeWrapper.Shinjiro]);
    public static CharacterWrapper Metis{ get; } = new(Character.Metis, [ShellTypeWrapper.Metis]);
    public static CharacterWrapper AigisReal{ get; } = new(Character.AigisReal, [ShellTypeWrapper.Aigis_SmallArms, ShellTypeWrapper.Aigis_LongArms]);

    public List<ShellTypeWrapper> Shells = [];
    
    public bool IsArmed => Shells.Count > 0;
    public static List<CharacterWrapper> Armed = List.Where(x => x.IsArmed).ToList();
    public CharacterWrapper(Character enumValue, List<ShellTypeWrapper> enumValues) : base(enumValue)
    {
        Shells = enumValues;
    }
    public CharacterWrapper(Character enumValue) : base(enumValue)
    {
    }

    public static implicit operator CharacterWrapper(Character value) => FromName(value.ToString());
    public static implicit operator Character(CharacterWrapper wrapper) => Enum.Parse<Character>(wrapper.Name);
}

public static class Characters
{
    public static List<CharacterWrapper> WFArmed { get; } = CharacterWrapper.Armed;
    public static List<CharacterWrapper> VanillaOnly { get; } = [CharacterWrapper.Player, CharacterWrapper.Shinjiro];
    public static List<CharacterWrapper> AstreaOnly { get; } = [CharacterWrapper.Metis];
    public static bool IsValidCharacter(this Character character, bool isAstrea) => !Unarmed.Contains(character) && (isAstrea ? !AstreaAbsent.Contains(character) : !VanillaAbsent.Contains(character));
    public static List<Character> Unarmed { get; } = [Character.Fuuka];
    public static List<Character> AstreaAbsent { get; } = [Character.Player, Character.Shinjiro];
    public static List <Character> VanillaAbsent { get; } = [Character.Metis, Character.AigisReal];
}