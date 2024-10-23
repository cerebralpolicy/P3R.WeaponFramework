using System.Collections;
using System.Text.Json.Serialization;

namespace P3R.WeaponFramework.Tools.DataUtils;
public class FModelPropertyObject<T> : ICollection<T>, IEnumerable<T>
    where T : IEquatable<T>
{
    protected FModelPropertyObject()
    {
    }


    public T[] Data { get; set; } = [];

    public int Count => ((ICollection<T>)Data).Count;

    public bool IsReadOnly => ((ICollection<T>)Data).IsReadOnly;

    public void Add(T item)
    {
        ((ICollection<T>)Data).Add(item);
    }

    public void Clear()
    {
        ((ICollection<T>)Data).Clear();
    }

    public bool Contains(T item)
    {
        return ((ICollection<T>)Data).Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        ((ICollection<T>)Data).CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)Data).GetEnumerator();
    }

    public bool Remove(T item)
    {
        return ((ICollection<T>)Data).Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Data.GetEnumerator();
    }
}
