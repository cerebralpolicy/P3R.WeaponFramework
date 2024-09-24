using P3R.WeaponFramework.Hooks;
using P3R.WeaponFramework.Hooks.Services;
using P3R.WeaponFramework.Interfaces;
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
        IDataTables dt,
        IUObjects uObjects,
        IUnreal unreal,
        WeaponRegistry registry,
        WeaponDescService weaponDesc,
        bool useAdvanced)
    {

        shellService = new(dt, uObjects, unreal, registry, useAdvanced);
        itemEquipHooks = new(registry);
        weaponHooks = new(unreal, uObjects, registry, weaponDesc, shellService, itemEquipHooks);
        itemCountHook = new(registry);
        weaponNameHook = new(uObjects, unreal, registry);
    }

    public void UpdateMode(bool useAdvanced)
    {
        shellService.UseAdvanced = useAdvanced;
    }
}