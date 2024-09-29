using P3R.WeaponFramework.Hooks.Weapons.Models;
using P3R.WeaponFramework.Types;
using P3R.WeaponFramework.Utils;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Unreal.ObjectsEmitter.Interfaces;

using static P3R.WeaponFramework.Types.ShellExtensions;

namespace P3R.WeaponFramework.Hooks.Services;
internal struct ShellPathEntry
{
    List<string> basePaths;
    List<string> defaultPaths;
    List<string> nowPaths;

    public ShellPathEntry(ShellType shellType)
    {
        basePaths = shellType.AsShell().BasePaths;
        defaultPaths = shellType.AsShell().ShellPaths;
        nowPaths = shellType.AsShell().BasePaths;
    }
    public ShellPathEntry(List<string> basePaths, List<string> defaultPaths, List<string> nowPaths)
    {
        this.basePaths = basePaths;
        this.defaultPaths = defaultPaths;
        this.nowPaths = nowPaths;
    }
    public void Update (List<string> strings)
    {
        nowPaths.Clear();
        foreach (string s in strings)
        {
            nowPaths.Add(s);
        }
    }
    public List<string> BasePaths
    {
        get => basePaths;
        set => basePaths = value;
    }
    public List<string> DefaultPaths
    {
        get => defaultPaths;
        set => defaultPaths = value;
    }
    public List<string> NowPaths
    {
        get => this.nowPaths;
        set => this.nowPaths = value;
    }
}
internal class ShellPathLibrary: IEnumerable<KeyValuePair<ShellType, ShellPathEntry>>, IReadOnlyCollection<KeyValuePair<ShellType, ShellPathEntry>>
{
    private Dictionary<ShellType, ShellPathEntry> mainDict;
    private readonly Dictionary<ShellType, List<string>> baseShellPaths;
    private readonly Dictionary<ShellType, List<string>> defaultShellPaths;
    private readonly Dictionary<ShellType, List<string>> nowShellPaths;
    private readonly List<IDictionary> allDicts; 

    public ShellPathLibrary() 
    {
        mainDict = [];
        baseShellPaths = [];
        defaultShellPaths = [];
        nowShellPaths = [];
        allDicts = [mainDict, baseShellPaths, defaultShellPaths, nowShellPaths];
    }

    public Dictionary<ShellType, ShellPathEntry> MainDict
    {
        get => mainDict;
        set => mainDict = value;
    }

    public int Count => mainDict.Count;

    public bool IsReadOnly => false;

    public IEnumerator<KeyValuePair<ShellType, ShellPathEntry>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<ShellType, ShellPathEntry>>)mainDict).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)mainDict).GetEnumerator();
    }

    public ShellPathEntry this[ShellType key] => GetEntry(key);
    private ShellPathEntry GetEntry(ShellType key) => mainDict[key];
    public void Add(ShellType item)
    {
        var entry = new ShellPathEntry(item);
        mainDict.Add(item, entry);
        baseShellPaths.Add(item, entry.BasePaths);
        defaultShellPaths.Add(item, entry.DefaultPaths);
        nowShellPaths.Add(item, entry.NowPaths);
    }

    public void Clear()
    {
        foreach (var dict in allDicts)
        {
            dict.Clear();
        }
    }

    public bool Contains(ShellType type)
    {
        return allDicts.All(dict => dict.Contains(type));
    }

    public bool Remove(ShellType type)
    {
        foreach (var dict in allDicts) { dict.Remove(type); }
        return !Contains(type);
    }

    public void Update(ShellType shellType, List<string> newPaths)
    {
        mainDict[shellType].Update(newPaths);
        nowShellPaths[shellType] = newPaths;
    }
    public List<string> BasePaths(ShellType type) => baseShellPaths.Where(s => s.Key == type).SelectMany(s => s.Value).ToList();
    public List<string> DefaultPaths(ShellType type) => defaultShellPaths.Where(s => s.Key == type).SelectMany(s => s.Value).ToList();
    public List<string> NowPaths(ShellType type) => nowShellPaths.Where(s => s.Key == type).SelectMany(s => s.Value).ToList();
}
internal unsafe class WeaponShellService
{
    const string MODULE = "Weapon Framework - Shell Service";
    private readonly Dictionary<ECharacter, Dictionary<ShellType, Weapon>> defaultWeaponShells = [];
    private readonly Dictionary<ShellType, int> currWeapIds = [];
    private readonly Dictionary<ShellType, int> currWeapModelIds = [];
    private readonly Dictionary<ShellType, int> prevWeapIds = [];
    private readonly Dictionary<ShellType, int> prevWeapModelIds = [];

    private readonly ShellPathLibrary shellPathLib = [];
    private readonly Dictionary<ECharacter, List<ShellType>> defaultShells = [];
    private readonly Dictionary<ShellType, int> defaultModelIds = [];
    private readonly Dictionary<ShellType, Weapon> defaultWeapons = [];

