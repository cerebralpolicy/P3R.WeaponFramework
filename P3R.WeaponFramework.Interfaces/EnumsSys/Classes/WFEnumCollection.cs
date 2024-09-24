using System.Collections.ObjectModel;

namespace P3R.WeaponFramework.Interfaces.Types;
public class WFEnumCollection<TValue> : Collection<TValue>, IEquatable<WFEnumCollection<TValue>>, IComparable<WFEnumCollection<TValue>>
    where TValue : WFEnumBase<TValue, int>, IEquatable<TValue>
{
    public WFEnumCollection()
    {
    }

    public WFEnumCollection(IList<TValue> list) : base(list)
    {
    }

    public int CompareTo(WFEnumCollection<TValue>? other)
    {
        if (other == null)
            return 0;
        return object.ReferenceEquals(other, this) ? 1 : -1;
    }

    public bool Equals(WFEnumCollection<TValue>? other)
    {
        return object.ReferenceEquals(other, this);
    }
}
public class WFEnumCollection<TValue, TBaseValue> : Collection<TValue>, IEquatable<WFEnumCollection<TValue, TBaseValue>>, IComparable<WFEnumCollection<TValue, TBaseValue>>
    where TValue : WFEnum<TValue, WFEnumCollection<TBaseValue>>, IEquatable<TValue>
    where TBaseValue : WFEnum<TBaseValue, int>, IEquatable<TBaseValue>
{
    public WFEnumCollection()
    {
    }

    public WFEnumCollection(IList<TValue> list) : base(list)
    {
    }

    public int CompareTo(WFEnumCollection<TValue, TBaseValue>? other)
    {
        if (other == null)
            return 0;
        return object.ReferenceEquals(other, this) ? 1 : -1;
    }

    public bool Equals(WFEnumCollection<TValue, TBaseValue>? other)
    {
        return object.ReferenceEquals(other, this);
    }
}