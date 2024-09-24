namespace P3R.WeaponFramework.Interfaces.Types;

public abstract class WFEnum<TEnum, TValue> : WFEnumBase<TEnum, TValue>, IEquatable<TEnum>, IComparable<TEnum>
    where TEnum : WFEnumBase<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    
    protected WFEnum(string name, TValue value) : base(name, value)
    {
        //var id = Enum.GetName(typeof(TValue), value);
    }
}
