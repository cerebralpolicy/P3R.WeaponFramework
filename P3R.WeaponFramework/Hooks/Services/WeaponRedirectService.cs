using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using p3rpc.classconstructor.Interfaces;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;
using YamlDotNet.Core.Tokens;
using static P3R.WeaponFramework.Hooks.Services.WeaponRedirectService.ModelIdRedirect;

namespace P3R.WeaponFramework.Hooks.Services
{
    internal unsafe class SkeletalMeshDictionary : IDictionary<string, nint>
    {
        private IDictionary<string,nint> namedPtrs;

        public SkeletalMeshDictionary()
        {
            namedPtrs = new Dictionary<string,nint>();
        }

        public nint this[string key] { get => namedPtrs[key]; set => namedPtrs[key] = value; }

        public bool TryGetMesh(string key, [NotNullWhen(true)] out USkeletalMesh* mesh)
        {
            if (namedPtrs.TryGetValue(key, out var ptr))
            {
                mesh = (USkeletalMesh*)ptr;
                return true;
            }
            else
            {
                mesh = null;
                return false;
            }
        }

        public ICollection<string> Keys => namedPtrs.Keys;

        public ICollection<nint> Values => namedPtrs.Values;

        public int Count => namedPtrs.Count;

        public bool IsReadOnly => namedPtrs.IsReadOnly;

        public bool TryAdd(string key, USkeletalMesh* mesh)
        {
            if (namedPtrs.ContainsKey(key))
                return false;
            Add(key, mesh);
            return true;
        }

        public void Add(string key, USkeletalMesh* mesh)
        {
            var value = (nint)mesh;
            namedPtrs.Add(key, value);
        }
        public void Add(string key, nint value)
        {
            namedPtrs.Add(key, value);
        }

        public void Add(KeyValuePair<string, nint> item)
        {
            namedPtrs.Add(item);
        }

        public void Clear()
        {
            namedPtrs.Clear();
        }

