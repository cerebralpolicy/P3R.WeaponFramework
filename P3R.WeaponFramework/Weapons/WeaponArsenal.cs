using P3R.WeaponFramework.Utils;
using P3R.WeaponFramework.Weapons.Models;
using System.Threading;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Weapons
{
    internal class WeaponArsenal
    {

        private readonly GameWeapons weapons;

   

        public WeaponArsenal(GameWeapons weapons)
        {
            this.weapons = weapons;
        }


        public Weapon? Create(WeaponMod mod, string weaponDir, ECharacter character)
        {
            int step = 1;
            void OutputStep(string message)
            {
                Log.Debug($"[{step}] {message}");
                step++;
            }
            OutputStep("Reading config");
            var config = GetWeaponConfig(weaponDir);
            OutputStep("Creating weapon");
            var weapon = CreateOrFindWeapon(mod.ModId, character, config.Shell, config.Name ?? Path.GetFileName(weaponDir));
            if (weapon == null)
            {
                return null;
            }
            OutputStep("Applying config");
            ApplyWeaponConfig(weapon, config);
            OutputStep("Loading files");
            LoadWeaponFiles(mod, weapon, weaponDir);
            Log.Information($"Weapon created: {weapon.Character} || {weapon.Name} || Weapon ID: {weapon.WeaponItemId}\nFolder: {weaponDir}\nStats: {weapon.Stats.Attack} ATK, {weapon.Stats.Accuracy} ACC\nShell Target: {weapon.ShellTarget}\nDescription: {weapon.Description}");
            if (!weapon.IsEnabled)
                weapon.IsEnabled = true;
            return weapon;
        }
        private void ApplyWeaponConfig(Weapon weapon, WeaponConfig config)
        {
            ModUtils.IfNotNull(config.Name, str => weapon.Name = str);
            ModUtils.IfNotNull(config.Model, model => weapon.Config.Model = model);
            ModUtils.IfNotNull(config.Stats, stats =>
            {
                
                if (stats is null)
                {
                    return;
                }
                Log.Debug($"{stats.Summarize()}");
                weapon.Stats.AttrId = stats.AttrId;
                weapon.Stats.Tier = stats.Tier;
                weapon.Stats.Rarity = stats.Rarity;
                weapon.Stats.Attack = stats.Attack;
                weapon.Stats.Accuracy = stats.Accuracy;
                weapon.Stats.Strength = stats.Strength;
                weapon.Stats.Magic = stats.Magic;
                weapon.Stats.Endurance = stats.Endurance;
                weapon.Stats.Agility = stats.Agility;
                weapon.Stats.Luck = stats.Luck;
                weapon.Stats.SkillId = stats.SkillId;
                weapon.Stats.Price = stats.Price;
                weapon.Stats.SellPrice = stats.SellPrice;
                weapon.VerifyPrices();
            });
            ModUtils.IfNotNull(config.Shell, shell =>
            {
                weapon.Config.Shell = shell;
                weapon.ShellTarget = shell;
                weapon.ModelId = (int)shell;
            });
        }

        private void LoadWeaponFiles(WeaponMod mod, Weapon weapon, string weaponDir)
        {

            // BASEMESH WILL ALWAYS BE A REF
            //            SetWeaponFile(mod, Path.Join(weaponDir, "base-mesh.uasset"), path => weapon.Config.Base.MeshPath = path);
            //            SetWeaponFile(mod, Path.Join(weaponDir, "base-anim.uasset"), path => weapon.Config.Base.MeshPath = path);

            if (weapon.Config.HasMultipleModels.GetValueOrDefault(false))
                SetWeaponFile(mod, Path.Join(weaponDir, "weapon-mesh2.uasset"), path => weapon.Config.Model!.MeshPath2 = path);
            SetWeaponFile(mod, Path.Join(weaponDir, "weapon-mesh.uasset"), path => weapon.Config.Model!.MeshPath1 = path);
            SetWeaponFile(mod, Path.Join(weaponDir, "description.msg"), path => weapon.Description = File.ReadAllText(path), SetType.Full);
        }
        private static void SetWeaponFile(WeaponMod mod, string modFile, Action<string> setFile, SetType type = SetType.Relative)
        {
            if (File.Exists(modFile))
            {
                if (type == SetType.Relative)
                {
                    setFile(Path.GetRelativePath(mod.ContentDir, modFile));
                }
                else
                {
                    setFile(modFile);
                }
            }
        }
        public Weapon? CreateOrFindWeapon(string ownerId, ECharacter character, ShellType shellType, string name)
        {
            _ = weapons.TryGetFirstWeaponOfPredicate(x => x.Character == character && x.Name == name, out var existingWeapon);
            if (existingWeapon != null)
            {
                return existingWeapon;
            }


            _ = weapons.TryGetFirstAssignableWeapon(out var newWeapon);
            
            if (newWeapon != null)
            {
                newWeapon.Name = name;
                newWeapon.Character = character;
                newWeapon.IsVanilla = true;
                newWeapon.IsAstrea = true;
                newWeapon.IsEnabled = true;
                newWeapon.ShellTarget = shellType;
                newWeapon.ModelId = shellType.ModelId();
                newWeapon.OwnerModId = ownerId;
                return newWeapon;
            }
            else
            {
                Log.Warning("No new weapon slots available.");
            }
            return newWeapon;
        }
        private static WeaponConfig GetWeaponConfig(string weaponDir)
        {
            var configFile = Path.Join(weaponDir, "config.yaml");
            //Log.Debug($"Reading {configFile}");
            if (File.Exists(configFile))
            {
                return YamlSerializer.DeserializeFile<WeaponConfig>(configFile);
            }
            Log.Error($"Failed to parse {configFile}");
            return new();
        }
        private enum SetType
        {
            Relative,
            Full,
        }
    }
}
public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
{
    // Indicates whether the current thread is processing work items.
    [ThreadStatic]
    private static bool _currentThreadIsProcessingItems;

    // The list of tasks to be executed
    private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks)

    // The maximum concurrency level allowed by this scheduler.
    private readonly int _maxDegreeOfParallelism;

    // Indicates whether the scheduler is currently processing work items.
    private int _delegatesQueuedOrRunning = 0;

    // Creates a new instance with the specified degree of parallelism.
    public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
    {
        if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
        _maxDegreeOfParallelism = maxDegreeOfParallelism;
    }

    // Queues a task to the scheduler.
    protected sealed override void QueueTask(Task task)
    {
        // Add the task to the list of tasks to be processed.  If there aren't enough
        // delegates currently queued or running to process tasks, schedule another.
        lock (_tasks)
        {
            _tasks.AddLast(task);
            if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
            {
                ++_delegatesQueuedOrRunning;
                NotifyThreadPoolOfPendingWork();
            }
        }
    }

    // Inform the ThreadPool that there's work to be executed for this scheduler.
    private void NotifyThreadPoolOfPendingWork()
    {
        ThreadPool.UnsafeQueueUserWorkItem(_ =>
        {
            // Note that the current thread is now processing work items.
            // This is necessary to enable inlining of tasks into this thread.
            _currentThreadIsProcessingItems = true;
            try
            {
                // Process all available items in the queue.
                while (true)
                {
                    Task item;
                    lock (_tasks)
                    {
                        // When there are no more items to be processed,
                        // note that we're done processing, and get out.
                        if (_tasks.Count == 0)
                        {
                            --_delegatesQueuedOrRunning;
                            break;
                        }

                        // Get the next item from the queue
                        item = _tasks.First.Value;
                        _tasks.RemoveFirst();
                    }

                    // Execute the task we pulled out of the queue
                    base.TryExecuteTask(item);
                }
            }
            // We're done processing items on the current thread
            finally { _currentThreadIsProcessingItems = false; }
        }, null);
    }

    // Attempts to execute the specified task on the current thread.
    protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        // If this thread isn't already processing a task, we don't support inlining
        if (!_currentThreadIsProcessingItems) return false;

        // If the task was previously queued, remove it from the queue
        if (taskWasPreviouslyQueued)
            // Try to run the task.
            if (TryDequeue(task))
                return base.TryExecuteTask(task);
            else
                return false;
        else
            return base.TryExecuteTask(task);
    }

    // Attempt to remove a previously scheduled task from the scheduler.
    protected sealed override bool TryDequeue(Task task)
    {
        lock (_tasks) return _tasks.Remove(task);
    }

    // Gets the maximum concurrency level supported by this scheduler.
    public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

    // Gets an enumerable of the tasks currently scheduled on this scheduler.
    protected sealed override IEnumerable<Task> GetScheduledTasks()
    {
        bool lockTaken = false;
        try
        {
            Monitor.TryEnter(_tasks, ref lockTaken);
            if (lockTaken) return _tasks;
            else throw new NotSupportedException();
        }
        finally
        {
            if (lockTaken) Monitor.Exit(_tasks);
        }
    }
}