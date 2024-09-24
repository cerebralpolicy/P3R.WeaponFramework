using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Interfaces.Types;


public abstract class WFEnumBase<TEnum, TValue> : SmartEnum<TEnum, TValue>, IEquatable<TEnum>, IComparable<TEnum>
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

    public abstract int CompareTo(TEnum? other);
    public abstract bool Equals(TEnum? other);
}