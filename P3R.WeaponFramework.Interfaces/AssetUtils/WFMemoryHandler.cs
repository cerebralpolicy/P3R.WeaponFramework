using P3R.WeaponFramework.Interfaces.Types;
using p3rpc.classconstructor.Interfaces;
using System.Drawing;
using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Interfaces;

public static unsafe class WFMemoryHandler
{
    private static IUnreal? Unreal;
    private static IDataTables? DataTables;
    private static IMemoryMethods? MemoryMethods;
    private static IObjectMethods? ObjectMethods;
    public static void InitHandler(IUnreal unreal, IDataTables dataTables, IMemoryMethods memoryMethods, IObjectMethods objectMethods)
    {
        Unreal = unreal;
        DataTables = dataTables;
        MemoryMethods = memoryMethods;
        ObjectMethods = objectMethods;
    }

    private static UDataTable* FindWFTable(string name)
    {
        if (DataTables == null)
            throw new NullReferenceException(nameof(DataTables));
        DataTables.TryGetDataTable(name, out var table);
        if (table == null)
            throw new NullReferenceException(nameof(table));
        var natTab = (UDataTable*)table.Self;
        return natTab;
    }

    public static unsafe TSoftObjectPtr<TType>* GetSoftPointer<TType>(string path) where TType : unmanaged
    {
        if (ObjectMethods == null)
            throw new NullReferenceException(nameof(ObjectMethods));
        var name = path.Split('/').Last();
        var obj = ObjectMethods.FindObject(name, "USkeletalMesh");
        TSoftObjectPtr<TType> newSoftPtr = new((TType*)obj);
        return &newSoftPtr;
    }

    public static unsafe bool TMap_Insert<TKey,TValue>(string str, TKey key, TValue value, string? tableName = null)
        where TKey : unmanaged, IEquatable<TKey>
        where TValue : unmanaged
    {
        if (MemoryMethods == null)
            throw new NullReferenceException(nameof(MemoryMethods));
        if (ObjectMethods == null)
            throw new NullReferenceException(nameof(ObjectMethods));
        var fname = ObjectMethods.GetFName($"BP_{str}");
        var table = FindWFTable(tableName ?? $"DT_{str}");
        var dtElements = (TMap<Native.FName,TMap<TKey,TValue>>*)&table->RowMap;
        var bpMap = dtElements->TryGet(fname);
        return MemoryMethods.TMap_Insert(bpMap, key, value);
    }
    public static unsafe Types.FString MakeFString(this string str)
    {
        if (Unreal == null)
            throw new NullReferenceException(nameof(Unreal));
        return new Types.FString(Unreal, str);
    }
    public static unsafe Types.FName* MakeFName(this string str)
    {
        if (Unreal == null)
            throw new NullReferenceException(nameof(Unreal));
        return (Types.FName*)Unreal.FName(str);
    }
    public static unsafe TType* WFMemory_Malloc<TType>(uint alignment) where TType : unmanaged => (TType*)WFMemory_Malloc(sizeof(TType), alignment);
    public static unsafe TType* WFMemory_Malloc<TType>() where TType : unmanaged => (TType*)WFMemory_Malloc(sizeof(TType), (uint)sizeof(nint));
    public static unsafe nint WFMemory_Malloc(nint size, uint alignment)
    {
        if (MemoryMethods == null)
            throw new NullReferenceException(nameof(MemoryMethods));
        return MemoryMethods.FMemory_Malloc(size, alignment);
    }
    public static unsafe TType* WFMemory_Realloc<TType>(TType* obj, nint size, uint alignment) where TType : unmanaged => (TType*)WFMemory_Realloc((nint)obj, size, alignment);
    public static unsafe TType* WFMemory_Realloc<TType>(TType* obj, uint alignment) where TType : unmanaged => (TType*)WFMemory_Realloc((nint)obj, sizeof(TType), alignment);
    public static unsafe TType* WFMemory_Realloc<TType>(TType* obj) where TType : unmanaged => (TType*)WFMemory_Realloc((nint)obj, sizeof(TType), (uint)sizeof(nint));
    public static unsafe nint WFMemory_Realloc(nint ptr, nint size, uint alignment)
    {
        if (MemoryMethods == null)
            throw new NullReferenceException(nameof(MemoryMethods));
        return MemoryMethods.FMemory_Realloc(ptr, size, alignment);
    }
    public static unsafe nint WFMemory_GetAllocSize<TType>(TType* objPtr) where TType : unmanaged => WFMemory_GetAllocSize((nint)objPtr);
    public static unsafe nint WFMemory_GetAllocSize(nint ptr)
    {
        if (MemoryMethods == null)
            throw new NullReferenceException(nameof(MemoryMethods));
        return MemoryMethods.FMemory_GetAllocSize(ptr);
    }
}
