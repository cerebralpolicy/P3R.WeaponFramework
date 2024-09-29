using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Numerics;
using static P3R.WeaponFramework.Enums.TypeExtensions;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace P3R.WeaponFramework.Enums;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8625 // Converting null literal or possible null value to non-nullable type.

public abstract class WFEnumBase<TEnum,TValue> : IWFEnum, IEquatable<WFEnumBase<TEnum, TValue>>,  IComparable<WFEnumBase<TEnum, TValue>>
    where TEnum : WFEnumBase<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    static readonly Lazy<TEnum[]> _enums = new Lazy<TEnum[]>(GetAllOptions, LazyThreadSafetyMode.ExecutionAndPublication);
    static readonly Lazy<Dictionary<string, TEnum>> _fromName =
           new(() => _enums.Value.ToDictionary(item => item.Name));

    static readonly Lazy<Dictionary<string, TEnum>> _fromNameIgnoreCase =
        new(() => _enums.Value.ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));

    static readonly Lazy<Dictionary<TValue, TEnum>> _fromValue =
        new(() =>
        {
            // multiple enums with same value are allowed but store only one per value
            var dictionary = new Dictionary<TValue, TEnum>(GetValueComparer());
            foreach (var item in _enums.Value)
            {
                if (item._value != null && !dictionary.ContainsKey(item._value))
                    dictionary.Add(item._value, item);
            }
            return dictionary;
        });
    
    private static IEqualityComparer<TValue> GetValueComparer()
    {
        var comparer = typeof(TEnum).GetCustomAttribute<WFEnumComparerAttribute<TValue>>();
        if (comparer == null)
            throw new InvalidOperationException(nameof(comparer));
        return comparer.Comparer;
    }

    private static TEnum[] GetAllOptions()
    {
        Type baseType = typeof(TEnum);
        return Assembly.GetAssembly(baseType)!
                       .GetTypes()
                       .Where(t => baseType.IsAssignableFrom(t))
                       .SelectMany(t => t.GetFieldsOfType<TEnum>())
                       .OrderBy(t => t.Name)
                       .ToArray();
    }
    public static IReadOnlyCollection<TEnum> List => _enums.Value;

    private readonly string _name;
    private readonly TValue _value;

    public string Name => _name;
    public TValue Value => _value;

    protected WFEnumBase(string name, TValue value)
    {
        _name = name;
        _value = value;
    }
    public static TEnum FromName(string name, bool ignoreCase = false)
    {
        if (String.IsNullOrEmpty(name))
            ThrowHelper.ThrowArgumentNullOrEmptyException(name);

        if (ignoreCase)
            return FromName(_fromNameIgnoreCase.Value);
        else
            return FromName(_fromName.Value);

        TEnum FromName(Dictionary<string, TEnum> dictionary)
        {
            if (!dictionary.TryGetValue(name, out var result))
            {
                ThrowHelper.ThrowNameNotFoundException<TEnum,TValue>(name);
            }
            return result;
        }
    }
    public static TEnum FromValue(TValue value)
    {
        TEnum result;

        if (value != null)
        {
            if (!_fromValue.Value.TryGetValue(value, out result))
            {
                ThrowHelper.ThrowValueNotFoundException<TEnum, TValue>(value);
            }
        }
        else
        {
            result = _enums.Value.FirstOrDefault(x => x.Value == null);
            if (result == null)
            {
                ThrowHelper.ThrowValueNotFoundException<TEnum, TValue>(value);
            }
        }
        return result;
    }

    public static bool TryFromValue(TValue value, out TEnum? result)
    {
        if (value == null)
        {
            result = default;
            return false;
        }

        return _fromValue.Value.TryGetValue(value, out result);
    }
    public override bool Equals(object? obj) =>
        (obj is WFEnumBase<TEnum, TValue> other && Equals(other));

    public override int GetHashCode() => _value.GetHashCode();

    public bool Equals(WFEnumBase<TEnum, TValue>? other)
    {
        // check if same instance
        if (Object.ReferenceEquals(this, other))
            return true;

        // it's not same instance so
        // check if it's not null and is same value
        if (other is null)
            return false;

        return _value.Equals(other._value);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual int CompareTo(WFEnumBase<TEnum, TValue>? other)
        => _value.CompareTo(other!._value);

    public static bool operator ==(WFEnumBase<TEnum, TValue> left, WFEnumBase<TEnum, TValue> right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(WFEnumBase<TEnum, TValue> left, WFEnumBase<TEnum, TValue> right)
    {
        return !(left == right);
    }

    public static bool operator <(WFEnumBase<TEnum, TValue> left, WFEnumBase<TEnum, TValue> right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    public static bool operator <=(WFEnumBase<TEnum, TValue> left, WFEnumBase<TEnum, TValue> right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >(WFEnumBase<TEnum, TValue> left, WFEnumBase<TEnum, TValue> right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    public static bool operator >=(WFEnumBase<TEnum, TValue> left, WFEnumBase<TEnum, TValue> right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }
}
public interface IWFEnum { }