using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;

namespace P3R.WeaponFramework.Enums;
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

internal static class TypeExtensions
{
    public static List<TFieldType> GetFieldsOfType<TFieldType>(this Type type) => 
        type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(p => type.IsAssignableFrom(p.FieldType))
            .Select(pi => (TFieldType)pi.GetValue(null))
            .ToList();

/*    public static bool TryGetFlagEnumValuesByName<TEnum, TValue>(this Dictionary<string, TEnum> dictionary, string names, [NotNullWhen(true)] out IEnumerable<TEnum>? outputEnums)
            where TEnum : WFFlagBase<TEnum, TValue>
            where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        var outputList = new List<TEnum>(dictionary.Count);

        var commaSplitNameList = names.Replace(" ", "").Trim().Split(',');
        Array.Sort(commaSplitNameList);

        foreach (var enumValue in dictionary.Values)
        {
            var result = Array.BinarySearch(commaSplitNameList, enumValue.Name);
            if (result >= 0)
            {
                outputList.Add(enumValue);
            }
        }

        if (!outputList.Any())
        {
            outputEnums = null;
            return false;
        }

        outputEnums = outputList.ToList();
        return true;
    }*/
    public static T ToValue<T>(this Enum _enum)
        where T : IEquatable<T>, IComparable<T>
    {
        if (TypeDescriptor.GetConverter(_enum).CanConvertTo(typeof(T)))
            return (T)TypeDescriptor.GetConverter(_enum).ConvertTo(_enum, typeof(T))!;
        return (T)TypeDescriptor.GetConverter(0).ConvertTo(0, typeof(T))!;
    }
    public static int ToValue(this Enum _enum)
    {
        if (TypeDescriptor.GetConverter(_enum).CanConvertTo(typeof(int)))
            return (int)TypeDescriptor.GetConverter(_enum).ConvertTo(_enum, typeof(int))!;
        return 0;
    }
}
