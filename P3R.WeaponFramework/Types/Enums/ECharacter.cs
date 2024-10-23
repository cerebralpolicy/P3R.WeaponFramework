using P3R.WeaponFramework.Enums;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
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
    private int armbandId;
    private List<ShellType> shells;
    private bool vanilla;
    private bool astrea;
    private bool armed;
    private EEquipFlag equipId;
    private EWeaponType? weaponType;

    public Character(ECharacter enumValue, List<ShellType> shells, bool vanilla = true, bool astrea = true, bool armed = true, int armbandId = -1) : base(enumValue)
    {
        equipId = Enum.Parse<EEquipFlag>(enumValue.ToString());
        if (Enum.TryParse(enumValue.ToString(), out EWeaponType weaponType))
        {
            this.weaponType = weaponType;
        }
        else
            this.weaponType = null;
        this.armbandId = armbandId;
        this.shells = shells;
        this.vanilla = vanilla;
        this.astrea = astrea;
        this.armed = armed;
    }

    public Character(string name, ushort value, List<ShellType> shells, bool vanilla = true, bool astrea = true, bool armed = true, int armbandId = -1) : base(name, value)
    {
        equipId = Enum.Parse<EEquipFlag>(name);
        if (Enum.TryParse(name, out EWeaponType weaponType))
        {
            this.weaponType = weaponType;
        }
        else
            this.weaponType = null;
        this.armbandId = armbandId;
        this.shells = shells;
        this.vanilla = vanilla;
        this.astrea = astrea;
        this.armed = armed;
    }
    public int ArmbandId => armbandId;
    public List<ShellType> ShellTypes => shells;
    public List<Shell> Shells => shells
      .ConvertAll<Shell>(new(s => s.AsShell()));
    public bool IsAstrea => astrea;
    public bool IsVanilla => vanilla;
    public bool IsArmed => armed;
    public EEquipFlag EquipID => equipId;
    public EWeaponType WeaponType => (EWeaponType)weaponType!;
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
            new (ECharacter.Player, [ShellType.Player], astrea: false, armbandId: 3),
            new (ECharacter.Yukari, [ShellType.Yukari], armbandId: 4),
            new (ECharacter.Stupei, [ShellType.Stupei], armbandId: 5),
            new (ECharacter.Akihiko, [ShellType.Akihiko], armbandId: 1),
            new (ECharacter.Mitsuru, [ShellType.Mitsuru], armbandId: 0),
            new (ECharacter.Fuuka, [], armed: false, armbandId: 6),
            new (ECharacter.Aigis, [ShellType.Aigis_SmallArms, ShellType.Aigis_LongArms], armbandId: 7),
            new (ECharacter.Ken, [ShellType.Ken], armbandId: 9),
            new (ECharacter.Koromaru, [ShellType.Koromaru], armbandId: 8),
            new (ECharacter.Shinjiro, [ShellType.Shinjiro], astrea: false, armbandId: 2),
            new (ECharacter.Metis, [ShellType.Metis], vanilla: false),
        ];
    public static bool IsValidCharacter(this ECharacter character, bool isAstrea) => Lookup.Valid(isAstrea).Contains(character);
    #region Interactive
    public static EWeaponType ToWeaponType(this ECharacter character)
    {
        double val = (double)character;
        if (character == ECharacter.Fuuka)
            return EWeaponType.NONE;
        else
            if (character > ECharacter.Fuuka)
        {
            val = val - 1;
        }
        return (EWeaponType)Math.Pow(2, val);
    }
    public static EEquipFlag ToEquipID(this ECharacter character) => Enum.Parse<EEquipFlag>(character.ToString());
    public static ECharacter[] CharacterList()
    {
        List<ECharacter> results = [];
        for (int i = 1; i < 11; i++)
        {
            results.Add((ECharacter)i);
        }
        return results.ToArray();
    }
    public static void Summarize(this ECharacter character)
    {
        var equip = character.ToEquipID();
        var type = character.ToWeaponType();
        var sb = new StringBuilder();
        sb.AppendLine($"{character}");
        sb.AppendLine($"EquipID: {equip} ({(int)equip})");
        sb.AppendLine($"WeaponType: {type} ({(int)type})");
        var result = sb.ToString();
        Console.WriteLine(result);
    } 
    #endregion
    public static List<ECharacter> Armed => Lookup.Armed;
    public static List<ECharacter> Unarmed => Lookup.Unarmed;
    public static List<ECharacter> Astrea => Lookup.Astrea;
    public static List <ECharacter> Vanilla => Lookup.Vanilla;
}