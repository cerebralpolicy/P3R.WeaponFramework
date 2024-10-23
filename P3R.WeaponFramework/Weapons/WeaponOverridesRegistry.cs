using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Weapons.Models;
using Reloaded.Memory.Extensions;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace P3R.WeaponFramework.Weapons;


internal class OverrideCollection : KeyedCollection<int, WeaponOverride>
{
    protected override int GetKeyForItem(WeaponOverride item) => item.OriginalWeaponId;
}



public class WeaponOverridesRegistry(WeaponRegistry weapons) : IWeaponApi
{
    private readonly WeaponRegistry weapons = weapons;
    private readonly OverrideCollection weaponsOverrides = [];
    private readonly List<int> overridenIds = [];
    private readonly List<string> overrideNames = [];
    public Dictionary<int,string> OverridenWeapons { get; private set; } = [];
    private void AddNewOverride(ECharacter character, FEpisode episode, int originalWeaponId, string newWeaponName)
    {
        overridenIds.Add(originalWeaponId);
        overrideNames.Add(newWeaponName);
        weaponsOverrides.Add(new()
        {
            Character = character,
            Episode = episode,
            OriginalWeaponId = originalWeaponId,
            NewWeaponName = newWeaponName
        });

        Log.Information($"Weapon override: {character} || WeaponId: {originalWeaponId} || New: {newWeaponName}");
    }
    private void ReplaceOverride(int originalWeaponId, string newWeaponName)
    {
        var index = overridenIds.IndexOf(originalWeaponId);
        var prevOverride = weaponsOverrides[index];
        var character = prevOverride.Character;
        var oldWeaponName = prevOverride.NewWeaponName;
        Log.Information($"Replaced override: {character}|| WeaponId: {originalWeaponId} || New: {newWeaponName} || Was: {oldWeaponName}");
        prevOverride.NewWeaponName = newWeaponName;
    }

