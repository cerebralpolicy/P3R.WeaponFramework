using System.Numerics;

namespace P3R.WeaponFramework.Enums;

public abstract class WFEnumWrapper<TEnum, EEnum> : WFEnumBase<TEnum, int>, IEquatable<EEnum>, IComparable<EEnum>
    where TEnum : WFEnumBase<TEnum, int>
    where EEnum : Enum
{
    public WFEnumWrapper(EEnum enumValue) : base(enumValue.ToString(), enumValue.ToValue())
    {

    }
    protected WFEnumWrapper(string name, int value) : base(name, value)
    {
    }

    public static TEnum FromEnum(EEnum enumValue) => FromValue(enumValue.ToValue());

    public int CompareTo(EEnum? other) => Value.CompareTo(other?.ToValue());

    public bool Equals(EEnum? other) => Value.Equals(other?.ToValue());
}
public abstract class WFEnumWrapper<TEnum, TValue, EEnum> : WFEnumBase<TEnum, TValue>, IEquatable<EEnum>, IComparable<EEnum>
    where TEnum : WFEnumBase<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
    where EEnum : Enum
{
    public WFEnumWrapper(EEnum enumValue) : base(enumValue.ToString(),enumValue.ToValue<TValue>())
        { }
    protected WFEnumWrapper(string name, TValue value) : base(name, value)
    {

    }
    public static TEnum FromEnum(EEnum enumValue) => FromValue(enumValue.ToValue<TValue>());

    public int CompareTo(EEnum? other) => Value.CompareTo(other!.ToValue<TValue>());

    public bool Equals(EEnum? other) => Value.Equals(other?.ToValue());
}