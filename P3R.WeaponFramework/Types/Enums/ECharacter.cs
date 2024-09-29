using P3R.WeaponFramework.Enums;
using System.Collections;
using System.Collections.ObjectModel;
using YamlDotNet.Core.Tokens;

namespace P3R.WeaponFramework.Types;

public enum ECharacter : ushort
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
public class Character : WFEnumWrapper<Character,ushort,ECharacter>
{
    private List<ShellType> shells;
    private bool vanilla;
    private bool astrea;
    private bool armed;

    public Character(ECharacter enumValue, List<ShellType> shells, bool vanilla = true, bool astrea = true, bool armed = true) : base(enumValue)
    {
        this.shells = shells;
        this.vanilla = vanilla;
        this.astrea = astrea;
        this.armed = armed;
    }

    public Character(string name, ushort value, List<ShellType> shells, bool vanilla = true, bool astrea = true, bool armed = true) : base(name, value)
    {
        this.shells = shells;
        this.vanilla = vanilla;
        this.astrea = astrea;
        this.armed = armed;
    }
    public List<ShellType> ShellTypes => shells;
    public List<Shell> Shells => shells
      .ConvertAll<Shell>(new(s => s.AsShell()));
    public bool IsAstrea => astrea;
    public bool IsVanilla => vanilla;
    public bool IsArmed => armed;

}

public class CharacterDB : WFEnumCollection<Character, ushort, ECharacter>
{
    protected override ECharacter GetKeyForItem(Character item) => item.EnumValue;

    public ECharacter FirstOfList(List<ECharacter> list) => list.First();
    public IList<Character> ValidItems(bool astrea) => Items
        .Where(x => x.IsArmed && astrea ? x.IsAstrea : x.IsVanilla).ToList();
    public List<ECharacter> HasShell(ShellType shell, bool astrea) => ValidItems(astrea)
        .Where(x => x.ShellTypes.Contains(shell))
        .Select(chara => chara.EnumValue)
        .ToList();
    public List<ECharacter> HasShell(ShellType shell) => Items
        .Where(x => x.ShellTypes.Contains(shell))
        .Select(chara => chara.EnumValue)
        .ToList();
    public List<ECharacter> Armed => Items
        .Where(x => x.IsArmed)
        .Select(chara => chara.EnumValue)
        .ToList();
    public List<ECharacter> Unarmed => Items
        .Select(chara => chara.EnumValue)
        .Except(Armed)
        .ToList();
    public List<ECharacter> Astrea => Items
        .Where (x => x.IsAstrea)
        .Select (chara => chara.EnumValue)
        .ToList();
    public List<ECharacter> Vanilla => Items
        .Where(x => x.IsVanilla)
        .Select(chara => chara.EnumValue)
        .ToList();
    public List<ECharacter> Valid(bool astrea = false) => ValidItems(astrea)
        .Select(chara => chara.EnumValue)
        .ToList();
}
public static class Characters
{
    public static CharacterDB Lookup => [
            new (ECharacter.NONE, [ShellType.None], false, false, false),
            new (ECharacter.Player, [ShellType.Player], astrea: false),
            new (ECharacter.Yukari, [ShellType.Yukari]),
            new (ECharacter.Stupei, [ShellType.Stupei]),
            new (ECharacter.Akihiko, [ShellType.Akihiko]),
            new (ECharacter.Mitsuru, [ShellType.Mitsuru]),
            new (ECharacter.Fuuka, [], armed: false),
            new (ECharacter.Aigis, [ShellType.Aigis_SmallArms, ShellType.Aigis_LongArms]),
            new (ECharacter.Ken, [ShellType.Ken]),
            new (ECharacter.Koromaru, [ShellType.Koromaru]),
            new (ECharacter.Shinjiro, [ShellType.Shinjiro], astrea: false),
            new (ECharacter.Metis, [ShellType.Metis], vanilla: false),
        ];
    public static bool IsValidCharacter(this ECharacter character, bool isAstrea) => Lookup.Valid(isAstrea).Contains(character);
    public static List<ECharacter> Armed => Lookup.Armed;
    public static List<ECharacter> Unarmed => Lookup.Unarmed;
    public static List<ECharacter> Astrea => Lookup.Astrea;
    public static List <ECharacter> Vanilla => Lookup.Vanilla;
}