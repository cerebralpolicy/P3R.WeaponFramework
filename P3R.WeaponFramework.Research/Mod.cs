using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Interfaces.Types;
using P3R.WeaponFramework.Research.Configuration;
using P3R.WeaponFramework.Research.Exports;
using P3R.WeaponFramework.Research.Template;
using P3R.WeaponFramework.Research.Types;
using Project.Utils;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Nodes;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Research
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader ModLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? Hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger Logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod Owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config Config;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig ModConfig;

        private readonly ExportService ExportService;
        
        public Mod(ModContext context)
        {
            ModLoader = context.ModLoader;
            Hooks = context.Hooks;
            Logger = context.Logger;
            Owner = context.Owner;
            Config = context.Configuration;
            ModConfig = context.ModConfig;

            Log.Initialize(ModConfig.ModName, Logger, Color.White);
            Log.LogLevel = LogLevel.Verbose;
            ExportService = new(ModLoader);

            ExportService.FindObject(CostumeItemsData);
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            Config = configuration;
            Logger.WriteLine($"[{ModConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}