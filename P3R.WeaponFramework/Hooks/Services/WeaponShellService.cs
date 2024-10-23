using P3R.WeaponFramework.Utils;
using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using p3rpc.classconstructor.Interfaces;
using System.Collections;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Hooks.Services;

public unsafe class WeaponShellService
{
    private readonly IUnreal unreal;
    private readonly IUObjects uObjects;
    private readonly IObjectMethods objectMethods;
    private readonly WeaponRegistry weapons;
    private readonly WeaponOverridesRegistry overrides;

    #region ItemId Logic
    private Dictionary<ShellType, int> prevWeapIds = [];
    private Dictionary<ShellType, bool> idIsAtlus = [];
    private bool EvalIsAtlus(int itemId)
    {
        return itemId < 512;
    }
    #endregion
    #region Model Id Logic
    private Dictionary<ShellType, int> prevWeapModelIds = [];
    private Dictionary<ShellType, bool> modelUsesShell = [];
    private Dictionary<ShellType, bool> modelIsMod = [];
    private bool EvalIsDefault(ShellType shell, int modelId)
    {
        return modelId == shell.ModelId();
    }
    private bool EvalIsMod(int itemId) => !EvalIsAtlus(itemId);
    #endregion
    private Dictionary<ShellType, SkeletalShell> shells = [];

    private List<UnrealObject> WeaponMeshes = [];

    public WeaponShellService(IUnreal unreal, IUObjects uObjects, IObjectMethods objectMethods, WeaponRegistry weapons, WeaponOverridesRegistry overrides)
    {
        this.unreal = unreal;
        this.uObjects = uObjects;
        this.objectMethods = objectMethods;
        this.weapons = weapons;
        this.overrides = overrides;

        uObjects.FindObject("weapon-mesh", NewMeshFound);
    }

    private void NewMeshFound(UnrealObject obj)
    {
        Log.Debug("Weapon mesh found");
        WeaponMeshes.Add(obj);
    }

    /// <summary>
    /// Deferred method to populate the shell dictionary. If called with the ctor
    /// </summary>
    public void Init()
    {
        foreach (var shell in ShellExtensions.ShellLookup)
        {
            var shellType = shell.EnumValue;
            var skeletalShell = new SkeletalShell(objectMethods, shell);
            shells.Add(shellType, skeletalShell);

            idIsAtlus.Add(shellType, true);
            modelUsesShell.Add(shellType, false);
            modelIsMod.Add(shellType, false);
            prevWeapIds.Add(shellType, -1);
            prevWeapModelIds.Add(shellType, -1);
        }
    }

