using P3R.WeaponFramework.Hooks;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Weapons;

internal unsafe class WeaponService
{
    private readonly WeaponHooks weaponHooks;
    private readonly ItemCountHook itemCountHook;
    private readonly WeaponNameHook weaponNameHook;
    private readonly ItemEquipHooks itemEquipHooks;

    public WeaponService(
        Core core,
        WeaponRegistry registry,
        WeaponDescService weaponDesc)
    {
        itemEquipHooks = new();
        weaponHooks = new(core, registry, weaponDesc, itemEquipHooks);
        itemCountHook = new(registry);
        weaponNameHook = new(core, registry);
    }
}
