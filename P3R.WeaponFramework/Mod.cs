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

        private readonly EpisodeHook episodeHook;
        private readonly WeaponService weapons;
        private readonly WeaponRegistry weaponRegistry;
        private readonly WeaponDescService weaponDescService;

        private List<Task> modQueue = new List<Task>();

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
            if (!TryGetController<IStartupScanner>(out var scanner, out var scannerEx))
                throw scannerEx;
            if (!TryGetController<IUnreal>(out var unreal, out var unrealEx))
                throw unrealEx;
            if (!TryGetController<IUObjects>(out var uObjects, out var uObjectsEx))
                throw uObjectsEx;
            if (!TryGetController<IMemoryMethods>(out var memory, out var memoryEx))
                throw memoryEx;
            if (!TryGetController<IDataTables>(out var tables, out var tablesEx))
                throw tablesEx;
            if (!TryGetController<IAtlusAssets>(out var atlusAssets, out var atlusAssetsEx))
                throw atlusAssetsEx;

            // INIT DATA //
            LoadUnrealComponent(essentials, UnrealComponent.Shells);
            // Expanding the data assets produces a crash on launch
            if (config.ExpandedDataAssets == true)
            {
                //LoadUnrealComponent(essentials, UnrealComponent.DataAssets);
            }
            this.episodeHook = new(astrea,vanilla);
            this.weaponRegistry = new(episodeHook);
            this.weaponDescService = new(episodeHook,atlusAssets);
            this.weapons = new(uObjects,
                               unreal,
                               memory,
                               weaponRegistry,
                               weaponDescService);

            modLoader.OnModLoaderInitialized += AllModsLoaded;
//            modLoader.ModLoaded += LoadMod;
            //modLoader.OnModLoaderInitialized += this.weapons.InitShellService;

            Project.Start();
        }
        private bool InterfaceNullCheck(object?[] ifaces)
        {
            List<string> messages = new List<string>();
            List<object?> nullInterfaces = [];
            foreach (var iface in ifaces)
            {
                if (iface == null)
                {
                    messages.Add(nameof(iface));
                    nullInterfaces.Add(iface);
                }
            }
            return nullInterfaces.Any();
        }

        private List<Task> ModsToLoad = new List<Task>();
        private void AllModsLoaded()
        {
            var mods = modLoader.GetActiveMods();
            foreach (var mod in mods)
            {
                LoadMod(mod.Mod, mod.Generic);
            }
        }
        private void LoadMod(IModV1 mod, IModConfigV1 config)
        {
            if (!config.ModDependencies.Contains(this.modConfig.ModId))
            {
                return;
            }

            var modDir = this.modLoader.GetDirectoryForModId(config.ModId);
            this.weaponRegistry.RegisterMod(config.ModId, modDir);
        }


        public enum UnrealComponent
        {
            Shells,
            DataAssets,
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