    public int UpdateFromEquippedWeapon(ECharacter character, int equippedItemId, int compModelId)
    {
        int stage = 1;
        int step = 1;
        void OutputStep(string message)
        {
            Log.Verbose($"\t[{step}] {message}");
            step++;
        }
        void OutputStage(string messsage)
        {
            Log.Debug($"[{stage}] {messsage}");
            stage++;
            step = 1;
        }

        if (weapons.TryGetWeaponByItemId(equippedItemId, out var weapon))
        {
            var chara = weapon.Character;
            var shell = weapon.ShellTarget;
            var shellIsAstrea = shell == ShellType.Aigis_SmallArms_Astrea || shell == ShellType.Aigis_LongArms_Astrea;
            OutputStage("Loading variables.");
            OutputStep("Grabbing shell model ID");
            var shellModelId = (int)shell - (shellIsAstrea ? 1000 : 0);
            OutputStep("Defining weapon ID");
            var weapId = weapon.WeaponItemId;
            OutputStep("Checking previous isIdAtlus() state");
            var wasAtlus = idIsAtlus[shell];
            OutputStep("Defining model ID");
            var weapModelId = weapon.ModelId;
            
            OutputStep("Evaluating if model uses shell.");
            var isShell = EvalIsDefault(shell, weapModelId);
            var isMod = EvalIsMod(weapId);
            if (overrides.TryGetWeaponOverrideFrom(weapon.Character, weapId, out var newWeapon))
            {
                Log.Verbose($"Weapon override found || Original: {weapon.Name} || New: {newWeapon.Name}");
                weapon = newWeapon;
                weapModelId = newWeapon.ModelId;
                isMod = true;
                isShell = true;
            }
            // If it's the same weapon as before, don't proceed.
            if (prevWeapIds[shell] == weapId)
            {
                return isShell ? shellModelId : compModelId;
            }
            OutputStep("Checking previous modelIsMod() state");
            var wasMod = modelIsMod[shell];
         
            // Data application
            OutputStage("Applying new mesh data.");

            if (isShell)
            {
                prevWeapIds[shell] = weapId;
                var skelShell = shells[shell];
                OutputStep($"Resetting {chara.ToPossPNoun()} weapon shell");
                skelShell.RevertModMeshes();
                if (isMod)
                {
                    OutputStep("Mesh should be applied");
                    idIsAtlus[shell] = false;
                    modelUsesShell[shell] = true;
                    modelIsMod[shell] = true;
                    Log.Warning($"ModelId should be {shellModelId}");
                    return shellModelId;
                    /*skelShell.UpdateModMeshes(weapon, objectMethods, uObjects);
                    if (wasAtlus)
                    {
                        // Log
                    }
                    else if (wasMod)
                    {
                        // Log
                    }
                    return weapModelId;*/
                }
                else
                {
                    OutputStep("Mesh should be reset.");
                    idIsAtlus[shell] = true;
                    modelUsesShell[shell] = true;
                    modelIsMod[shell] = false;
                    Log.Warning($"ModelId should be {shellModelId}");
                    return shellModelId;
                    // Log
                }
            }
            else
            {
                idIsAtlus[shell] = true;
                modelUsesShell[shell] = false;
                modelIsMod[shell] = false;
                return compModelId;
            }
        }
        else
            return compModelId;

    }

    public unsafe class SkeletalShell : IReadOnlyList<SkeletalMeshWrapper>
    {
        private Dictionary<EArmature, SkeletalMeshWrapper> meshes = [];
        public SkeletalShell(IObjectMethods objectMethods, Shell shell)
        {
            foreach (var armature in shell.Armatures)
            {
                var skeletalMesh = new SkeletalMeshWrapper(armature, objectMethods);
                meshes.Add(armature.EnumValue, skeletalMesh);
            }
        }

        public SkeletalMeshWrapper this[int index] => ((IReadOnlyList<SkeletalMeshWrapper>)SkeletalMeshes)[index];

        public void RevertModMeshes()
        {
            foreach (var skelMesh in this)
            {
                skelMesh.RevertModMesh();
            }
        }
        public void UpdateModMeshes(Weapon weapon, IObjectMethods objectMethods, IUObjects uObjects)
        {

            if (weapon.TryGetPaths(out var paths))
            {
                int i = 0;
                foreach (var skelMesh in this)
                {
                    //skelMesh.RevertModMesh();
                    var thisPath = paths[i];
                    if (skelMesh.UpdateModMesh(thisPath, objectMethods, uObjects))
                    {
                        Log.Debug($"\t\t Applying {paths[i]}");
                        skelMesh.ApplyModMesh();
                    }
                    i++;
                }
            }
        }

        public List<SkeletalMeshWrapper> SkeletalMeshes => meshes.Values.ToList();

        public int Count => ((IReadOnlyCollection<SkeletalMeshWrapper>)SkeletalMeshes).Count;

