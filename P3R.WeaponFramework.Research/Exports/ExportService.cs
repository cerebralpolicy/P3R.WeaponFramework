using P3R.WeaponFramework.Interfaces;
using P3R.WeaponFramework.Research.Types;
using Reloaded.Mod.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Research.Exports;

internal unsafe class ExportService
{
    private readonly IModLoader _modLoader;
    public readonly IUnreal? Unreal;
    public readonly IDataTables? Tables;
    public readonly IUObjects? Objects;


    private List<Weapon> Weapons = [];

    private T? GetController<T>() where T : class
    {
        var controller = _modLoader.GetController<T>();
        if (controller == null || !controller.TryGetTarget(out var target))
        {
            Log.Error($"Unable to get controller for {nameof(T)}");
            return null;
        }
        return target;
    } 

    public ExportService(IModLoader loader)
    {
        _modLoader = loader;
        Unreal = GetController<IUnreal>();
        Tables = GetController<IDataTables>();
        Objects = GetController<IUObjects>();
    }

    public void FindObject(string name)
    {
        if (Objects == null)
            return;
        else
        {
            Objects.FindObject(name,UObjectFound);
        }
    }
    private void UObjectFound(UnrealObject obj)
    {
        if (obj.Name == WeaponItemsData)
            WeaponItemsFound(obj);
        else if (obj.Name == WeaponNamesData)
            WeaponNamesFound(obj);
        else if (obj.Name == CostumeItemsData)
            CostumeItemsFound(obj);
    }

    private void CostumeItemsFound(UnrealObject obj)
    {
        var costumeItemList = (UCostumeItemListTable*)obj.Self;
        Log.Information($"{costumeItemList->Count}");
        for (int i = 0; i < costumeItemList->Count; i++)
        {
            var costumeItem = (*costumeItemList)[i];
            var chara = costumeItem.EquipID.GetCharacter();
            Log.Debug($"Processing a costune for {Enum.GetName(chara)}");
        }
    }

    private void WeaponItemsFound(UnrealObject obj)
    {
        Log.Information($"");
        var weaponItemListTable = (UWeaponItemListTable*)obj.Self;
        var weaponItemList = weaponItemListTable->Data;
        var validWeaponItemListTable = weaponItemListTable->Where(x => x.EquipID.GetCharacter() != Character.NONE);
        Log.Information($"{WeaponItemsData} found. || {weaponItemListTable->Count} weapons found.");
    }

    private void WeaponNamesFound(UnrealObject obj)
    {
        Log.Information($"{WeaponNamesData} found");
        var itemNameListTable = (UItemNameListTable*)obj.Self;
    
    }
}
