using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Extensions;

public abstract class TManagedArrayBase<T> : IList<T>, IDisposable where T : unmanaged
{
    protected IMemoryMethods MemoryMethods;
    protected unsafe TArray<T>* Self;
    protected bool bOwnsAllocation;
    protected bool bIsDisposed;

    protected unsafe TManagedArrayBase(IMemoryMethods memoryMethods, TArray<T>* self)
    {
        this.MemoryMethods = memoryMethods;
        this.Self = self;
        bOwnsAllocation = false;
    }

    protected unsafe TManagedArrayBase(IMemoryMethods memoryMethods)
    {
        this.MemoryMethods = memoryMethods;
        Self = memoryMethods.FMemory_Malloc<TArray<T>>();
        NativeMemory.Fill(Self, (nuint)sizeof(TArray<T>), 0);
        bOwnsAllocation = true;
    }

    public unsafe int Count => Self->arr_num;
    public bool IsReadOnly => false;

    public abstract T this[int index] { get; set; }

    protected virtual void Dispose(bool disposing)
    {
        if (!bIsDisposed)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            if (bOwnsAllocation)
                unsafe
                {
                    MemoryMethods.FMemory_Free(Self);
                }
            bIsDisposed = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~TManagedArrayBase()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public abstract int IndexOf(T item);
    public abstract void Insert(int index, T item);
    public abstract void RemoveAt(int index);
    public abstract void Add(T item);
    public abstract void Clear();
    public abstract bool Contains(T item);
    public abstract void CopyTo(T[] array, int arrayIndex);
    public abstract bool Remove(T item);
    public abstract IEnumerator<T> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
public abstract class TManagedArrayBaseEnumerator<T> : IEnumerator<T>, IEnumerator where T : unmanaged
{
    protected unsafe TArray<T>* self;
    public abstract object Current { get; }
    T IEnumerator<T>.Current => (T)Current;
    public void Dispose() { }
    public abstract bool MoveNext();
    public abstract void Reset();
}
public class TManagedArray<T> : TManagedArrayBase<T>, IEnumerable where T : unmanaged
{
    public unsafe TManagedArray(IMemoryMethods memoryMethods, TArray<T>* self) : base(memoryMethods, self)
    {
    }

    public unsafe override T this[int index] 
    { 
        get => *Self->GetRef(index);
        set => MemoryMethods.TArray_Insert(Self, value, index); 
    }

    public unsafe override void Add(T item) => MemoryMethods.TArray_Insert(Self, item);

    public unsafe override void Clear()
    {
        NativeMemory.Clear(Self->allocator_instance, (nuint)(sizeof(T) * Self->arr_num));
        Self->arr_num = 0;
    }

    public override bool Contains(T item)
    {
        foreach (var el in this)
            if (el.Equals(item)) return true;
        return false;
    }

    public unsafe override void CopyTo(T[] array, int arrayIndex)
    {
        if (arrayIndex > Self->arr_num || arrayIndex < 0) return; // lol, lmao even
        for (int i = 0; i < Self->arr_num - arrayIndex; i++)
            array[i] = Self->allocator_instance[i + arrayIndex];
    }


    public unsafe override IEnumerator<T> GetEnumerator() => new TManagedArrayEnumerator<T>(Self);

    public override int IndexOf(T item)
    {
        int index = 0;
        foreach (var el in this)
        {
            if (el.Equals(item)) return index;
            index++;
        }
        return -1;
    }

    public unsafe override void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    public unsafe override bool Remove(T item)
    {
        int ItemIndex = IndexOf(item);
        if (ItemIndex != -1)
        {
            RemoveAt(ItemIndex);
            return true;
        }
        return false;
    }

    public unsafe override void RemoveAt(int index) => MemoryMethods.TArray_Delete(Self, index);
}
public class TManagedArrayEnumerator<T> : TManagedArrayBaseEnumerator<T>, IEnumerator<T> where T : unmanaged
{
    protected unsafe TArray<T>* Self;
    private int position = -1;
    public unsafe override object Current
    {
        get
        {
            if (position < 0 || position >= Self->arr_num)
                throw new InvalidOperationException();
            return *Self->GetRef(position);
        }
    }
    public unsafe TManagedArrayEnumerator(TArray<T>* _Self) => Self = _Self;
    public unsafe override bool MoveNext() => ++position < Self->arr_num;
    T IEnumerator<T>.Current => (T)Current;
    public override void Reset() => position = -1;
}

public unsafe class TWeaponItemListTable : TManagedArray<FWeaponItemList>
{
    public TWeaponItemListTable(IMemoryMethods memoryMethods, TArray<FWeaponItemList>* self) : base(memoryMethods, self)
    {
    }
    public unsafe void Overwrite(int index, FWeaponItemList fWeapon)
    {
        MemoryMethods.TArray_Insert(Self,fWeapon, index);
    }
    public unsafe void Swap(int unusedIndex, int modIndex)
    {
        var temp = this[unusedIndex];
        var mod = this[modIndex];
        Log.Debug($"Swapping ID: {unusedIndex}\nEquip ID: {mod.EquipID}");
        this[unusedIndex] = this[modIndex];
        this[modIndex] = temp;
    }
}
