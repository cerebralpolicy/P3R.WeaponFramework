using P3R.WeaponFramework.Enums;
using P3R.WeaponFramework.Weapons.Models;
using System.Reflection;
using System.Text.Json;
using Unreal.AtlusScript.Interfaces;

namespace P3R.WeaponFramework.Types;
[Flags]
public enum FEpisode
{
    None = 1 << 0,
    Vanilla = 1 << 1,
    Astrea = 1 << 2,
}

public class Episode : WFFlagWrapper<Episode, FEpisode>
{
    #region Constants
    /// <summary>
    /// Each episode's weapon item list has 512 entries, although not all are used setting the mod weapons to begin at 1024 makes the most sense.
    /// </summary>
    /// <remarks>
    /// <b>VALUES</b><br/>
    /// <b>Int</b>: <c>1024</c><br/>
    /// <b>Hex</b>: <c>400</c>
    /// </remarks>
    public const int BASE_MOD_WEAP_ID = 1024; //1025
    /// <summary>
    /// Assuming that the <see cref="Weapon.IsItemIdWeapon(int)">span of weapon item IDs</see> starts at <c>28672</c> and ends at <c>32767</c>, the capacity for weapon items must be <c>4096</c> or <c>0x1000</c>. Thus the slots for both episodes number at <c>8192</c> or <c>0x2000</c><br/>
    /// Given that we have already <see cref="BASE_MOD_WEAP_ID">allocated</see> the first <c>1024</c> item IDs to the original weapons, and modded weapons begin at <c>1024</c>, this leaves <c>7168</c> avaliable slots.<br/>
    /// <i>Note: Weapon folders for characters other than the protagonist, Shinjiro, and Metis will</i>
    /// </summary>
    /// <remarks>
    /// <b>VALUES</b><br/>
    /// <b>Int</b>: <c>7168</c><br/>
    /// <b>Hex</b>: <c>1C00</c>
    /// </remarks>
    public const int NUM_MOD_WEAPS = 7168; //3072
    /// <summary>
    /// Each episode's weapon item list has 512 entries, although not all are used setting the mod weapons to begin at 512 makes the most sense, assuming IDs are zero-indexed.
    /// </summary>
    /// <remarks>
    /// <b>VALUES</b><br/>
    /// <b>Int</b>: <c>512</c><br/>
    /// <b>Hex</b>: <c>200</c>
    /// </remarks>
    public const int BASE_EPISODE_WEAP_ID = 512; //1025
    /// <summary>
    /// Assuming that the <see cref="Weapon.IsItemIdWeapon(int)">span of weapon item IDs</see> starts at <c>28672</c> and ends at <c>32767</c>, the capacity for weapon items must be <c>4096</c> or <c>0x1000</c>.<br/>
    /// Given that we have already <see cref="BASE_EPISODE_WEAP_ID">allocated</see> the first <c>512</c> zero-indexed item IDs to the original weapons, and modded weapons begin at <c>512</c>, this leaves <c>3584</c> or <c>E00</c> avaliable slots.<br/>
    /// </summary>
    /// <remarks>
    /// <b>VALUES</b><br/>
    /// <b>Int</b>: <c>3584</c><br/>
    /// <b>Hex</b>: <c>E00</c>
    /// </remarks>
    public const int NUM_EPISODE_WEAPS = 3584; //3072
    #endregion

    private readonly List<Weapon> weapons;
    private readonly List<string> descriptions;

    public List<Weapon> Weapons => weapons;
    public List<string> Descriptions => descriptions;

    public Episode(FEpisode flagValue) : base(flagValue)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var name = flagValue.ToString();
        Log.Debug($"Initializing {name} episode resources.");
        var weapResPath = $"P3R.WeaponFramework.Resources.{name}.Weapons.json";
        var descResPath = $"P3R.WeaponFramework.Resources.{name}.Descriptions.msg";

        weapons = LoadWeapons(assembly, weapResPath, name);
        descriptions = LoadDescriptions(assembly, descResPath, name);
    }
    private List<Weapon> LoadWeapons(Assembly assembly, string path, string name)
    {
        Log.Debug($"Initializing {name} weapons.");

        using var stream = assembly.GetManifestResourceStream(path);
        if (stream == null)
            throw new NullReferenceException(nameof(stream));
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var gameWeapons = JsonSerializer.Deserialize<List<Weapon>>(json);
        if (gameWeapons == null || gameWeapons.Count == 0)
            throw new NullReferenceException($"No weapons found for {path}");
        Log.Debug($"Total weapon defs: {gameWeapons.Count} (Should be 511).");
        var weapons = gameWeapons;
        var isAstrea = name == "Astrea";
        foreach (var weapon in weapons)
            weapon.InitAtlusWeapon();
        for (int i = 0; i < NUM_EPISODE_WEAPS; i++)
        {
            var weaponId = BASE_EPISODE_WEAP_ID + i;
            var weapon = new Weapon(weaponId)
            {
                Character = ECharacter.NONE,
                IsVanilla = !isAstrea,
                IsAstrea = isAstrea,
                IsModded = true,
                WeaponId = weaponId,
                ModelId = 8,
                ShellTarget = ShellType.Unassigned,
            };
            weapons.Add(weapon);
        }
        return weapons;
    }
    private List<string> LoadDescriptions(Assembly assembly, string path, string name)
    {
        Log.Debug($"Initializing {name} descriptions.");
        List<string> entries = new List<string>();
        using var stream = assembly.GetManifestResourceStream(path);
        if (stream == null)
            throw new NullReferenceException(nameof(stream));
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line?.StartsWith("[uf") == true)
            {
                if (!line.EndsWith("[n][e]"))
                {
                    line = $"{line}[n][e]";
                }
                
                entries.Add(line);
            }
        }

        // Add placeholder entries.
        for (int i = 0; i < 100; i++)
        {
            entries.Add("[uf 0 5 65278][uf 2 1]未使用[n][e]");
        }
        return entries;
    }
}