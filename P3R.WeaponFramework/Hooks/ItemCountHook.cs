using P3R.WeaponFramework.Weapons;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;

namespace P3R.WeaponFramework.Hooks;

internal class ItemCountHook
{
    [Function(CallingConventions.Microsoft)]
    private delegate int FUN_14c15cad0(int itemId);
    private IHook<FUN_14c15cad0>? hook;

    private readonly WeaponRegistry registry;

    public ItemCountHook(WeaponRegistry registry)
    {
        this.registry = registry;
        ScanHooks.Add(
            "GET_ITEM_NUM",
            "49 89 E3 48 81 EC 88 00 00 00 48 8B 05 ?? ?? ?? ?? 48 31 E0",
            (hooks, result) => this.hook = hooks.CreateHook<FUN_14c15cad0>(this.Hook, result).Activate());
    }

    private int Hook(int itemId)
    {
        if (this.registry.TryGetWeaponByItemId(itemId, out var weapon))
        {
            if (weapon.Name == "Unused" || weapon.ModelId < 10)
                return 0;
            return 1;
        }

        return this.hook!.OriginalFunction(itemId);
    }
}
