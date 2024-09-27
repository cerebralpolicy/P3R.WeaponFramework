using P3R.WeaponFramework.Hooks.Weapons.Models;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using System.Collections.Concurrent;
using Unreal.ObjectsEmitter.Interfaces;

using static P3R.WeaponFramework.Types.ShellExtensions;

namespace P3R.WeaponFramework.Hooks.Services;

internal unsafe class WeaponShellService
{
    const string MODULE = "Weapon Framework - Shell Service";
    private readonly Dictionary<Character, Dictionary<ShellType, Weapon>> defaultWeaponShells = [];
    private readonly Dictionary<ShellType, List<string>> baseShellPaths = [];
    private readonly Dictionary<ShellType, List<string>> currShellPaths = [];
    private readonly Dictionary<ShellType, int> currWeapIds = [];
    private readonly Dictionary<ShellType, int> currWeapModelIds = [];
    private readonly Dictionary<ShellType, int> prevWeapIds = [];
    private readonly Dictionary<ShellType, int> prevWeapModelIds = [];

    private readonly Dictionary<Character, ShellType[]> defaultShells = [];
    private readonly Dictionary<ShellType, int> defaultModelIds = [];
    private readonly Dictionary<ShellType, Weapon> defaultWeapons = [];

    private readonly IUnreal unreal;
    private readonly WeaponRegistry weapons;
    public WeaponShellService(IUnreal unreal, WeaponRegistry weapons)
    {
        this.weapons = weapons;
        this.unreal = unreal;
        foreach (var character in Characters.Armed)
        {
            List<ShellType> shells = [];
            Dictionary<ShellType, Weapon> charWeaps = [];
            foreach (var shellType in Characters.Lookup[character].Value)
            {
                var shell = shellType.AsShell();
                var defaultWeapon = new DefaultWeapon(shell.Key);
                defaultWeapons[shell.Key] = defaultWeapon;
                charWeaps[shell.Key] = defaultWeapon;
                defaultModelIds[shell.Key] = shell.Value.Item3.First();
                shells.Add(shell.Key);
                baseShellPaths[shell.Key] = shell.BasePaths;

            }
            defaultWeaponShells[character] = charWeaps;
            defaultShells[character] = [.. shells];
        }
    }

    public int UpdateWeapon(Character character, int weaponId)
    {
        if (!weapons.TryGetWeapon(character, weaponId, out var weapon))
            return defaultModelIds[defaultShells[character].First()];

        var shellType = weapon.ShellTarget;
        var SHELL_MODEL_ID = defaultModelIds[shellType];
        var modelId = weapon.ModelId;

        if (modelId == SHELL_MODEL_ID && currWeapModelIds[shellType] != modelId)
        {
            RedirectWeaponShell(weapon, true);
        }

        if (weaponId < 1025)
        {
            prevWeapIds[shellType] = currWeapIds[shellType];
            currWeapIds[shellType] = weaponId;
            prevWeapModelIds[shellType] = currWeapModelIds[shellType];
            currWeapModelIds[shellType] = modelId;
            return weaponId;
        }

        var shouldRedirectShell = prevWeapIds[shellType] != weaponId;
        if (shouldRedirectShell)
        {
            RedirectWeaponShell(weapon);
        }
        prevWeapModelIds[shellType] = currWeapIds[shellType];
        currWeapModelIds[shellType] = SHELL_MODEL_ID;
        return SHELL_MODEL_ID;
    }

    private void RedirectWeaponShell(Weapon weapon, bool doReset = false)
    {
        SetWeaponAsset(weapon, WeaponAssetType.Weapon_Mesh, doReset);
        if (ShellLookup[weapon].Meshes > 1)
            SetWeaponAsset(weapon, WeaponAssetType.Weapon_Mesh2, doReset);
    }

    private void SetWeaponAsset(Weapon weapon, WeaponAssetType assetType, bool doReset)
    {
        var shellFile = GetDefaultAsset(weapon.ShellTarget, assetType);
        var assetFile = weapon.Config.GetAssetFile(assetType) ?? shellFile;
        if (assetFile == null || shellFile == null)
        {
            Log.Error($"Weapon asset path is null.\nWeapon ({weapon.Character}): {weapon.Name} || Asset: {assetType}");
            return;
        }
        if (doReset)
        {
            weapon.TryGetPaths(out var paths);
            if (paths == null)
                return;
            DefaultShells(weapon);
            return;
        }
        else if (shellFile != assetFile)
        {
            weapon.TryGetPaths(out var paths);
            if (paths != null)
                currShellPaths[weapon.ShellTarget] = paths;
            RedirectAsset(weapon);
            return;
        }
        else
        {
            Log.Error("Cannot redirect assets.");
            return;
        }
    }

    public void RedirectShell(Weapon weapon)
    {
        var shell = weapon.ShellTarget;
        if (weapon.ModelId == (int)shell)
            DefaultShells(weapon);
        else
            RedirectAsset(weapon);
    }
    private void DefaultShells(Weapon weapon)
    {
        var basePaths = GetBasePaths(weapon);
        var shellPaths = GetShellPaths(weapon);
        for (int i = 0; i < basePaths.Count; i++)
        {
            unreal.AssignFName(MODULE, basePaths[i], shellPaths[i]);
        }
    }
    private void RedirectAsset(Weapon weapon)
    {
        var basePaths = GetBasePaths(weapon);
        var weapPaths = GetWeaponPaths(weapon);
        if (weapPaths != null && weapPaths.Count == basePaths.Count) 
        { 
            for (int i = 0; i < weapPaths.Count; i++)
            {
                unreal.AssignFName(MODULE, basePaths[i], weapPaths[i]);
            }
            return;
        }
        return;
    }
    private static List<string> GetBasePaths(Weapon weapon) => ShellExtensions.ShellLookup[weapon].BasePaths;
    private static List<string> GetShellPaths(Weapon weapon) => ShellExtensions.ShellLookup[weapon].ShellPaths;
    private static List<string>? GetWeaponPaths(Weapon weapon) 
    {
        weapon.TryGetPaths(out var paths);
        if (paths != null && paths.Any())
            return paths;
        return null;
    }
    public string? GetDefaultAsset(ShellType shellType, WeaponAssetType assetType)
        => defaultWeapons[shellType].Config.GetAssetFile(assetType);

}
