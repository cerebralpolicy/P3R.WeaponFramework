using System.Reflection.Emit;

namespace P3R.WeaponFramework.Enums;


public abstract class WFFlagWrapper<TEnum, EEnum> : WFFlagWrapper<TEnum, int, EEnum>
    where TEnum : WFFlagEnumBase<TEnum, int>
    where EEnum : struct, Enum
{
    protected WFFlagWrapper(EEnum flagValue) : base(flagValue)
    {
    }

    protected WFFlagWrapper(string name, int value) : base(name, value)
    {
    }
}
[AllowUnsafeFlagEnumValues]
public abstract class WFFlagWrapper<TEnum, TValue, EEnum> : WFFlagEnumBase<TEnum, TValue>
    where TEnum : WFFlagEnumBase<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
    where EEnum : struct, Enum 
{
    private readonly EEnum _flagValue;
    public EEnum FlagValue => _flagValue;
    protected WFFlagWrapper(EEnum flagValue) : base(flagValue.ToString(), flagValue.ToValue<TValue>())
    {
        _flagValue = flagValue;
    }


    protected WFFlagWrapper(string name, TValue value) : base(name, value)
    {
        _flagValue = (EEnum)Enum.Parse(typeof(EEnum), name);
    }
}
