using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Weapons.Models;
using Reloaded.Memory.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace P3R.WeaponFramework.Weapons;

internal class WeaponOverridesRegistry(WeaponRegistry weapons) : IWeaponApi
{
    private readonly WeaponRegistry weapons = weapons;
    private readonly List<WeaponOverride> weaponsOverrides = [];

    public bool TryGetWeaponOverride(ECharacter character, int originalWeaponId, [NotNullWhen(true)] out Weapon? newWeapon)
    {
        var weaponOverride = weaponsOverrides.FirstOrDefault(x => x.Character == character);
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
                    if (!int.TryParse(weaponOverride.OriginalWeaponID, out var weaponItemId))
                    {
                        // Else search for weapon by name
                        // Episode Aigis features duplicate weapons so this will allow an override to replace multiple instances with the same name
                        var existingWeapons = this.weapons.Weapons.Where(x => x.Name?.Equals(weaponOverride.OriginalWeaponID, StringComparison.OrdinalIgnoreCase) == true).ToList()
                            ?? throw new Exception();
                        // If there's more than one weapon that matches that name, add all to overrides
                        if (existingWeapons.Count > 1)
                        {
                            foreach (var weap in existingWeapons)
                            {
                                weaponsOverrides.Add(new()
                                {
                                    Character = chara,
                                    Episode = episode,
                                    OriginalWeaponID = weap.WeaponItemId,
                                    NewWeaponName = weaponOverride.NewWeaponName,
                                });
                            }
                        }
                        // Else if only one weapon is found add that.
                        else if (existingWeapons.Count > 0)
                        {
                            weaponsOverrides.Add(new()
                            {
                                Character = chara,
                                Episode = episode,
                                OriginalWeaponID = existingWeapons.First().WeaponItemId,
                                NewWeaponName = weaponOverride.NewWeaponName,
                            });
                        }
                    }
                    // If the override specifies a weaponItemId there will only be one override anyway
                    // No need to replace multiple. 
                    else
                    {
                        weaponsOverrides.Add(new()
                        {
                            Character = chara,
                            Episode = episode,
                            OriginalWeaponID = weaponItemId,
                            NewWeaponName = weaponOverride.NewWeaponName,
                        });
                    }
                }
                else if (episode == FEpisode.Vanilla)
                {
                    // Parse weapon ItemID as int
                    if (!int.TryParse(weaponOverride.OriginalWeaponID, out var weaponItemId))
                    {
                        // Else search for weapon by name
                        var existingWeapon = this.weapons.GetActiveWeapons().FirstOrDefault(x => x.Name?.Equals(weaponOverride.OriginalWeaponID, StringComparison.OrdinalIgnoreCase) == true)
                            ?? throw new Exception();
                        weaponItemId = existingWeapon.WeaponItemId;
                    }
                    // No duplicate vanilla weapons exist so vanilla overrides are much simpler.
                    weaponsOverrides.Add(new()
                    {
                        Character = chara,
                        Episode = episode,
                        OriginalWeaponID = weaponItemId,
                        NewWeaponName = weaponOverride.NewWeaponName,
                    });
                }
            }
        }

        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to load overrides file.\nFile: {file}");
        }
    }
}
