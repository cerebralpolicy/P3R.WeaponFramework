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
        IUObjects uObjects,
        IUnreal unreal,
        WeaponRegistry registry,
        WeaponDescService weaponDesc)
    {
        itemEquipHooks = new();
        weaponHooks = new(unreal, uObjects, registry, weaponDesc, itemEquipHooks);
        itemCountHook = new(registry);
        weaponNameHook = new(uObjects, registry);
    }
}
