using P3R.WeaponFramework.Configuration;
using P3R.WeaponFramework.Enums;
using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Template;
using P3R.WeaponFramework.Weapons;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;
using Unreal.AtlusScript.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;
using UnrealEssentials.Interfaces;
using System.Drawing;
using P3R.WeaponFramework.Core;
using System.Diagnostics.CodeAnalysis;
using P3R.WeaponFramework;
using P3R.WeaponFramework.Utils;
using P3R.WeaponFramework.Debugging;
using P3R.WeaponFramework.Hooks.Services;
using p3rpc.classconstructor.Interfaces;
using P3R.WeaponFramework.Weapons.Models;
using System.ComponentModel.DataAnnotations;

namespace P3R.WeaponFramework
{

    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase, IExports // <= Do not Remove.
    {
        public const string NAME = "P3R.WeaponFramework";
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger log;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config config;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig modConfig;

        private readonly IUnrealEssentials unrealEssentials;

        private readonly EpisodeHook episodeHook;
        private readonly DebugService debugService;
        private readonly WeaponService weapons;
        private readonly WeaponRegistry weaponRegistry;
        private readonly WeaponOverridesRegistry overridesRegistry;
        private readonly WeaponDescService weaponDescService;

        private string MainWFDirectory => modLoader.GetDirectoryForModId(modConfig.ModId);
        private List<WeaponMod> weaponMods = [];

        public Mod(ModContext context)
        {
            modLoader = context.ModLoader;
            hooks = context.Hooks!;
            log = context.Logger;
            owner = context.Owner;
            config = context.Configuration;
            modConfig = context.ModConfig;

            Project.Init(modConfig, modLoader, log, color: Color.LightGreen);
            Log.LogLevel = config.LogLevel;

            Episode vanilla = new(FEpisode.Vanilla);
            Episode astrea = new(FEpisode.Astrea);

            if (!TryGetController<IUnrealEssentials>(out var essentials, out var essentialEx))
                throw essentialEx;

            LoadUnrealComponent(essentials, UnrealComponent.Shells);

            // Init Systems
            if (!TryGetController<IStartupScanner>(out var scanner, out var scannerEx))
                throw scannerEx;

            if (!TryGetController<IUnrealEssentials>(out var unrealEssentials, out var unrealEssentialsEx))
                throw unrealEssentialsEx;
            if (!TryGetController<IUnreal>(out var unreal, out var unrealEx))
                throw unrealEx;
            if (!TryGetController<IUObjects>(out var uObjects, out var uObjectsEx))
                throw uObjectsEx;
            if (!TryGetController<IMemoryMethods>(out var memory, out var memoryEx))
                throw memoryEx;
            if (!TryGetController<IObjectMethods>(out var objectMethods, out var objectMethodsEx))
                throw objectMethodsEx;
            if (!TryGetController<IDataTables>(out var tables, out var tablesEx))
                throw tablesEx;
            if (!TryGetController<IAtlusAssets>(out var atlusAssets, out var atlusAssetsEx))
                throw atlusAssetsEx;

            this.unrealEssentials = unrealEssentials;
            this.debugService = new(unreal, tables);
            this.episodeHook = new(astrea,vanilla);
            this.weaponRegistry = new(episodeHook);
            this.overridesRegistry = new(weaponRegistry);
            this.weaponDescService = new(episodeHook,atlusAssets);
            this.weapons = new(uObjects,
                               tables,
                               unreal,
                               memory,
                               objectMethods,
                               weaponRegistry,
                               overridesRegistry,
                               weaponDescService);

            PrepLinks();
            modLoader.ModLoaded += LoadMod;
            modLoader.OnModLoaderInitialized += AllModsLoaded;
            //modLoader.OnModLoaderInitialized += this.weapons.InitShellService;

            Project.Start();
        }
        private void AllModsLoaded()
        {
            
            var mods = modLoader.GetActiveMods();
            var hasFEMC = mods.Any(mod => mod.Generic.ModId.Equals("p3rpc.femc"));
            LogHelper.Init(hasFEMC);
        }
        private void LoadMod(IModV1 mod, IModConfigV1 config)
        {
            if (!config.ModDependencies.Contains(this.modConfig.ModId))
            {
                return;
            }

            var modDir = this.modLoader.GetDirectoryForModId(config.ModId);
            var weapMod = new WeaponMod(config.ModId, modDir);
            weaponMods.Add(weapMod);

            Log.Debug($"{weaponMods.Count} Weapon Mods Active");

            Log.Debug($"Found WeaponMod || Mod: {weapMod.ModId}");

            MakeLinks(weapMod);
            //this.unrealEssentials.AddFromFolder(weapMod.UnrealDir); // Files need to be in WFs stream
            this.weaponRegistry.RegisterMod(weapMod);


            var overridesFile = Path.Join(weapMod.ConfigDir, "overrides.yaml");
            if(File.Exists(overridesFile))
            {
                this.overridesRegistry.AddOverridesFile(overridesFile);
            }
            if (this.config.TestAssets)
                LoadUnrealComponent(unrealEssentials, UnrealComponent.ModCollection);
        }


        public enum UnrealComponent
        {
            Shells,
            ModCollection,
        }
        private void LoadUnrealComponent(IUnrealEssentials essentials, UnrealComponent component)
        {
            Log.Information($"Loading {component}...");
            var modFolder = Path.Join(modLoader.GetDirectoryForModId(modConfig.ModId), "Core");
            if (modFolder == null)
                return;
            essentials.AddFromFolder(Path.Join(modFolder, $"{component}"));
            Log.Information($"Loaded {component}");
        }

        public bool TryGetController<T>([NotNullWhen(true)]out T? output, [NotNullWhen(false)] out Exception? exception)
            where T : class
        {
            var iName = typeof(T).Name;
            exception = null;
            var attempt = this.modLoader.GetController<T>().TryGetTarget(out var target);
            output = target;
            if (!attempt)
            {
                Log.Error($"{NAME} could not load the controller for {iName}.");
                exception = new NullReferenceException();
            }
            return attempt;
        }
        public void PrepLinks()
        {
            string LocalDestination(ECharacter character) => Path.Join(MainWFDirectory, "Core", "ModCollection", "P3R", "Content", "Weapons", $"{character}");
            string LocalOtherAssets = Path.Join(MainWFDirectory, "Core", "ModCollection", "P3R", "Content", "xrd777");
            foreach (var character in Characters.Armed)
            {
                var localFolder = LocalDestination(character);
                if (!Directory.Exists(localFolder))
                    Directory.CreateDirectory(localFolder);
                var folders = Directory.EnumerateDirectories(localFolder);
                foreach (var folder in folders)
                    Directory.Delete(folder, true);
            }
            if(!Directory.Exists(LocalOtherAssets))
                Directory.CreateDirectory(LocalOtherAssets);
            var otherFolders = Directory.EnumerateDirectories(LocalOtherAssets);
            foreach (var folder in otherFolders)
                Directory.Delete(folder, true);
        }
        public void MakeLinks(WeaponMod mod)
        {
            Log.Debug($"Processing {mod.ModId}");
            string ModSource(ECharacter character) => Path.Join(mod.WeaponsDir, $"{character}");
            string ModOtherAssets = mod.xrd777Dir;
            string LocalDestination(ECharacter character) => Path.Join(MainWFDirectory, "Core", "ModCollection", "P3R", "Content", "Weapons", $"{character}");
            string LocalOtherAssets = Path.Join(MainWFDirectory, "Core", "ModCollection", "P3R", "Content", "xrd777");

            foreach (var character in Characters.Armed)
            {
                var modFolder = ModSource(character);
                if (!Directory.Exists(modFolder))
                    continue;
                var localFolder = LocalDestination(character);
                var modWeaponFolders = Directory.EnumerateDirectories(modFolder);
                var count = modWeaponFolders.Count();
                Log.Debug($"{modFolder} has {count} director{(count == 1 ? "y" : "ies")}");
                if (modWeaponFolders.Any())
                    Log.Debug("Folders found.");
                string localWeaponFolder(string weapon) => Path.Join(localFolder, weapon);
                foreach (var folder in modWeaponFolders)
                {
                    var weapon = Path.GetFileName(folder);
                    if (!Directory.Exists(localWeaponFolder(weapon)))
                        Directory.CreateDirectory(localWeaponFolder(weapon));
                    var assets = Directory.EnumerateFiles(folder).Where(x => Path.GetExtension(x) == ".uasset");
                    foreach (var asset in assets)
                    {
                        var assetName = Path.GetFileName(asset);
                        var localAsset = Path.Combine(localWeaponFolder(weapon), assetName);
                        File.Copy(asset, localAsset);
                    }
                }
            }
            var otherAssetsFolder = ModOtherAssets;
            var otherAssetFiles = Directory.EnumerateFiles(otherAssetsFolder, "*.uasset", SearchOption.AllDirectories);
            if (!Directory.Exists(LocalOtherAssets))
                Directory.CreateDirectory(LocalOtherAssets);
            foreach (var otherAssetFile in otherAssetFiles)
            {
                var assetPath = Path.GetRelativePath(otherAssetsFolder, otherAssetFile);
                Log.Debug($"Making link for {assetPath}");
                var assetPathArray = assetPath.Split(Path.DirectorySeparatorChar);
                Log.Debug($"{assetPathArray.Count()} elements in relative path");

                var localPath = LocalOtherAssets;
                foreach (var element in assetPathArray)
                {
                    localPath = Path.Join(localPath, element);
                    if (!element.Contains(".uasset"))
                    {
                        if (!Directory.Exists(localPath))
                            Directory.CreateDirectory($"{localPath}");
                        continue;
                    }
                    File.Copy(otherAssetFile, localPath);
                }
            }

        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            config = configuration;
            log.WriteLine($"[{modConfig.ModId}] Config Updated: Applying");
        }

        public Type[] GetTypes() => [
            typeof(IWeaponApi),
            typeof(IWFEnum),
            typeof(ICommonMethods),
            ];
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}