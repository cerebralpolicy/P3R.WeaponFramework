using P3R.WeaponFramework.Weapons;
using P3R.WeaponFramework.Weapons.Models;
using p3rpc.classconstructor.Interfaces;
using Reloaded.Hooks.Definitions;
using Reloaded.Memory;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using SharedScans.Interfaces;
using System.Runtime.InteropServices;
using Unreal.AtlusScript.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace P3R.WeaponFramework
{
    public class Core
    {
        public Core(long baseAddress, string modLocation, IConfigurable config, ILogger logger, IStartupScanner startupScanner, IReloadedHooks hooks, Utils utils, Memory memory, ISharedScans sharedScans, IClassMethods classMethods, IObjectMethods objectMethods, IMemoryMethods memoryMethods,IAtlusAssets atlusAssets, IUnreal unreal)
        {
            BaseAddress = baseAddress;
            ModLocation = modLocation;
            Config = config;
            Logger = logger;
            StartupScanner = startupScanner;
            Hooks = hooks;
            Utils = utils;
            Memory = memory;
            SharedScans = sharedScans;
            ClassMethods = classMethods;
            ObjectMethods = objectMethods;
            MemoryMethods = memoryMethods;
            AtlusAssets = atlusAssets;
            Unreal = unreal;
        }
        public const int NEWITEMSTART = 1000;
        public int NewItemIndex = 1000;
        public long BaseAddress { get; init; }
        public string ModLocation { get; init; }
        public IConfigurable Config {  get; set; }
        public ILogger Logger { get; init; }
        public IStartupScanner StartupScanner { get; init; }
        public IReloadedHooks Hooks { get; init; }
        public Utils Utils { get; init; }
        public Memory Memory { get; init; }
        public ISharedScans SharedScans { get; init; }
        public IClassMethods ClassMethods { get; private set; }
        public IObjectMethods ObjectMethods { get; private set; }
        public IMemoryMethods MemoryMethods { get; private set; }
        public IAtlusAssets AtlusAssets { get; private set; }
        public IUnreal Unreal { get; private set; }

        public unsafe string GetFName(FName name) => ObjectMethods.GetFName(name);
        public unsafe string GetObjectName(UObject* obj) => ObjectMethods.GetObjectName(obj);
        public unsafe string GetFullName(UObject* obj) => ObjectMethods.GetFullName(obj);
        public unsafe string GetObjectType(UObject* obj) => ObjectMethods.GetObjectType(obj);
        public unsafe UClass* GetType(string type) => ObjectMethods.GetType(type);
        public unsafe void GetTypeAsync(string type, Action<nint> foundCb) => ObjectMethods.GetTypeAsync(type, foundCb);
        public unsafe bool IsObjectSubclassOf(UObject* obj, UClass* type) => ObjectMethods.IsObjectSubclassOf(obj, type);
        public unsafe bool DoesNameMatch(UObject* tgtObj, string name) => ObjectMethods.DoesNameMatch(tgtObj, name);
        public unsafe bool DoesClassMatch(UObject* tgtObj, string name) => ObjectMethods.DoesClassMatch(tgtObj, name);
        // Convenience functions
        public unsafe UObject* GetEngineTransient() => ObjectMethods.GetEngineTransient();
        // void cb -> Action<UObject*>
        public unsafe void NotifyOnNewObject(UClass* type, Action<nint> cb) => ObjectMethods.NotifyOnNewObject(type, cb);
        public unsafe void NotifyOnNewObject(string typeName, Action<nint> cb) => ObjectMethods.NotifyOnNewObject(typeName, cb);
        public unsafe void FindObjectAsync(string targetObj, string? objType, Action<nint> foundCb) => ObjectMethods.FindObjectAsync(targetObj, objType, foundCb);
        public unsafe void FindObjectAsync(string targetObj, Action<nint> foundCb) => ObjectMethods.FindObjectAsync(targetObj, foundCb);
        public unsafe void FindFirstOfAsync(string objType, Action<nint> foundCb) => ObjectMethods.FindFirstOfAsync(objType, foundCb);
        public unsafe void FindAllOfAsync(string objType, Action<ICollection<nint>> foundCb) => ObjectMethods.FindAllOfAsync(objType, foundCb);

        // CUSTOM
        public unsafe void NotifyOnNewObject<T>(Action<nint> cb) where T : unmanaged => ObjectMethods.NotifyOnNewObject<T>(cb);
        public unsafe void FindObject<T>(string targetObject) where T : unmanaged => ObjectMethods.FindObject<T>(targetObject);

        public unsafe void AddNewWeapon(Weapon weapon, int armatureIndex = 1)
        {
            var config = weapon.Config;
            var mesh = config.Mesh.MeshPath;
            if (mesh == null)
                return;
            var meshFName = ObjectMethods.GetFName(mesh);
            var meshData = new FAppCharWeaponMeshData(meshFName);

            var BPName = AssetUtils.GetWeaponBP(weapon.Character, armatureIndex);
            var BP = ObjectMethods.FindObject<AAppCharWeaponBase>(BPName!);
            var map = BP->WeaponTbl.Data;

            var nameTable = ObjectMethods.FindObject<UItemNameListTable>("DatItemWeaponNameDataAsset")->Data;
            MemoryMethods.TArray_Insert(&nameTable, weapon.Name.MakeFString());

            var id = weapon.ModelId;
            MemoryMethods.TMap_Insert(&map, id, meshData);
            NewItemIndex++;
        }
    }
}
