//using P3R.WeaponFramework.Interfaces.Types;
using P3R.WeaponFramework.Search.Configuration;
using P3R.WeaponFramework.Search.Template;
using p3rpc.classconstructor.Interfaces;
using Native = p3rpc.nativetypes.Interfaces;
using Project.Utils;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System.Drawing;
using Unreal.ObjectsEmitter.Interfaces;
using Emit = Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Search
{
    public enum Character
    {
        NONE,
        Player,
        Yukari,
        Stupei,
        Akihiko,
        Mitsuru,
        Fuuka,
        Aigis,
        Ken,
        Koromaru,
        Shinjiro,
        Metis,
        AigisEpisode,
    }
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
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
        private readonly ILogger logger;

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
        private readonly IUnreal unreal;
        private readonly IUObjects uObjects;
        private readonly IClassMethods classMethods;
        private readonly IObjectMethods objectMethods;

        public Mod(ModContext context)
        {
            modLoader = context.ModLoader;
            hooks = context.Hooks;
            logger = context.Logger;
            owner = context.Owner;
            config = context.Configuration;
            modConfig = context.ModConfig;

            Log.Initialize("WEAPON RESEARCH", logger, Color.PaleVioletRed);
            Log.LogLevel = config.LogLevel;
            
            this.modLoader.GetController<IUObjects>().TryGetTarget(out var uobjects);
            if (uobjects == null)
            {
                throw new ArgumentNullException(nameof(uobjects));
            }
            this.uObjects = uobjects;

            this.modLoader.GetController<IUnreal>().TryGetTarget(out var unreal);
            if (unreal == null)
            {
                throw new ArgumentNullException(nameof(unreal));
            }
            this.unreal = unreal;

            this.modLoader.GetController<IClassMethods>().TryGetTarget(out var classMethods);
            if (classMethods == null)
            {
                throw new ArgumentNullException(nameof(classMethods));
            }
            this.classMethods = classMethods;

            this.modLoader.GetController<IObjectMethods>().TryGetTarget(out var objectMethods);
            if (objectMethods == null)
            {
                throw new ArgumentNullException(nameof(objectMethods));
            }
            this.objectMethods = objectMethods;


         
            modLoader.OnModLoaderInitialized += loaded;
            // For more information about this template, please see
            // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

            // If you want to implement e.g. unload support in your mod,
            // and some other neat features, override the methods in ModBase.

            // TODO: Implement some mod logic
        }

        private void loaded()
        {
            Log.Debug("All mods loaded.");
            var characters = Enum.GetValuesAsUnderlyingType<Character>()
                .Cast<int>().Where(
                chara => chara > (int)Character.NONE &&
                chara < (int)Character.Metis &&
                chara != (int)Character.Fuuka);

            string[] bpLookups = [];
            foreach (var character in characters)
            {
                bpLookups.Append($"BP_Wp{character:0000}_C");
            }
            foreach (var bp in bpLookups)
            {
                uObjects.FindObject(bp, bpFound);
                //                objectMethods.FindObjectAsync(bp, onBPFound);
            }

        }

        private unsafe void allCharWeaponBases(ICollection<nint> collection)
        {
            Log.Debug("Searching for weapon bases.");
            foreach (var ptr in collection)
            {
                Log.Debug(ptr.ToString());
            }
/*            foreach (var ptr in collection)
            {
                var obj = (Native.UObject*)ptr;
                var name = objectMethods.GetFullName(obj);
                Log.Debug($"OBJECT METHODS: {nameof(AAppCharWeaponBase)} {name} found.");
            }*/
        }

/*        private unsafe void onBPFound(nint ptr)
        {
            var obj = (Native.UObject*)ptr;
            var name = objectMethods.GetFullName(obj);
            Log.Debug($"OBJECT METHODS: {name} found.");
        }
*/
        private unsafe void bpFound(Emit.UnrealObject bpObject)
        {
            Log.Debug($"OBJECTS EMITTER: {bpObject.Name} found.");
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            this.config = configuration;
            logger.WriteLine($"[{modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}