    public bool TryGetWeaponOverridesTo(Weapon weapon, [NotNullWhen(true)] out List<Weapon>? overridenWeapons)
    {
        overridenWeapons = [];
        var chara = weapon.Character;
        var name = weapon.Name;
        if (name == null)
        {
            overridenWeapons = null;
            return false;
        }
        var overridesWithName = weaponsOverrides.Where(x => x.Character == chara && x.NewWeaponName == name).ToList();
        if (overridesWithName.Count == 0 || overridesWithName == null)
        {
            overridenWeapons = null;
            return false;
        }
        var ids = overridesWithName.Select(x => x.OriginalWeaponId).ToList();
        foreach ( var id in ids )
        {
            if (weapons.Weapons.TryGetWeaponByItemId(id, out weapon) == true)
            {
                overridenWeapons.Add(weapon);
            }
        }
        return true;
    }
    public bool TryGetWeaponOverrideFor(Weapon weapon, [NotNullWhen(true)] out Weapon? newWeapon)
    {
        newWeapon = null;
        var weaponOverride = weaponsOverrides.FirstOrDefault(x => x.Character == weapon.Character && x.OriginalWeaponId == weapon.WeaponItemId);
        if (weaponOverride == null)
            return false;
        newWeapon = weapons.Weapons.FirstOrDefault(x => x.Character == weaponOverride.Character && x.Name?.Equals(weaponOverride.NewWeaponName, StringComparison.OrdinalIgnoreCase) == true);
        if (newWeapon == null)
        {
            return false;
        }
        return newWeapon != null;
    }
    public bool TryGetWeaponOverrideFrom(ECharacter character, int originalWeaponId, [NotNullWhen(true)] out Weapon? newWeapon)
    {
        var weaponOverride = weaponsOverrides.FirstOrDefault(x => x.Character == character && x.OriginalWeaponId == originalWeaponId);
        if (weaponOverride == null)
        {
            newWeapon = null;
            return false;
        }

        newWeapon = weapons.Weapons.FirstOrDefault(x => x.Character == weaponOverride.Character && x.Name?.Equals(weaponOverride.NewWeaponName, StringComparison.OrdinalIgnoreCase) == true);
        if (newWeapon == null)
        {
            return false;
        }

        return newWeapon != null;
    }
    public void AddOverridesFile(string file)
    {
        try
        {
            var overrides = Utils.YamlSerializer.DeserializeFile<WeaponOverrideSerialized[]>(file);
            foreach (var weaponOverride in overrides)
            {
                var chara = Enum.Parse<ECharacter>(weaponOverride.Character, true);
                var episode = EpFlag.Parse(weaponOverride.Episode, true);
                // If
                if (episode.HasFlagFast(FEpisode.Astrea))
                {
                    // Parse weapon ItemID as int
                    if (!int.TryParse(weaponOverride.OriginalWeaponId, out var weaponItemId))
                    {
                        // Else search for weapon by name
                        // Episode Aigis features duplicate weapons so this will allow an override to replace multiple instances with the same name
                        var existingWeapons = 
                            // Try by name
                            this.weapons.Weapons.Where(x => x.Name?.Equals(weaponOverride.OriginalWeaponId, StringComparison.OrdinalIgnoreCase) == true).ToList() ??
                            // Try by itemDef
                            this.weapons.Weapons.Where(x => x.ItemDef?.Equals(weaponOverride.OriginalWeaponId, StringComparison.OrdinalIgnoreCase) == true).ToList() ??
                            throw new Exception();
                        // If there's more than one weapon that matches that name, add all to overrides
                        if (existingWeapons.Count > 1)
                        {
                            foreach (var weap in existingWeapons)
                            {
                                var thisId = weap.WeaponItemId;
                                if (overridenIds.Contains(thisId))
                                {
                                    ReplaceOverride(thisId, weaponOverride.NewWeaponName);
                                }
                                else
                                {
                                    AddNewOverride(chara, episode, thisId, weaponOverride.NewWeaponName);
                                }
                            }
                        }
                        // Else if only one weapon is found add that.
                        else if (existingWeapons.Count > 0)
                        {
                            var thisId = existingWeapons.First().WeaponItemId;
                            if (overridenIds.Contains(thisId))
                            {
                                ReplaceOverride(thisId, weaponOverride.NewWeaponName);
                            }
                            else
                            {
                                AddNewOverride(chara, episode, thisId, weaponOverride.NewWeaponName);
                            }
                        }
                    }
                    // If the override specifies a weaponItemId there will only be one override anyway
                    // No need to replace multiple. 
                    else
                    {
                        var thisId = weaponItemId;
                        if (overridenIds.Contains(thisId))
                        {
                            ReplaceOverride(thisId, weaponOverride.NewWeaponName);
                        }
                        else
                        {
                            AddNewOverride(chara, episode, thisId, weaponOverride.NewWeaponName);
                        }
                    }
                }
                else if (episode == FEpisode.Vanilla)
                {
                    // Parse weapon ItemID as int
                    if (!int.TryParse(weaponOverride.OriginalWeaponId, out var weaponItemId))
                    {
                        // Else search for weapon by string
                        var existingWeapon = 
                            // Try weapon name
                            this.weapons.Weapons.FirstOrDefault(x => x.Name?.Equals(weaponOverride.OriginalWeaponId, StringComparison.OrdinalIgnoreCase) == true) ??
                            // Try weapon itemDef
                            this.weapons.Weapons.FirstOrDefault(x => x.ItemDef?.Equals(weaponOverride.OriginalWeaponId, StringComparison.OrdinalIgnoreCase) == true) ??
                            throw new Exception();
                        weaponItemId = existingWeapon.WeaponItemId;
                    }
                    var thisId = weaponItemId;
                    if (overridenIds.Contains(thisId))
                    {
                        ReplaceOverride(thisId, weaponOverride.NewWeaponName);
                    }
                    else
                    {
                        AddNewOverride(chara, episode, thisId, weaponOverride.NewWeaponName);
                    }
                }
            }
        }

        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to load overrides file.\nFile: {file}");
        }
    }
}
