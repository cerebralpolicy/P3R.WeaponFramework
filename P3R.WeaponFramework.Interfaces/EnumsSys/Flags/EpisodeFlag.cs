using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace P3R.WeaponFramework.Interfaces.Types;
/// <summary>
/// A bitwise value used to denote which episode a given <see cref="IWeapon">Weapon</see> belongs to.<br/>
/// This is need because unlike with costumes, the weapon tables do not line up.<br/>
/// <b>Values:</b>
/// <list type="bullet">
/// <item><term><see cref="None"/>  [0b0000]</term> <description>Default value used to initialize the weapon slots.</description></item>
/// <item><term><see cref="Vanilla"/>   [0b0001]</term> <description>Weapons that are used in the main game.</description></item>
/// <item><term><see cref="Astrea"/>    [0b0010]</term><description>Weapons used by Episode Aigis.</description></item>
/// <item><term><see cref="Vanilla"/>   [0b0011]</term><description>Used to create modded weapons for characters that appear in both episodes.</description></item>
/// </list>
/// </summary>
[DefaultProperty("None")]
public class EpisodeFlag : WFFlag<EpisodeFlag, int>
{
    /// <summary>
    /// Default value used to initialize the weapon slots.
    /// </summary>
    public static EpisodeFlag None = new(nameof(None), 1 << 0);
    /// <summary>
    /// Weapons that are used in the main game.
    /// </summary>
    public static EpisodeFlag Vanilla = new(nameof(Vanilla), 1 << 1);
    /// <summary>
    /// Weapons used by Episode Aigis.
    /// </summary>
    public static EpisodeFlag Astrea = new(nameof(Astrea), 1 << 2);
    /// <summary>
    /// Used to create modded weapons for characters that appear in both episodes.
    /// </summary>
    public static EpisodeFlag Both = new(nameof(Both), Vanilla | Astrea);
    public EpisodeFlag(string name, int value) : base(name, value)
    {
        if (name == "None" || name == "Both")
            return;
        WeaponResource = WeapResource(name);
        DescriptionResource = DescResource(name);
    }

    public string? WeaponResource;
    public string? DescriptionResource;
    public bool HasFlag(EpisodeFlag otherFlag)
    {
        var thisFlagDumb = (EEpisodeFlag)this;
        return thisFlagDumb.HasFlag(otherFlag);
    }
    private string WeapResource(string name) => $"P3R.WeaponFramework.Resources.Weapons{(name == "Astrea" ? $"_{name}" : null)}.json";
    private string DescResource(string name) => $"P3R.WeaponFramework.Resources.EN.Descriptions{(name == "Astrea" ? $"_{name}" : null)}.msg";

    public static implicit operator EEpisodeFlag(EpisodeFlag flag) => (EEpisodeFlag)flag.Value;

    public override int CompareTo(EpisodeFlag? other)
    {
        if (other == null)
        {
            return 0;
        }
        return other.Value + Value;
    }

    public override bool Equals(EpisodeFlag? other)
    {
        if (other == null)
        { return false; }
        return Value == other.Value;
    }
}

public static class EpisodeFlags
{
   
}