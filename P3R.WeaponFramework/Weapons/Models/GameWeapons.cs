using P3R.WeaponFramework.Hooks;
using P3R.WeaponFramework.Types;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace P3R.WeaponFramework.Weapons.Models;

internal unsafe class GameWeapons: IReadOnlyCollection<Weapon>
{
    #region Constants
    /// <summary>
    /// Each episode's weapon item list has 512 entries, although not all are used setting the mod weapons to begin at 1025 makes the most sense. 
    /// </summary>
    public const int BASE_MOD_WEAP_ID   = 1025; //1025
    private const int NUM_MOD_WEAPS     = 10000;
    #endregion
    public readonly List<Weapon> weapons = [];
    #region EpisodeHook
    private delegate bool IsAstreaSave();
    private IsAstreaSave? isAstreaSave;
    public EpisodeFlag Filter
    {
        get
        {
            if (isAstreaSave == null)
            {
                return EpisodeFlag.None;
            }
            else
            {
                return isAstreaSave() ? EpisodeFlag.Astrea : EpisodeFlag.Vanilla;
            }
        }
    }
    #endregion
    public GameWeapons()
    {
        ScanHooks.Add(
            "WF_IsAstreaSave",
                "48 83 EC 28 E8 ?? ?? ?? ?? 48 85 C0 74 ?? E8 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? 3C 01 0F 94 C0 48 83 C4 28 C3 48 83 C4 28 C3",
                (hooks, result) => this.isAstreaSave = hooks.CreateWrapper<IsAstreaSave>(result, out _));
        
        ProcessResource(EpisodeFlag.Vanilla);
        ProcessResource(EpisodeFlag.Astrea);
        AddModSlots();
    }

    public List<Weapon> FilteredWeapons => weapons.Where(x => x.EpisodeFlag.HasFlag(Filter)).ToList();

    public int Count => FilteredWeapons.Count;


    internal void ProcessResource(EpisodeFlag episode)
    {
        if (episode.WeaponResource == null)
        {
            Log.Error("Invalid episode specified");
            return;
        }   
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(episode.WeaponResource)!;
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var gameWeapons = JsonSerializer.Deserialize<Dictionary<ECharacter, Weapon[]>>(json)!;
        foreach (var charWeapons in gameWeapons)
        {
            foreach (var weapon in charWeapons.Value)
            {
                weapon.EpisodeFlag = episode;
                weapon.ShellTarget = ShellType.FromWeapon(weapon)!;
            }
            weapons.AddRange(charWeapons.Value);
        }
    }
    public void AddModSlots()
    {
        for (int i = 0; i < NUM_MOD_WEAPS; i++)
        {
            var weaponId = BASE_MOD_WEAP_ID + i;
            var weapon = new Weapon(weaponId)
            {
                EpisodeFlag = EpisodeFlag.None,
                WeaponId = weaponId,
                ModelId = 10,
            };
            Log.Debug($"New slot {weaponId}");
            weapons.Add(weapon);
        }
    }

    public bool TryGetWeaponByItemId(int itemId, [NotNullWhen(true)] out Weapon? weapon)
    {
        weapon = FilteredWeapons.FirstOrDefault(x => x.WeaponItemId == itemId);
        return weapon != null;
    }

    public IEnumerator<Weapon> GetEnumerator() => FilteredWeapons.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => FilteredWeapons.GetEnumerator();
}
