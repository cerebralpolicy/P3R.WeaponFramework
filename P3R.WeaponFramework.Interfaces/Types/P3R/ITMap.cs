using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Interfaces.Types
{
    internal unsafe interface ITMap<KeyType, ValueType> : ICollection<TMapPair<KeyType, ValueType>>
        where KeyType : notnull, IEquatable<KeyType>
        where ValueType : unmanaged, IEquatable<ValueType>
    {
        ValueType this[KeyType key] { get; set; }
        ICollection<KeyType> Keys { get; }
        ICollection<ValueType> Values { get; }
        bool ContainsKey(KeyType key);
        void TrySet(KeyType key, ValueType value);
        void Add(KeyType key, ValueType value);
        void Set(KeyType key, ValueType value);
        bool Remove(KeyType key);
        bool TryGetValue(KeyType key, out ValueType value);
    }
}
