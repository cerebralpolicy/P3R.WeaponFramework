using P3R.WeaponFramework.Hooks;
using P3R.WeaponFramework.Hooks.Services;
using P3R.WeaponFramework.Hooks.Weapons;
using p3rpc.classconstructor.Interfaces;
using System.Reflection;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Weapons;


internal unsafe interface IWeaponService
{
    public Action InitShellService { get; set; }
}
internal unsafe class WeaponService: IWeaponService
{

    private readonly WeaponRedirectService redirectService;
    private readonly WeaponHooks weaponHooks;
    private readonly ItemCountHook itemCountHook;
    private readonly WeaponNameHook weaponNameHook;
    private readonly ItemEquipHooks itemEquipHooks;

    public WeaponService(
        IUObjects uObjects,
        IDataTables dataTables,
        IUnreal unreal,
        IMemoryMethods memoryMethods,
        IObjectMethods objectMethods,

        WeaponRegistry registry,
        WeaponOverridesRegistry overrides,
        WeaponDescService weaponDesc)
    {
        redirectService = new(unreal, uObjects, memoryMethods, objectMethods, registry, overrides);
        itemEquipHooks = new(registry);
        weaponHooks = new(unreal,
                          uObjects,
                          memoryMethods,
                          registry,
                          overrides,
                          weaponDesc,
                          redirectService,
                          itemEquipHooks);
        itemCountHook = new(registry);
        weaponNameHook = new(uObjects, unreal, registry, overrides);
    }

    public Action InitShellService { get; set; }
}