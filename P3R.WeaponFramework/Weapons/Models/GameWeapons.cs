using P3R.WeaponFramework.Core;
using P3R.WeaponFramework.Hooks;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

namespace P3R.WeaponFramework.Weapons.Models;

internal unsafe class GameWeapons : IReadOnlyCollection<Weapon>
{
    public delegate bool AstreaSaveCheck();
    public AstreaSaveCheck? IsAstreaSave;
    public bool AstreaSave
    { 
        get
        {
            var save = IsAstreaSave;
            if (save == null)
                return false;
            else
                return save.Invoke();
        }
    }
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
    public readonly List<Weapon> weaponsMerged = [];
    public readonly List<Weapon> weaponsVanilla = [];
    public readonly List<Weapon> weaponsAstrea = [];
    public List<Weapon> WeaponFilter() => weaponsMerged.Where(MatchesFilter).ToList();
    private bool MatchesFilter (Weapon weapon)
    {
        var save = IsAstreaSave;
        if (save == null)
            return weapon.IsVanilla;
        var saveCheck = save.Invoke();
        if (saveCheck)
            return weapon.IsAstrea;
        else
            return weapon.IsVanilla;
    }
    public GameWeapons()
    {
        ProcessEpisode(false);
        ProcessEpisode(true);
        AddModSlots(false);
        AddModSlots(true);
        ScanHooks.Listen("WF_IsAstreaSave", (hooks, result) => this.IsAstreaSave = hooks.CreateWrapper<AstreaSaveCheck>(result, out _));
    }
    /// <summary>
    /// 
    /// </summary>
    public List<Weapon> Weapons
    {
        get
        {
            if (AstreaSave)
            {
                return weaponsAstrea;
            }
            else
            {
                return weaponsVanilla;
            }
        }
    }

    public int Count => Weapons.Count;

    internal void ProcessMerged()
    {
        var assembly = Assembly.GetExecutingAssembly();
        string resource = "P3R.WeaponFramework.Resources.Weapons.json";
        using var stream = assembly.GetManifestResourceStream(resource)!;
        if (stream == null)
            return;
        using var reader = new StreamReader(stream!);
        var json = reader.ReadToEnd();
        var gameWeapons = JsonSerializer.Deserialize<Dictionary<Character, Weapon[]>>(json)!;
        foreach (var charWeapons in gameWeapons)
        {
            weaponsMerged.AddRange(charWeapons.Value);
        }
    }
    internal void ProcessEpisode(bool isAstrea = false)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string resource = isAstrea ? "P3R.WeaponFramework.Resources.Astrea.Weapons.json" : "P3R.WeaponFramework.Resources.Vanilla.Weapons.json";
        using var stream = assembly.GetManifestResourceStream(resource)!;
        if (stream == null)
            return;
        using var reader = new StreamReader(stream!);
        var json = reader.ReadToEnd();
        var episode = isAstrea ? "Astrea" : "Vanilla";
        Log.Information($"Processing {episode} weapons.");
        var gameWeapons = JsonSerializer.Deserialize<Dictionary<Character, Weapon[]>>(json)!;
        foreach (var charWeapons in gameWeapons)
        {
            if (isAstrea)
            {
                weaponsAstrea.AddRange(charWeapons.Value);
            }
            else
            {
                weaponsVanilla.AddRange(charWeapons.Value);
            }
        }
        if (isAstrea)
        {
            foreach (var weapon in weaponsAstrea)
            {
                if (weapon.Name != "unused" && weapon.Character.IsValidCharacter(isAstrea))
                {
                    Log.Verbose($"Activating weapon. || Character: {weapon.Character} || Weapon: {weapon.Name} || SortNum: {weapon.SortNum}");
                    weapon.IsEnabled = true;
                }    
            }
        }
        else
        {
            foreach (var weapon in weaponsVanilla)
            {
                if (weapon.Name != "Unused" && weapon.Character.IsValidCharacter(isAstrea))
                {
                    Log.Verbose($"Activating weapon. || Character: {weapon.Character} || Weapon: {weapon.Name} || SortNum: {weapon.SortNum}");
                    weapon.IsEnabled = true;
                }
            }
        }
    }
    public void AddModSlotsMerged(bool isAstrea = false)
    {
        for (int i = 0; i < NUM_MOD_WEAPS; i++)
        {
            var weaponId = BASE_MOD_WEAP_ID + i;
            var weapon = new Weapon(weaponId)
            {
                IsVanilla = !isAstrea,
                IsAstrea = isAstrea,
                WeaponId = weaponId,
                ModelId = 10,
            };
            var episode = isAstrea ? "Astrea" : "Vanilla";
            Log.Verbose($"New {episode} slot {weaponId}");
            weaponsMerged.Add(weapon);
        }
    }
    public void AddModSlots(bool isAstrea = false)
    {
        for (int i = 0; i < NUM_EPISODE_WEAPS; i++)
        {
            var weaponId = BASE_EPISODE_WEAP_ID + i;
            var weapon = new Weapon(weaponId)
            {
                IsVanilla = !isAstrea,
                IsAstrea = isAstrea,
                WeaponId = weaponId,
                ModelId = 10,
            };
            var episode = isAstrea ? "Astrea" : "Vanilla";
            Log.Verbose($"New {episode} slot {weaponId}");
            if (isAstrea)
                weaponsAstrea.Add(weapon);
            else
                weaponsVanilla.Add(weapon);
        }
    }

    public bool TryGetWeaponByItemId(int itemId, [NotNullWhen(true)] out Weapon? weapon)
    {
        weapon = Weapons.FirstOrDefault(x => x.WeaponItemId == itemId);
        return weapon != null;
    }

    public IEnumerator<Weapon> GetEnumerator() => Weapons.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Weapons.GetEnumerator();
}
