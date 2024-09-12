using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Interfaces.Types;
public abstract class WFEnum<TEnum, TValue, TRow> : WFEnumBase<TEnum, TValue>
    where TEnum : WFEnumBase<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
    where TRow : unmanaged
{
    protected WFEnum(string name, TValue value) : base(name, value)
    {
        //var id = Enum.GetName(typeof(TValue), value);
    }
/*
    public unsafe bool AddMesh(int id, string meshPath, bool multi, string? tableName = null)
    {
        // Create a new soft object pointer
        var tSoftObj = memAPI.GetSoftPointer<UObject>(meshPath);
        var meshData = new FAppCharWeaponMeshData(tSoftObj, multi);
        return AddMesh(id, meshData, tableName ?? Name);
    }
    public unsafe bool AddMesh(int id, FAppCharWeaponMeshData meshData, string tableName)
        => memAPI.TMap_Insert(tableName, id, meshData, tableName);*/
}
public class WFEnumCollection<TValue> : Collection<TValue>, IEquatable<WFEnumCollection<TValue>>, IComparable<WFEnumCollection<TValue>>
    where TValue : SmartEnum<TValue, int>
{
    public WFEnumCollection()
    {
    }

    public WFEnumCollection(IList<TValue> list) : base(list)
    {
    }

    public int CompareTo(WFEnumCollection<TValue>? other)
    {
        if (other == null)
            return 0;
        return object.ReferenceEquals(other, this) ? 1 : -1;
    }

    public bool Equals(WFEnumCollection<TValue>? other)
    {
        return object.ReferenceEquals(other, this);
    }
}

public abstract class WFEnumBase<TEnum, TValue> : SmartEnum<TEnum, TValue>
        where TEnum : SmartEnum<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
{
    protected WFEnumBase(string name, TValue value) : base(name, value)
    {
    }

    internal static string GetAssetPath(string assetFile)
    {
        var adjustedPath = assetFile.Replace('\\', '/').Replace(".uasset", string.Empty);

        if (adjustedPath.IndexOf("Content") is int contentIndex && contentIndex > -1)
        {
            adjustedPath = adjustedPath.Substring(contentIndex + 8);
        }

        if (!adjustedPath.StartsWith("/Game/"))
        {
            adjustedPath = $"/Game/{adjustedPath}";
        }
        return adjustedPath;
    }
}