    private readonly List<ShellType> activeShells = [];

    private readonly IUnreal unreal;
    private readonly WeaponRegistry weapons;
    public WeaponShellService(IUnreal unreal, WeaponRegistry weapons)
    {
        this.weapons = weapons;
        this.unreal = unreal;
        Log.Debug($"{string.Join(", ", Characters.Armed)}");
        foreach (var character in Characters.Armed)
        {
            List<ShellType> shells = [];
            Dictionary<ShellType, Weapon> charWeaps = [];
            foreach (var shellType in Characters.Lookup[character].ShellTypes)
            {
                var defaultShellWeap = new DefaultWeapon(shellType);
                shellPathLib.Add(shellType);
                shells.Add(shellType);
                charWeaps.Add(shellType, defaultShellWeap);
                activeShells.Add(shellType);
            }
            defaultWeaponShells[character] = charWeaps;
            defaultShells[character] = [.. shells];
        }
    }

    public void InitRedirects()
    {
        foreach (var shell in activeShells)
        {
            Log.Debug($"Initializing {shell} shell.");
            DefaultShells(shell);
        }
    }

    public int UpdateWeapon(ECharacter character, int weaponId)
    {
        if (!weapons.TryGetWeapon(character, weaponId, out var weapon))
            return defaultModelIds[defaultShells[character].First()];

        var shellType = weapon.ShellTarget;
        var SHELL_MODEL_ID = defaultModelIds[shellType];
        var modelId = weapon.ModelId;

        if (modelId == SHELL_MODEL_ID && currWeapModelIds[shellType] != modelId)
        {
            RedirectHandler(weapon);
        }

        if (weaponId < 512)
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
            RedirectHandler(weapon);
        }
        prevWeapModelIds[shellType] = currWeapIds[shellType];
        currWeapModelIds[shellType] = SHELL_MODEL_ID;
        return SHELL_MODEL_ID;
    }


    private void DefaultShells(ShellType shellType)
    {
        var entry = shellPathLib[shellType];
        var basePaths = entry.BasePaths;
        var shellPaths = entry.DefaultPaths;
        shellPathLib.Update(shellType, shellPaths);
        var bp1 = basePaths[0];
        var sp1 = shellPaths[0];
        unreal.AssignFName(MODULE, bp1, sp1);
        if (basePaths.Count != 2)
        {
            return;
        }
        else
        {
            var bp2 = basePaths[1];
            var sp2 = shellPaths[1];
            unreal.AssignFName(MODULE, bp2, sp2);
        }
    }

    public void RedirectHandler(Weapon weapon)
    {
        var shell = weapon.ShellTarget;
        if (weapon.ModelId == (int)shell && weapon.ModelId < 512)
            DefaultShells(weapon);
        else if (weapon.ModelId == (int)shell)
            RedirectShells(weapon);
    }
    private void DefaultShells(Weapon weapon)
    {
        var entry = shellPathLib[weapon.ShellTarget];
        var basePaths = entry.BasePaths;
        var shellPaths = entry.DefaultPaths;
        shellPathLib.Update(weapon.ShellTarget, shellPaths);
        var bp1 = basePaths[0];
        var sp1 = shellPaths[0];
        unreal.AssignFName(MODULE, bp1, sp1);
        if (basePaths.Count != 2)
        {
            return;
        }
        else
        {
            var bp2 = basePaths[1];
            var sp2 = shellPaths[1];
            unreal.AssignFName(MODULE, bp2, sp2);
        }
    }
    private void RedirectShells(Weapon weapon)
    {
        var entry = shellPathLib[weapon.ShellTarget];
        var basePaths = entry.BasePaths;
        if (!weapon.TryGetPaths(out var weapPaths))
            return;
        if (weapPaths != null && weapPaths.Count == basePaths.Count) 
        {
            shellPathLib.Update(weapon.ShellTarget, weapPaths);
            var bp1 = basePaths[0];
            var wp1 = weapPaths[0];
            unreal.AssignFName(MODULE, bp1, wp1);
            if (basePaths.Count != 2)
            {
                return;
            }
            else
            {
                var bp2 = basePaths[1];
                var wp2 = weapPaths[1];
                unreal.AssignFName(MODULE, bp2, wp2);
            }
        }
        return;
    }

    private static List<string>? GetWeaponPaths(Weapon weapon) 
    {
        {            
            weapon.TryGetPaths(out var paths);
            if (paths != null && paths.Any())
                return paths;
            return null;
        }
    }
    public string? GetDefaultAsset(ShellType shellType, WeaponAssetType assetType)
        => defaultWeapons[shellType].Config.GetAssetFile(assetType);

}
