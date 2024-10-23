using P3R.WeaponFramework.Hooks.Weapons.Models;
using P3R.WeaponFramework.Types;
using P3R.WeaponFramework.Utils;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using Unreal.ObjectsEmitter.Interfaces;

using FNamePool = Unreal.ObjectsEmitter.Interfaces.Types.FNamePool;
using FName = Unreal.ObjectsEmitter.Interfaces.Types.FName;
using EFindName = Unreal.ObjectsEmitter.Interfaces.Types.EFindName;

using static P3R.WeaponFramework.Types.ShellExtensions;
using P3R.WeaponFramework.Hooks.Weapons;
using Unreal.ObjectsEmitter.Interfaces.Types;

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
    private readonly Dictionary<ShellType, bool> redirectStatus;
    private readonly Dictionary<ShellType, int> currentWeaponIds;
    private readonly Dictionary<ShellType, List<string>> baseShellPaths;
    private readonly Dictionary<ShellType, List<string>> defaultShellPaths;
    private readonly Dictionary<ShellType, List<string>> nowShellPaths;

    private readonly List<IDictionary> allDicts; 

    public ShellPathLibrary() 
    {
        mainDict = [];
        redirectStatus = [];
        currentWeaponIds = [];
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
        redirectStatus.Add(item, false);
        currentWeaponIds.Add(item, (int)item);
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

    public void Update(ShellType shellType, int weaponId, List<string> newPaths)
    {
        mainDict[shellType].Update(newPaths);
        currentWeaponIds[shellType] = weaponId;
        if (newPaths == defaultShellPaths[shellType])
            redirectStatus[shellType] = false;
        else 
            redirectStatus[shellType] = true;
        nowShellPaths[shellType] = newPaths;
    }
    public bool RedirectStatus(ShellType type) => redirectStatus[type];
    public int CurrentWeaponId(ShellType type) => currentWeaponIds[type];
    public List<string> BasePaths(ShellType type) => baseShellPaths.Where(s => s.Key == type).SelectMany(s => s.Value).ToList();
    public List<string> DefaultPaths(ShellType type) => defaultShellPaths.Where(s => s.Key == type).SelectMany(s => s.Value).ToList();
    public List<string> NowPaths(ShellType type) => nowShellPaths.Where(s => s.Key == type).SelectMany(s => s.Value).ToList();
}


internal unsafe class WeaponShellServiceOld
{
    const string MODULE = "Weapon Framework - Shell Service";

    static ShellType[] validShells = [
        ShellType.Player,
        ShellType.Yukari,
        ShellType.Stupei,
        ShellType.Akihiko,
        ShellType.Mitsuru,
        ShellType.Aigis_SmallArms,
        ShellType.Aigis_LongArms,
        ShellType.Ken,
        ShellType.Koromaru,
        ShellType.Shinjiro,
        ShellType.Metis,
        ShellType.Aigis_SmallArms_Astrea,
        ShellType.Aigis_LongArms_Astrea
        ];

    private readonly Dictionary<ECharacter, Dictionary<ShellType, Weapon>> defaultWeaponShells = [];
    private Dictionary<ShellType, Weapon> defaultWeapons = [];

    private Dictionary<ShellType, int> prevWeapIds = [];
    private void updatePrevWeapId(ShellType shell, int value)
    {
        if (!prevWeapIds.TryAdd(shell, value))
        {
            prevWeapIds[shell] = value;
        }
    }
    private Dictionary<ShellType, int> prevWeapModelIds = [];
    private void updatePrevWeapModelId(ShellType shell, int value)
    {
        if(!prevWeapModelIds.TryAdd(shell,value))
        {
            prevWeapModelIds[shell] = value;
        }
    }

    private Dictionary<ShellType, List<nint>> shellDataPtrs = [];
    private void updateShellDataPtrs(ShellType shell, List<nint> nints)
    {
        if(!shellDataPtrs.TryAdd(shell,nints))
        {
            shellDataPtrs[shell] = nints;
        }
    }
    private Dictionary<ShellType, List<nint>> shellDataRows = [];
    private void updateShellDataRows(ShellType shell, List<nint> nints)
    {
        if (!shellDataRows.TryAdd(shell,nints))
        {
            shellDataRows[shell] = nints;
        }
    }

