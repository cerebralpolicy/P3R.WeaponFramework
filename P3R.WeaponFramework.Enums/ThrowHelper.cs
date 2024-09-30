using P3R.WeaponFramework.Enums.Exceptions;

namespace P3R.WeaponFramework.Enums;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
static class ThrowHelper
{
    public static void ThrowArgumentNullException(string paramName)
        => throw new ArgumentNullException(paramName);

    public static void ThrowArgumentNullOrEmptyException(string paramName)
        => throw new ArgumentException("Argument cannot be null or empty.", paramName);

    public static void ThrowNameNotFoundException<TEnum, TValue>(string name)
        where TEnum : IWFEnum
        where TValue : IEquatable<TValue>, IComparable<TValue>
        => throw new WFEnumNotFoundException($"No {typeof(TEnum).Name} with Name \"{name}\" found.");

    public static void ThrowValueNotFoundException<TEnum, TValue>(TValue value)
        where TEnum : IWFEnum
        where TValue : IEquatable<TValue>, IComparable<TValue>
        => throw new WFEnumNotFoundException($"No {typeof(TEnum).Name} with Value {value} found.");
    public static void ThrowContainsNegativeValueException<TEnum, TValue>()
            where TEnum : IWFEnum
            where TValue : IEquatable<TValue>, IComparable<TValue>
        => throw new WFFlagEnumContainsNegativeValueException($"The {typeof(TEnum).Name} contains negative values other than (-1).");

    public static void ThrowInvalidValueCastException<TEnum, TValue>(TValue value)
        where TEnum : IWFEnum
        where TValue : IEquatable<TValue>, IComparable<TValue>
    => throw new InvalidFlagEnumValueParseException($"The value: {value} input to {typeof(TEnum).Name} could not be parsed into an integer value.");

    public static void ThrowNegativeValueArgumentException<TEnum, TValue>(TValue value)
        where TEnum : IWFEnum
        where TValue : IEquatable<TValue>, IComparable<TValue>
    => throw new NegativeValueArgumentException($"The value: {value} input to {typeof(TEnum).Name} was a negative number other than (-1).");
    public static void ThrowNegativeValueArgumentException<TEnum, TValue>(int value)
        where TEnum : IWFEnum
        where TValue : IEquatable<TValue>, IComparable<TValue>
        => throw new NegativeValueArgumentException($"The value: {value} input to {typeof(TEnum).Name} was a negative number other than (-1).");

    public static void ThrowDoesNotContainPowerOfTwoValuesException<TEnum, TValue>()
        where TEnum : IWFEnum
        where TValue : IEquatable<TValue>, IComparable<TValue>
    => throw new WFFlagEnumDoesNotContainPowerOfTwoValuesException($"the {typeof(TEnum).Name} does not contain consecutive power of two values.");
}