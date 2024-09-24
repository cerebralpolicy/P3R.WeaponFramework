namespace P3R.WeaponFramework.Interfaces.Types;

public abstract class WFFlag<TFlag, TValue> : WFFlagBase<TFlag, TValue>, IEquatable<TFlag>, IComparable<TFlag>
    where TFlag : WFFlagBase<TFlag,TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    protected WFFlag(string name, TValue value) : base(name, value)
    {
    }
}
