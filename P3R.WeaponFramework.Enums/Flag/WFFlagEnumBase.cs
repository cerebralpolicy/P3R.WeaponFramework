using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace P3R.WeaponFramework.Enums;
public abstract class WFFlagEnumBase<TEnum,TValue> : 
    WFFlagEngine<TEnum,TValue>, 
    IWFEnum,
    IEquatable<WFFlagEnumBase<TEnum, TValue>>,
    IComparable<WFFlagEnumBase<TEnum, TValue>>
    where TEnum : WFFlagEnumBase<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    static readonly Lazy<Dictionary<string, TEnum>> _fromName =
    new Lazy<Dictionary<string, TEnum>>(() => GetAllOptions().ToDictionary(item => item.Name));

    static readonly Lazy<Dictionary<string, TEnum>> _fromNameIgnoreCase =
        new Lazy<Dictionary<string, TEnum>>(() => GetAllOptions().ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));

    private static IEnumerable<TEnum> GetAllOptions()
    {
        Type baseType = typeof(TEnum);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        IEnumerable<Type> enumTypes = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType.IsAssignableFrom(t));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        List<TEnum> options = new List<TEnum>();
        foreach (Type enumType in enumTypes)
        {
            List<TEnum> typeEnumOptions = enumType.GetFieldsOfType<TEnum>();
            options.AddRange(typeEnumOptions);
        }

        return options.OrderBy(t => t.Value);
    }

    public static IReadOnlyCollection<TEnum> List =>
        _fromName.Value.Values.ToList().AsReadOnly();
    private readonly string _name;
    private readonly TValue _value;

    protected WFFlagEnumBase(string name, TValue value)
    {
        if (String.IsNullOrEmpty(name))
            ThrowHelper.ThrowArgumentNullOrEmptyException(nameof(name));
        
        _name = name;
        _value = value;
    }

    public string Name => _name;
    public TValue Value => _value;

    public static IEnumerable<TEnum> FromName(string names, bool ignoreCase = false, bool deserialize = false)
    {
        if (String.IsNullOrEmpty(names))
            ThrowHelper.ThrowArgumentNullOrEmptyException(nameof(names));

        if (ignoreCase)
            return FromName(_fromNameIgnoreCase.Value);
        else
            return FromName(_fromName.Value);

        IEnumerable<TEnum> FromName(Dictionary<string, TEnum> dictionary)
        {
            if (!dictionary.TryGetFlagEnumValuesByName<TEnum, TValue>(names, out var result))
            {
                ThrowHelper.ThrowNameNotFoundException<TEnum, TValue>(names);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return result;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFromName(string names, out IEnumerable<TEnum> result) =>
    TryFromName(names, false, out result);
    public static bool TryFromName(string names, bool ignoreCase,  [NotNullWhen(true)] out IEnumerable<TEnum> result)
    {
        if (String.IsNullOrEmpty(names))
            ThrowHelper.ThrowArgumentNullOrEmptyException(nameof(names));

        if (ignoreCase)
#pragma warning disable CS8601 // Possible null reference assignment.
            return _fromNameIgnoreCase.Value.TryGetFlagEnumValuesByName<TEnum, TValue>(names, out result);
#pragma warning restore CS8601 // Possible null reference assignment.
        else
#pragma warning disable CS8601 // Possible null reference assignment.
            return _fromName.Value.TryGetFlagEnumValuesByName<TEnum, TValue>(names, out result);
#pragma warning restore CS8601 // Possible null reference assignment.
    }
    public static IEnumerable<TEnum> FromValue(TValue value)
    {
        if (value == null)
            ThrowHelper.ThrowArgumentNullException(nameof(value));

#pragma warning disable CS8604 // Possible null reference argument.
        if (GetFlagEnumValues(value, GetAllOptions()) == null)
        {
            ThrowHelper.ThrowValueNotFoundException<TEnum, TValue>(value);
        }
#pragma warning restore CS8604 // Possible null reference argument.

        return GetFlagEnumValues(value, GetAllOptions());
    }
    public static TEnum DeserializeValue(TValue value)
    {
        // we should not be calling get options for each deserialization. Perhaps move it to a lazy field _enumOptions.
        var enumList = GetAllOptions();

        var returnValue = enumList.FirstOrDefault(x => x.Value.Equals(value));

        if (returnValue is null)
        {
            ThrowHelper.ThrowValueNotFoundException<TEnum, TValue>(value);
        }

#pragma warning disable CS8603 // Possible null reference return.
        return returnValue;
#pragma warning restore CS8603 // Possible null reference return.
    }
    public static IEnumerable<TEnum> FromValue(TValue value, IEnumerable<TEnum> defaultValue)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        else
            return !TryFromValue(value, out var result) ? defaultValue : result;
    }
    public static bool TryFromValue(TValue value, [NotNullWhen(true)] out IEnumerable<TEnum>? result)
    {
        if (value == null || !int.TryParse(value.ToString(), out _))
        {
            result = default;
            return false;
        }
        result = GetFlagEnumValues(value, GetAllOptions());
        if (result == null)
        {
            return false;
        }
        return true;
    }
    public static string FromValueToString(TValue value)
    {
        if (!TryFromValue(value, out _))
        {
            ThrowHelper.ThrowValueNotFoundException<TEnum, TValue>(value);
        }

        return FormatEnumListString(GetFlagEnumValues(value, GetAllOptions()));
    }
    public static bool TryFromValueToString(TValue value, [NotNullWhen(true)] out string? result)
    {
        if (!TryFromValue(value, out var enumResult))
        {
            result = default;
            return false;
        }

        result = FormatEnumListString(GetFlagEnumValues(value, enumResult));
        return true;
    }
    private static string FormatEnumListString(IEnumerable<TEnum> enumInputList)
    {
        var enumList = enumInputList.ToList();
        var sb = new StringBuilder();

        foreach (var smartFlagEnum in enumList.Select(x => x.Name))
        {
            sb.Append(smartFlagEnum);
            if (enumList.Last().Name != smartFlagEnum && enumList.Count > 1)
            {
                sb.Append(", ");
            }
        }

        return sb.ToString();
    }

    public bool Equals(WFFlagEnumBase<TEnum, TValue>? other)
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

    public int CompareTo(WFFlagEnumBase<TEnum, TValue>? other)
    {
        throw new NotImplementedException();
    }

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    public override bool Equals(object obj) => (obj is WFFlagEnumBase<TEnum, TValue> other) && Equals(other);
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(WFFlagEnumBase<TEnum, TValue> left, WFFlagEnumBase<TEnum, TValue> right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }

        return left.Equals(right);
    }

    public static bool operator !=(WFFlagEnumBase<TEnum, TValue> left, WFFlagEnumBase<TEnum, TValue> right)
    {
        return !(left == right);
    }

    public static bool operator <(WFFlagEnumBase<TEnum, TValue> left, WFFlagEnumBase<TEnum, TValue> right)
    {
        return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
    }

    public static bool operator <=(WFFlagEnumBase<TEnum, TValue> left, WFFlagEnumBase<TEnum, TValue> right)
    {
        return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
    }

    public static bool operator >(WFFlagEnumBase<TEnum, TValue> left, WFFlagEnumBase<TEnum, TValue> right)
    {
        return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
    }

    public static bool operator >=(WFFlagEnumBase<TEnum, TValue> left, WFFlagEnumBase<TEnum, TValue> right)
    {
        return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}
