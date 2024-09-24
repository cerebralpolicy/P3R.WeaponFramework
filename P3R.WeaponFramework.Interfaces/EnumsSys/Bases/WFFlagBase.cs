using Ardalis.SmartEnum;

namespace P3R.WeaponFramework.Interfaces.Types;
public abstract class WFFlagBase<TFlag, TValue> : SmartFlagEnum<TFlag, TValue>, IEquatable<TFlag>, IComparable<TFlag>
    where TFlag : SmartFlagEnum<TFlag, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    protected WFFlagBase(string name, TValue value) : base(name, value)
    {
    }

    public abstract int CompareTo(TFlag? other);
    public abstract bool Equals(TFlag? other);
}