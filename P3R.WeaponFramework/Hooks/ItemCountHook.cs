using P3R.WeaponFramework.Weapons;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;

namespace P3R.WeaponFramework.Hooks;

internal class ItemCountHook
{
    private const string RenasPattern = "49 89 E3 48 81 EC 88 00 00 00 48 8B 05 ?? ?? ?? ?? 48 31 E0";
    private const string FUN_1411f25a0_PAT = "40 53 48 83 EC ?? 0F B7 D9 E8 ?? ?? ?? ?? 48 85 C0 75 ?? 48 83 C4 ?? 5B C3 48 8B C8 E8 ?? ?? ?? ?? 0F B6 04 03";
    private const string FUN_1411f28f0_PAT = "48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC ?? 41 0F B6 F8 0F B6 F2 0F B7 E9 E8 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B C8 48 89 5C 24 ?? E8 ?? ?? ?? ?? 8B D7 8B CE 48 8B D8 E8 ?? ?? ?? ?? 88 44 1D";
    private const string FUN_1411e5170_PAT = "48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 41 54 41 55 41 56 41 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 44 24 ?? E8";
    [Function(CallingConventions.Microsoft)]
    private delegate int GET_WEAP_NUM(int itemId);
    private IHook<GET_WEAP_NUM>? hook;

    private readonly WeaponRegistry registry;

    public ItemCountHook(WeaponRegistry registry)
    {
        this.registry = registry;
        ScanHooks.Add(
            "GET_WEAP_NUM",
            FUN_1411f25a0_PAT,
            (hooks, result) => this.hook = hooks.CreateHook<GET_WEAP_NUM>(this.Hook, result).Activate());
    }

    private int Hook(int itemId)
    {


        if (this.registry.TryGetWeaponByItemId(itemId, out var weapon))
        {
            Log.Verbose($"Attempting to retrieve one of {weapon.Character}'s weapons with an ID of {itemId}");
            if ((weapon.Name == "Unused" || weapon.ModelId < 10))
                return 0;
            return 1;
        }

        return this.hook!.OriginalFunction(itemId);
    }
}
