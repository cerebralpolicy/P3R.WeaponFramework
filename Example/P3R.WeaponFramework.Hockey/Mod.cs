using P3R.WeaponFramework.Hockey.Template;
using Project.Utils;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System.Drawing;
using UnrealEssentials.Interfaces;

namespace P3R.WeaponFramework.Hockey
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        private readonly IUnrealEssentials? unrealEssentials;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _modConfig = context.ModConfig;


            Log.Initialize(_modConfig.ModName, _logger, Color.White);
            Log.LogLevel = LogLevel.Verbose;

            var unrealEssentialsController = _modLoader.GetController<IUnrealEssentials>();
            if (unrealEssentialsController == null || !unrealEssentialsController.TryGetTarget(out var unrealEssentials))
            {
                Log.Error("Unable to get controller for Unreal Essentials.");
                return;
            }
            this.unrealEssentials = unrealEssentials;

        }


        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}