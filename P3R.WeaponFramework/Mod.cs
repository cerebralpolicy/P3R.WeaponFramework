using P3R.WeaponFramework.Configuration;
using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Template;
using P3R.WeaponFramework.Weapons;
using p3rpc.classconstructor.Interfaces;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;
using SharedScans.Interfaces;
using System.Diagnostics;
using System.Drawing;
using Unreal.AtlusScript.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;

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

        private readonly Core core;
        private readonly WeaponService weapons;
        private readonly WeaponRegistry weaponRegistry;
        private readonly WeaponDescService weaponDescService;

        public Mod(ModContext context)
        {
            modLoader = context.ModLoader;
            hooks = context.Hooks!;
            log = context.Logger;
            owner = context.Owner;
            config = context.Configuration;
            modConfig = context.ModConfig;


            var mainModule = Process.GetCurrentProcess().MainModule ?? throw new Exception($"[{modConfig.ModId}] Could not get main module");
            this.modLoader.GetController<IStartupScanner>().TryGetTarget(out var scanner);
            if (scanner == null) throw new Exception($"[{modConfig.ModId}] Could not get controller for Startup Scans");
            this.modLoader.GetController<ISharedScans>().TryGetTarget(out var sharedScans);
            if (sharedScans == null) throw new Exception($"[{modConfig.ModId}] Could not get controller for Shared Scans");
            this.modLoader.GetController<IMemoryMethods>().TryGetTarget(out var memoryMethods);
            if (memoryMethods == null) throw new Exception($"[{modConfig.ModId}] Could not get controller for Memory Methods");
            this.modLoader.GetController<IClassMethods>().TryGetTarget(out var classMethods);
            if (classMethods == null) throw new Exception($"[{modConfig.ModId}] Could not get controller for Class Methods");
            this.modLoader.GetController<IObjectMethods>().TryGetTarget(out var objectMethods);
            if (objectMethods == null) throw new Exception($"[{modConfig.ModId}] Could not get controller for Object Methods");
            this.modLoader.GetController<IUnreal>().TryGetTarget(out var unreal);
            if (unreal == null) throw new Exception($"[{modConfig.ModId}] Could not get controller for Unreal Object Emitter");
            this.modLoader.GetController<IAtlusAssets>().TryGetTarget(out var atlusAssets);
            if (atlusAssets == null) throw new Exception($"[{modConfig.ModId}] Could not get controller for Atlus Script Assets");
            var utils = new Utils(scanner, log, hooks, mainModule.BaseAddress, modConfig.ModId, Color.AliceBlue, config.LogLevel);
            Memory memory = new();

            core = new(mainModule.BaseAddress, modLoader.GetDirectoryForModId(modConfig.ModId), config, log, scanner, hooks, utils, memory, sharedScans, classMethods, objectMethods, memoryMethods, atlusAssets, unreal);

            this.weaponRegistry = new(core);
            this.weaponDescService = new(atlusAssets);
            this.weapons = new(core, weaponRegistry, weaponDescService);

     

            modLoader.ModLoaded += OnModLoaded;
        }

        private void OnModLoaded(IModV1 mod, IModConfigV1 config)
        {
            if (!config.ModDependencies.Contains(this.modConfig.ModId))
            {
                return;
            }

            var modDir = this.modLoader.GetDirectoryForModId(config.ModId);
            this.weaponRegistry.RegisterMod(config.ModId, modDir);
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            config = configuration;
            log.WriteLine($"[{modConfig.ModId}] Config Updated: Applying");
        }

        public Type[] GetTypes() => [typeof(IWeaponApi)];
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}