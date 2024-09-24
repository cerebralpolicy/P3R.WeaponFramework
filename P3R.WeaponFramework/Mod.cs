using Ardalis.SmartEnum;
using P3R.WeaponFramework.Configuration;
using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Template;
using P3R.WeaponFramework.Weapons;
using p3rpc.classconstructor.Interfaces;
using p3rpc.nativetypes.Interfaces;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;
using System.Diagnostics;
using System.Drawing;
using Unreal.AtlusScript.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;
using UnrealEssentials.Interfaces;
using YamlDotNet.Serialization;

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

            

            Project.Init(modConfig, modLoader, log);
            this.modLoader.GetController<IUnrealEssentials>().TryGetTarget(out var essentials);
            this.modLoader.GetController<IStartupScanner>().TryGetTarget(out var scanner);
            this.modLoader.GetController<IUnreal>().TryGetTarget(out var unreal);
            this.modLoader.GetController<IUObjects>().TryGetTarget(out var uobjects);
            this.modLoader.GetController<IDataTables>().TryGetTarget(out var tables);
            this.modLoader.GetController<IMemoryMethods>().TryGetTarget(out var memory);
            this.modLoader.GetController<IObjectMethods>().TryGetTarget(out var objectMethods);
            this.modLoader.GetController<IAtlusAssets>().TryGetTarget(out var atlusAssets);

            //if (essentials == null) throw new NullReferenceException(nameof(essentials));
            // INIT DATA //
            LoadUnrealComponent(essentials!, UnrealComponent.Data);
            // INIT BP //
            if (config.BPFlow)
                LoadUnrealComponent(essentials!, UnrealComponent.Blueprints);
            this.weaponRegistry = new();
            this.weaponDescService = new(atlusAssets!);
            this.weapons = new(tables!, uobjects!, unreal!, weaponRegistry, weaponDescService, config.BPFlow);

            modLoader.ModLoaded += OnModLoaded;
            Project.Start();
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


        public enum UnrealComponent
        {
            Data,
            Blueprints
        }
        private void LoadUnrealComponent(IUnrealEssentials essentials, UnrealComponent component)
        {
            Log.Information($"Loading {component}...");
            var modFolder = Path.Join(modLoader.GetDirectoryForModId(modConfig.ModId),"Core");
            if (modFolder == null)
                return;
            essentials.AddFromFolder(Path.Join(modFolder, $"{component}"));
            Log.Information($"Loaded {component}");
        }


        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            config = configuration;
            weapons.UpdateMode(configuration.BPFlow);
            log.WriteLine($"[{modConfig.ModId}] Config Updated: Applying");
        }

        public Type[] GetTypes() => [
            typeof(IWeaponApi),
            typeof(ISmartEnum),
            typeof(IYamlConvertible)
            ];
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}