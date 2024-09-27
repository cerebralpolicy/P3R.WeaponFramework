using P3R.WeaponFramework.Enums;
using System.Collections;
using System.Collections.ObjectModel;

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


public struct CharaDef
{
    private Character key;
    private List<ShellType> value;
    private bool vanilla;
    private bool astrea;
    private bool armed;

    public CharaDef(Character key, List<ShellType> value, bool vanilla = true, bool astrea = true, bool armed = true)
    {
        this.key = key;
        this.value = value;
        this.vanilla = vanilla;
        this.astrea = astrea;
        this.armed = armed;
    }

    public Character Key => key;
    public List<ShellType> Value => value;
    public readonly bool IsAstrea => astrea;
    public readonly bool IsVanilla => vanilla;
    public readonly bool IsArmed => armed;
}
public class CharDB : KeyedCollection<Character, CharaDef>
{
    public Character this[ShellType shellType] => this.First(x => x.Value.Contains(shellType)).Key;
    public List<Character> Armed => this.Where(x => x.IsArmed).Select(x => x.Key).ToList();
    public List<Character> Unarmed => this.Where(x => !x.IsArmed).Select(x => x.Key).ToList();
    public List<Character> Vanilla => this.Where(x => x.IsVanilla).Select(x => x.Key).ToList();
    public List<Character> Astrea => this.Where(x => x.IsAstrea).Select(x => x.Key).ToList();
    protected override Character GetKeyForItem(CharaDef item) => Items.First(x => x.Equals(item)).Key;
}
public static class Characters
{
    public static CharDB Lookup { get; } =
        [
            new (Character.Player, [ShellType.Player], astrea: false),
            new (Character.Yukari, [ShellType.Yukari]),
            new (Character.Stupei, [ShellType.Stupei]),
            new (Character.Akihiko, [ShellType.Akihiko]),
            new (Character.Mitsuru, [ShellType.Mitsuru]),
            new (Character.Fuuka, [], armed: false),
            new (Character.Aigis, [ShellType.Aigis_SmallArms, ShellType.Aigis_LongArms]),
            new (Character.Ken, [ShellType.Ken]),
            new (Character.Koromaru, [ShellType.Koromaru]),
            new (Character.Shinjiro, [ShellType.Shinjiro], astrea: false),
            new (Character.Metis, [ShellType.Metis], vanilla: false),
        ];
    public static bool IsValidCharacter(this Character character, bool isAstrea) => !Unarmed.Contains(character) && (isAstrea ? Astrea.Contains(character) : Vanilla.Contains(character));
    public static List<Character> Armed => Lookup.Armed;
    public static List<Character> Unarmed => Lookup.Unarmed;
    public static List<Character> Astrea => Lookup.Astrea;
    public static List <Character> Vanilla => Lookup.Vanilla;
}