        public bool Contains(KeyValuePair<string, nint> item)
        {
            return namedPtrs.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return namedPtrs.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, nint>[] array, int arrayIndex)
        {
            namedPtrs.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, nint>> GetEnumerator()
        {
            return namedPtrs.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return namedPtrs.Remove(key);
        }

        public bool Remove(KeyValuePair<string, nint> item)
        {
            return namedPtrs.Remove(item);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out nint value)
        {
            return namedPtrs.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)namedPtrs).GetEnumerator();
        }
    }
    internal unsafe class SkeletalMeshList : ICollection
    {
        private List<nint> meshPtrs = [];

        public USkeletalMesh* this[int index] { get => (USkeletalMesh*)meshPtrs[index]; set => meshPtrs[index] = (nint)value; }

        public SkeletalMeshList()
        {

        }

        public int Count => ((ICollection)meshPtrs).Count;

        public bool IsSynchronized => ((ICollection)meshPtrs).IsSynchronized;

        public object SyncRoot => ((ICollection)meshPtrs).SyncRoot;

        public void Add(USkeletalMesh* value)
        {
            meshPtrs.Add((nint)value);
        }

        public bool TryAdd(USkeletalMesh* value)
        {
            if (this.Contains(value)) return false;
            meshPtrs.Add((nint)value);
            return true;
        }

        public void Clear()
        {
            meshPtrs.Clear();
        }


        public bool Contains(USkeletalMesh* value)
        {
            return meshPtrs.Contains((nint)value);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)meshPtrs).CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)meshPtrs).GetEnumerator();
        }

        public int IndexOf(USkeletalMesh* value)
        {
            return meshPtrs.IndexOf((nint)value);
        }

        public void Insert(int index, USkeletalMesh* value)
        {
            meshPtrs.Insert(index, (nint)value);
        }

        public void Remove(USkeletalMesh* value)
        {
            meshPtrs.Remove((nint)value);
        }

        public void RemoveAt(int index)
        {
            meshPtrs.RemoveAt(index);
        }
    }
    internal unsafe class WeaponRedirectService
    {
        public const string SERVICEID = "P3WF Redirect Service";
        public IUnreal unreal;
        public IUObjects uObjects;
        public IMemoryMethods memoryMethods;
        public IObjectMethods objectMethods;
        private readonly WeaponRegistry weapons;
        private readonly WeaponOverridesRegistry overrides;
        private Dictionary<int, ModelIdRedirect> redirects = [];
        private List<nint> weaponMeshes = [];
        private List<string> WeaponNames = [];
        private SkeletalMeshDictionary meshDictionary = [];

        public WeaponRedirectService(IUnreal unreal, IUObjects uObjects, IMemoryMethods memoryMethods, IObjectMethods objectMethods, WeaponRegistry weapons, WeaponOverridesRegistry overrides)
        {
            this.unreal = unreal;
            this.uObjects = uObjects;
            this.memoryMethods = memoryMethods;
            this.objectMethods = objectMethods;
            this.weapons = weapons;
            this.overrides = overrides;
            InitRedirectDict();
            uObjects.ObjectCreated += WeaponMeshCreated;
        }

        private void WeaponMeshCreated(UnrealObject obj)
        {
            if (obj.Name.Contains("SK_Wp") || obj.Name.Contains("weapon-mesh", StringComparison.InvariantCultureIgnoreCase))
            {
                Log.Debug($"Found {obj.Name}");
                if (meshDictionary.TryAdd(obj.Name,(USkeletalMesh*)obj.Self))
                    Log.Debug($"{obj.Name} added to dict.");
                weaponMeshes.Add((nint)obj.Self);
                WeaponNames.Add(obj.Name);
            }
            else if (obj.Name.Contains("Weapon", StringComparison.InvariantCultureIgnoreCase))
            {
                var self = (Native.UObject*)obj.Self;
                var uClass = self->ClassPrivate->class_within->class_conf_name;
                
                var className = unreal.GetName(uClass.pool_location);
                Log.Debug($"Found {obj.Name} || Type: {className}");
            }
        }

        public void InitRedirectDict()
        {
            var uniqueWeaponsByModel = weapons.GetActiveWeapons().Where(x => x.HasValidModelId()).DistinctBy(x => x.ModelId);
            foreach (var model in uniqueWeaponsByModel)
            {
                var modelId = model.ModelId;
                var chara = model.Character;
                model.TryGetPaths(out var paths);
                if (paths == null)
                    continue;
                var redirect = new ModelIdRedirect(chara, modelId);
                redirects.Add(modelId, redirect);
            }
        }

        public bool NameSearch(string path)
        {
            var name = Path.GetFileName(path);
            var uPath = GetUnrealAssetPath(path);
            var uName = Path.GetFileName(uPath);
            int successes = 0;
            var queryArray = new List<string>([path, name, uPath, uName]);
            foreach (var query in queryArray)
            {
                if (LookupName(query))
                    successes++;
            }
            return successes > 0;
        }
        private bool LookupName(string nameQuery)
        {
            var fname = unreal.FName(nameQuery, EFindName.FName_Find);
            var name = unreal.GetName(fname);

            if (name == "None")
            {
                //Log.Error($"{nameQuery} does not exist.");
                return false;
            }
            Log.Debug($"{name} exists.");
            return true;
        }
        public bool LookupObject(string path, [NotNullWhen(true)] out USkeletalMesh* meshPtr)
        {
            var fileName = Path.GetFileName(path);
            if (meshDictionary.ContainsKey(fileName) && meshDictionary.TryGetMesh(fileName, out var mesh))
            {
                meshPtr = mesh;
                return true;
            }
            meshPtr = null;
            return false;
        }
        private void UpdateMesh(ModelIdRedirect.PathPair pathPair) 
        { 
            var basePath = pathPair.BasePath;
            var baseName = Path.GetFileName(basePath);
            var newPath = pathPair.NewPath;
            var newName = Path.GetFileName(newPath);
            var uPath = GetUnrealAssetPath(newPath);
            var uName = Path.GetFileName(uPath);            
            var query = LookupName(newPath) || LookupName(uPath) || LookupName(newName) || LookupName(uName);
            if (!query)
            {
                Log.Error($"Cannot find {newName} in name pool. Redirect may not work.");
            }
            unreal.AssignFName(SERVICEID, basePath, newPath);
        }
        public void RevertToShellPaths(ModelIdRedirect idRedirect)
        {
            var shellPair1 = idRedirect.Mesh1.ShellPair;
            var shellPair2 = idRedirect.Mesh2?.ShellPair;
            if (shellPair1 != null) 
                UpdateMesh(shellPair1);
            if (shellPair2 != null)
                UpdateMesh(shellPair2);
        }
        private bool VerifyRedirect(ModelIdRedirect.PathPair pathPair)
        {
            var basePath = pathPair.BasePath;
            var baseName = Path.GetFileName(basePath);
            var newPath = pathPair.NewPath;
            var newName = Path.GetFileName(newPath);
            var uPath = GetUnrealAssetPath(newPath);
            var uName = Path.GetFileName(uPath);

            if (!LookupObject(baseName, out var baseObj))
            {
                Log.Error($"Couldn't find {baseName} in registry");
                return false;
            }
            else
            {
                Log.Debug($"{baseName} exists in the mesh dictionary.");
                return true;
            }
        }
        public void RedirectToModPaths(ModelIdRedirect idRedirect)
        {
            var modPair1 = idRedirect.Mesh1.ModPair;
            var modPair2 = idRedirect.Mesh2?.ModPair;
            if (modPair1 != null)
                UpdateMesh(modPair1);
            if (modPair2 != null)
                UpdateMesh(modPair2);
        }

        public int UpdateFromEquippedWeapon(ECharacter character, int equippedItemId, int compModelId)
        {
            int stage = 1;
            int step = 1;
            void OutputStep(string message, bool isError = false)
            {
                var logMsg = $"\t[{step}] {message}";
                if (isError)
                {
                    Log.Error(logMsg);
                }
                else
                {
                    Log.Verbose(logMsg);
                }
                step++;
            }
            void OutputStage(string message, bool isError = false)
            {
                var logMsg = $"[{stage}] {message}";
                if (isError)
                {
                    Log.Error(logMsg);
                }
                else
                {
                    Log.Debug(logMsg);
                }
                stage++;
                step = 1;
            }
            if (weapons.TryGetWeaponByItemId(equippedItemId, out var weapon))
            {
                var weaponId = weapon.WeaponId;
                var weaponModelId = weapon.ModelId;
                if (compModelId < 0)
                {
                    return weaponModelId;
                }
                OutputStage("Searching for redirect definition.");
                if (!redirects.TryGetValue(weaponModelId, out var idRedirect))
                {
                    OutputStep("Couldn't find definition", true);
                }
                else
                {
                    var bIsMod = weapon.IsModded;
                    var pathSummary = $"Mesh 1\n" +
                        $"\tBase Path: {idRedirect.Mesh1.BasePath}\n" +
                        $"\tShell Path: {idRedirect.Mesh1.ShellPath}";
                    if (idRedirect.Mesh2 != null)
                    {
                        pathSummary = pathSummary + "\n" +
                            $"Mesh 2\n" +
                            $"\tBase Path: {idRedirect.Mesh2.BasePath}\n" +
                            $"\tShell Path: {idRedirect.Mesh2.ShellPath}";
                    }
                    OutputStep($"Redirect found\n{pathSummary}");
                    if (overrides.TryGetWeaponOverrideFrom(weapon.Character, weaponId, out var newWeapon))
                    {
                        OutputStep($"Weapon override found || Original: {weapon.Name} || New: {newWeapon.Name}");
                        weapon = newWeapon;
                        bIsMod = newWeapon.IsModded;
                    }
                    idRedirect.UpdateModPaths(weapon);

                    if (bIsMod)
                    {
                        OutputStage("Performing redirects.");
                        OutputStep("Applying mod mesh.");
                        idRedirect.bUsingMod = true;
                        idRedirect.bUsingShell = false;
                        idRedirect.iRedirectCount++;
                        RedirectToModPaths(idRedirect);
                        OutputStage("Redirection complete");
                        return compModelId;
                    }
                    else if (idRedirect.iRedirectCount > 0 && !bIsMod)
                    {
                        OutputStage("Performing redirects.");
                        OutputStep("Reverting to shell.");
                        idRedirect.bUsingMod = false;
                        idRedirect.bUsingShell = true;
                        idRedirect.iRedirectCount++;
                        RevertToShellPaths(idRedirect);
                        OutputStage("Redirection complete");
                        return compModelId;
                    }
                }
            }
            return compModelId;
        }
        #region Sub-classes

        internal class ModelIdRedirect
        {
            #region RedirectStatus
            public int iRedirectCount = 0;
            public bool bUsingShell = false;
            public bool bUsingMod = false;
            #endregion
            private MeshRedirect mesh1;
            internal bool hasMesh2 { get; set; }
            private MeshRedirect? mesh2;

            public MeshRedirect Mesh1 { get { return this.mesh1; } set { this.mesh1 = value; } }
            [MemberNotNullWhen(true, nameof(this.hasMesh2))]
            [NotNullIfNotNull(nameof(mesh2))]
            public MeshRedirect? Mesh2 { get { return this.mesh2; } set { this.mesh2 = value; } }

            public void UpdateModPaths(Weapon weapon)
            {
                weapon.TryGetPaths(out var paths);
                if (paths == null)
                    return;
                mesh1.ModPath = paths[0];
                if (paths.Count > 1 && mesh2 != null)
                    mesh2.ModPath = paths[1];
            }

            public ModelIdRedirect(ECharacter character, int modelId)
            {
                var meshId = ModelPairsInt[modelId];
                mesh1 = new MeshRedirect(character, meshId);
                if (DualModels.Contains(modelId))
                {
                    meshId += 200;
                    hasMesh2 = true;
                    mesh2 = new MeshRedirect(character, meshId);
                }
            }

            internal class MeshRedirect
            {
                public MeshRedirect(ECharacter character, int meshId)
                {
                    basePath = GetVanillaAssetFile(character, meshId);
                    shellPath = GetShellAssetFile(character, meshId);
                    modPath = GetVanillaAssetFile(character, meshId);
                }

                private string basePath;
                private string shellPath;
                private string modPath;

                public string BasePath { get { return basePath; } }
                public string ShellPath { get { return shellPath; } }
                public string ModPath { get { return modPath; } set { modPath = value; } }

                public PathPair ShellPair => PathPair.ShellPair(this);
                public PathPair ModPair => PathPair.ModPair(this);
            }
            internal class PathPair
            {
                public static PathPair ShellPair(MeshRedirect redirect) => new(redirect.BasePath, redirect.ShellPath);
                public static PathPair ModPair(MeshRedirect redirect) => new (redirect.BasePath, redirect.ModPath);

                private string basePath;
                private string newPath;

                public PathPair(string basePath, string newPath)
                {
                    this.basePath = basePath;
                    this.newPath = newPath;
                }
                public string BasePath { get { return basePath; } }
                public string NewPath { get { return newPath; } }
            }
        }
        #endregion
    }
}