        public IEnumerator<SkeletalMeshWrapper> GetEnumerator()
        {
            return ((IEnumerable<SkeletalMeshWrapper>)SkeletalMeshes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)SkeletalMeshes).GetEnumerator();
        }
    }

    public unsafe class SkeletalMeshWrapper
    {
        private enum MeshClass
        {
            Game,
            Shell,
            Mod
        }
        private BaseMeshObject baseMesh;
        private BaseMeshObject shellMesh;
        private ModMeshObject? modMesh;

        private Armature armature;
        public Armature Armature => armature;
        public SkeletalMeshWrapper(Armature armature, IObjectMethods objectMethods)
        {
            this.armature = armature;
            baseMesh = new BaseMeshObject(armature, objectMethods);
            shellMesh = new BaseMeshObject(armature, objectMethods);
            modMesh = new ModMeshObject(armature);
        }
        public bool UpdateModMesh(string path, IObjectMethods objectMethods, IUObjects uObjects)
        {
            return false;
        }
        public void RevertModMesh()
        {
        }
        public void ApplyModMesh()
        {

        }
    }

    public unsafe class BaseMeshObject : SkeletalMeshObject
    {
        public BaseMeshObject(Armature armature, IObjectMethods objectMethods)
        {
            var path = armature.BasePath;
            var name = Path.GetFileName(path);
            var matches = objectMethods.FindAllObjectsNamed(name, nameof(USkeletalMesh));
            foreach (var match in matches)
            {
                Native.UObject* obj = (Native.UObject*)match;
                var fullName = objectMethods.GetFullName(obj);
                Log.Debug(fullName);
                if (fullName == path)
                {
                    Ptr = match;
                    PtrData = (USkeletalMesh*)match;
                    Data = *PtrData;
                    Name = objectMethods.GetFName(name);
                    FullName = fullName;
                    Armature = armature;
                    break;
                }
                if (obj == (Native.UObject*)matches.Last())
                {
                    Log.Error("No mesh matched the specified path.");
                }
            }
        }

    }

    public unsafe class ModMeshObject : SkeletalMeshObject
    {
        public ModMeshObject(Armature armature)
        {
            Armature = armature;
        }
        public bool Update(string path, IObjectMethods objectMethods, IUObjects uObjects)
        {
            var uPath = AssetUtils.GetUnrealAssetPath(path);
            var name = Path.GetFileName(path);
            uObjects.FindObject(name, processCB);
            bool val = false;
            void processCB(UnrealObject match)
            {
                Native.UObject* obj = (Native.UObject*)match.Self;
                var fullName = objectMethods.GetFullName(obj);
                Log.Debug(fullName);
                if (fullName == path)
                {
                    Ptr = (nint)match.Self;
                    PtrData = (USkeletalMesh*)match.Self;
                    Data = *PtrData;
                    Name = objectMethods.GetFName(name);
                    FullName = fullName;
                    val = true;
                }
            }
            return val;
        }
        public ModMeshObject(Armature armature, string path, IObjectMethods objectMethods, IUObjects uObjects)
        {
            var uPath = AssetUtils.GetUnrealAssetPath(path);
            var name = Path.GetFileName(path);
            var fname = objectMethods.GetFName(name);

            uObjects.FindObject(name, processCB);
            void processCB(UnrealObject match)
            {
                Native.UObject* obj = (Native.UObject*)match.Self;
                var fullName = objectMethods.GetFullName(obj);
                Log.Debug(fullName);
                if (fullName == path)
                {
                    Armature = armature;
                    Ptr = (nint)match.Self;
                    PtrData = (USkeletalMesh*)match.Self;
                    Data = *PtrData;
                    Name = objectMethods.GetFName(name);
                    FullName = fullName;
                }
            }
        }
    }

    public unsafe abstract class SkeletalMeshObject : INullable
    {
        public nint Ptr = 0;
        public USkeletalMesh* PtrData;
        public USkeletalMesh Data;
        public Native.FName Name;
        public string? FullName;
        public Armature? Armature;

        public void UpdatePtrData(USkeletalMesh mesh)
        {
            try
            {
                PtrData->ReplaceData(mesh);
            }
            catch (Exception e)
            {
                Log.Error(e, "Unable to update mesh.");
            }
        }

        public bool IsNull => Ptr == 0;

        public void ApplyData(SkeletalMeshObject? other)
        {
            if (other == null || other.IsNull)
                return;
            Data = other.Data;
            UpdatePtrData(other.Data);
            Marshal.StructureToPtr(Data, Ptr, true);
            Name = other.Name;
            FullName = other.FullName;
        }

        public static implicit operator USkeletalMesh*(SkeletalMeshObject self) => self.PtrData;
    }
}
