using P3R.WeaponFramework.Hooks;
using P3R.WeaponFramework.Hooks.Services;
using System.Reflection;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Weapons;

internal unsafe class WeaponService
{

    private readonly WeaponShellService shellService;
    private readonly WeaponHooks weaponHooks;
    private readonly ItemCountHook itemCountHook;
    private readonly WeaponNameHook weaponNameHook;
    private readonly ItemEquipHooks itemEquipHooks;

    public WeaponService(
        IUObjects uObjects,
        IUnreal unreal,
        IMemoryMethods memoryMethods,
        WeaponRegistry registry,
        WeaponDescService weaponDesc)
    {

        shellService = new(unreal, registry);
        itemEquipHooks = new(registry);
        weaponHooks = new(unreal,
                          uObjects,
                          memoryMethods,
                          registry,
                          weaponDesc,
                          shellService,
                          itemEquipHooks);
        itemCountHook = new(registry);
        weaponNameHook = new(uObjects, unreal, registry);
    }
}