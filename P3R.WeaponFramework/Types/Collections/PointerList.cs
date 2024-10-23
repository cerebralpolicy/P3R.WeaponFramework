using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
namespace P3R.WeaponFramework.Types.Collections
{
    internal unsafe class TList<T> : IList<T>
        where T : unmanaged
    {
        public Emitter.TArray<T> Data { get; private set; } = new();
        public List<T> ConcurrentData { get; private set; } = new();

        public T this[int index]
        {
            get => Data.AllocatorInstance[index];
            set
            {
                ConcurrentData[index] = value;
                Data.AllocatorInstance[index] = value;
            }
        }

        public void Add(T item)
        {
            ConcurrentData.Add(item);
            Data.AllocatorInstance[Count] = item;
        }

        public void Add(T* item) => Add(*item);

        public int Count => Data.Num;

        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator() => new TArrayWrapper<T>(Data).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => ConcurrentData.IndexOf(item);

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item) => ConcurrentData.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }
    }
}

#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type