    private Dictionary<ShellType, bool> idIsAtlus = [];
    private void updateIdIsAtlus(ShellType shell, bool state)
    {
        if (!idIsAtlus.TryAdd(shell, state))
        {
            idIsAtlus[shell] = state;
        }
    }
    private Dictionary<ShellType, bool> modelIsDefault = [];
    private void updateModelIsDefault(ShellType shell, bool state)
    {
        if (!modelIsDefault.TryAdd(shell, state))
        {
            modelIsDefault[shell] = state;
        }
    }
    private Dictionary<ShellType, bool> modelIsMod = [];
    private void updateModelIsMod(ShellType shell, bool state)
    {
        if (!modelIsMod.TryAdd(shell, state))
        {
            modelIsMod[shell] = state;
        }    
    }

    private bool EvalIsAtlus(int itemId)
    {
        return itemId < 512;
    }
    private bool EvalIsDefault(ShellType shell, int modelId)
    {
        return modelId == shell.ModelId();
    }
    private bool EvalIsMod(int itemId) => !EvalIsAtlus(itemId);

    private bool DTLoaded = false;
    private readonly IUnreal unreal;
    private readonly IUObjects uobjs;
    private readonly IDataTables dt;
    private readonly WeaponRegistry weapons;
    private readonly WeaponTableService weaponTable;
    public WeaponShellServiceOld(IDataTables dt, IUObjects uobjs, IUnreal unreal, WeaponRegistry weapons, WeaponTableService weaponTable)
    {
        this.weapons = weapons;
        this.unreal = unreal;
        this.uobjs = uobjs;
        this.dt = dt;
        this.weaponTable = weaponTable;
        Log.Debug($"{string.Join(", ", Characters.Armed)}");
        TryInitDT();
        Log.Debug($"Weapon shell service loaded.");
        uobjs.ObjectCreated += creation;
    }

    private void creation(UnrealObject @object)
    {
        if (@object.Name == "DT_Weapon")
        {
            Log.Debug(@object.Name + " has been created");
        }
    }

    private void DTMade()
    {

    }

