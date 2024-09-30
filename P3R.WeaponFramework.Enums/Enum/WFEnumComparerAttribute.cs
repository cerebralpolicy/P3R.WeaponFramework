namespace P3R.WeaponFramework.Enums;

[AttributeUsage(AttributeTargets.Class)]
public class WFEnumComparerAttribute<T>: Attribute
{
    private readonly IEqualityComparer<T> _comparer;

    protected WFEnumComparerAttribute(IEqualityComparer<T> comparer)
    {
        _comparer = comparer;
    }

    public IEqualityComparer<T> Comparer => _comparer;
}