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
        => throw new KeyNotFoundException($"No {typeof(TEnum).Name} with Name \"{name}\" found.");

    public static void ThrowValueNotFoundException<TEnum, TValue>(TValue value)
        where TEnum : IWFEnum
        where TValue : IEquatable<TValue>, IComparable<TValue>
        => throw new KeyNotFoundException($"No {typeof(TEnum).Name} with Value {value} found.");
}