    private void TryInitDT()
    {
        if (!dt.TryGetDataTable("DT_Weapon", out _))
        {
            Log.Error("Couldn't get DT");
            return;
        }
        dt.FindDataTable<FAppCharWeaponTableRow>("DT_Weapon", table =>
        {
            foreach (var shell in ShellLookup)
            {
                Log.Debug($"Populating shell service entry for {shell.Name}");
                List<nint> ptrs = [];
                List<nint> rows = [];
                var shellType = shell.EnumValue;
                var shellModelId = shell.ModelIds.First();
                foreach (var armature in shell.Armatures)
                {
                    var rowName = armature.Name;
                    var rowObj = table.Rows.First(x => x.Name == rowName);
                    if (rowObj == null)
                    { continue; }
                    var row = rowObj.Self;
                    var rowWeapons = row->Data;
                    rows.Add((nint)row);
                    if (rowWeapons.TryGet(shellModelId, out var rowWeapon))
                    {
                        ptrs.Add((nint)rowWeapon);
                        if (shell.EnumValue > ShellType.Metis)
                        {
                            if (shell.EnumValue == ShellType.Aigis_SmallArms_Astrea)
                            {
                                shellType = ShellType.Aigis_SmallArms;
                            }
                            else
                            {
                                shellType = ShellType.Aigis_LongArms;
                            }
                        }
                    }
                }
                this.prevWeapIds.Add(shellType, -1);
                this.shellDataPtrs.Add(shellType, ptrs);
                this.shellDataRows.Add(shellType, rows);
                this.defaultWeapons.Add(shellType, new DefaultWeapon(shellType));
                this.idIsAtlus.Add(shellType, true);
                this.modelIsDefault.Add(shellType, false);
                this.modelIsMod.Add(shellType, false);
            }
        });
        DTLoaded = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    /// <param name="equppedItemId"></param>
    /// <returns></returns>
    public int UpdateFromEquippedWeapon(ECharacter character, int equppedItemId, int compModelId)
    {
        if (!DTLoaded)
        {
            TryInitDT();
        }
        int step = 1;
        void OutputStep(string message)
        {
            Log.Debug($"[{step}] {message}");
            step++;
        }
        this.weapons.TryGetWeaponByItemId(equppedItemId, out var weapon);
        if (weapon == null)
        {
           // Fallback to default modelID
           return Characters.Lookup[character].Shells[0].ShellModelId;
        }
        var shell = weapon.ShellTarget;
        var shellIsAstrea = shell == ShellType.Aigis_SmallArms_Astrea || shell == ShellType.Aigis_LongArms_Astrea;
        OutputStep("Grabbing shell model ID");
        var shellModelId = (int)shell - (shellIsAstrea ? 1000 : 0);
        OutputStep("Defining weapon ID");
        var weapId = weapon.WeaponItemId;
        OutputStep("Checking previous isIdAtlus() state");
        // Failing here - DT is not found
        var wasAtlus = idIsAtlus[shell];
        OutputStep("Defining model ID");
        var weapModelId = weapon.ModelId;
        OutputStep("Evaluating if model uses shell.");
        var isDefault = EvalIsDefault(shell, weapModelId);
        OutputStep("Evaluating if weapon is modded.");
        var isMod = EvalIsMod(weapId);
        OutputStep("Checking previous modelIsMod() state");
        var wasMod = modelIsMod[shell];
        

        if (isDefault)
        {
            this.prevWeapIds[shell] = weapId;
            this.weaponTable.SetWeaponData(shellModelId, defaultWeapons[shell]);
            
            if (isMod && wasAtlus)
            {
                this.prevWeapIds[shell] = weapId;
                this.weaponTable.SetWeaponData(weapModelId, weapon);
                this.idIsAtlus[shell] = false;
                this.modelIsDefault[shell] = false;
                this.modelIsMod[shell] = true;
            }
            
            else if (isMod && wasMod)
            {
                this.prevWeapIds[shell] = weapId;
                this.weaponTable.SetWeaponData(weapModelId, weapon);
                this.idIsAtlus[shell] = false;
                this.modelIsDefault[shell] = false;
                this.modelIsMod[shell] = true;
            }
            else
            {
                this.idIsAtlus[shell] = true;
                this.modelIsDefault[shell] = true;
                this.modelIsMod[shell] = false;
            }
        }
        else
        {
            this.idIsAtlus[shell] = true;
            this.modelIsDefault[shell] = isDefault;
            this.modelIsMod[shell] = false;
        }
        if (weapModelId != compModelId)
        {
            Log.Warning($"{weapon.Name}'s model id [{weapModelId}] does not match the comp value [{compModelId}]. Defaulting to the comp value.");
            return compModelId;
        }
        return weapModelId;
    }
    public int UpdateWeapon(ShellType shell, int itemId, int modelId)
    {
        var shellModelId = shell.ModelId();
        shell.TryGetCharacterFromShell(out var characterQuery);
        if (characterQuery == null)
        {
            return shellModelId;
        }

        var character = characterQuery.Value;

        if (modelId == shellModelId)
        {
            this.prevWeapIds[shell] = itemId;
            this.weaponTable.SetWeaponData(shellModelId, defaultWeapons[shell]);
        }
        if (itemId < Episode.BASE_EPISODE_WEAP_ID)
        {
            return modelId;
        }

        var shouldUpdateData = this.prevWeapIds[shell] != itemId;
        if (shouldUpdateData && this.weapons.TryGetWeapon(character, itemId, out var weapon))
        {
            this.weaponTable.SetWeaponData(shellModelId, weapon);
            this.prevWeapIds[shell] = weapon.WeaponId;
        }
        return shellModelId;
    }

}
