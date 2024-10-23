using System.Collections;
using System.Text.Json.Serialization;

namespace P3R.WeaponFramework.Tools.DataUtils;

public class FModelJsonDataAsset<T,InnerT>: ICollection<T>, IEnumerable<T>
    where T : IEquatable<T>
    where InnerT : FModelPropertyObject<T>, new()
{
    public FModelJsonDataAsset() { }

    [JsonPropertyName("Type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("Class")]
    public string Class { get; set; } = string.Empty;
    [JsonPropertyName("Properties")]
    public InnerT Properties { get; set; } = [];
    [JsonIgnore]
    public int Count => ((ICollection<T>)Properties).Count;
    [JsonIgnore]
    public bool IsReadOnly => ((ICollection<T>)Properties).IsReadOnly;
    public void Add(T item)
    {
        ((ICollection<T>)Properties).Add(item);
    }

    public void Clear()
    {
        ((ICollection<T>)Properties).Clear();
    }

    public bool Contains(T item)
    {
        return ((ICollection<T>)Properties).Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        ((ICollection<T>)Properties).CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)Properties).GetEnumerator();
    }

    public bool Remove(T item)
    {
        return ((ICollection<T>)Properties).Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Properties).GetEnumerator();
    }
}
