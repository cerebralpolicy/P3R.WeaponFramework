using P3R.WeaponFramework.Hooks.Weapons.Models;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using System.Collections.Concurrent;
using Unreal.ObjectsEmitter.Interfaces;

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

    private readonly Dictionary<Character, ShellTypeWrapper[]> defaultShells = [];
    private readonly Dictionary<ShellType, int> defaultModelIds = [];
    private readonly Dictionary<ShellType, Weapon> defaultWeapons = [];

    private readonly IUnreal unreal;
    private readonly WeaponRegistry weapons;
    public WeaponShellService(IUnreal unreal, WeaponRegistry weapons)
    {
        this.weapons = weapons;
        this.unreal = unreal;
        foreach (var character in Characters.WFArmed)
        {
            List<ShellTypeWrapper> shells = [];
            Dictionary<ShellType, Weapon> charWeaps = [];
            foreach (var shellType in character.Shells)
            {
                var defaultWeapon = new DefaultWeapon(shellType);
                defaultWeapons[shellType] = defaultWeapon;
                charWeaps[shellType] = defaultWeapon;
                defaultModelIds[shellType] = shellType.ShellTableBaseModelId;
                shells.Add(shellType);
                baseShellPaths[shellType] = shellType.GetBasePaths();

            }
            defaultWeaponShells[character] = charWeaps;
            defaultShells[character] = [.. shells];
            InitShells();
        }
    }

    public void InitShells()
    {
        foreach (var character in Characters.WFArmed)
        {
            foreach (var shellType in character.Shells)
            {
                currShellPaths[shellType] = shellType.GetShellPaths();
                shellType.Init(unreal);
            }
        }
    }

    public int UpdatWeapon(Character character, int weaponId)
    {
        if (!weapons.TryGetWeapon(character, weaponId, out var weapon))
            return defaultShells[character].First().ShellTableBaseModelId;

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
        if (ShellTypeWrapper.FromWeapon(weapon).GetRequiredMeshes() > 1)
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
            DefaultShells(weapon.ShellTarget);
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
    private void DefaultShells(ShellTypeWrapper shell)
    {
        currShellPaths[shell] = shell.GetShellPaths();
        shell.Init(unreal);
    }
    private void RedirectAsset(Weapon weapon)
    {
        weapon.ShellTargetWrapper.Apply(weapon,unreal);
    }
    public string? GetDefaultAsset(ShellType shellType, WeaponAssetType assetType)
        => defaultWeapons[shellType].Config.GetAssetFile(assetType);

}
