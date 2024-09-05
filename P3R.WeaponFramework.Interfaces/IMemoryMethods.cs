using P3R.WeaponFramework.Interfaces.Types;

namespace P3R.WeaponFramework.Interfaces
{
    internal interface IMemoryMethods
    {
        public interface IMemoryMethods
        {
            // Memory Allocation
            public unsafe void FMemory_Free(nint ptr);
            public unsafe void FMemory_Free<TType>(TType* ptr) where TType : unmanaged;
            public unsafe nint FMemory_Realloc(nint ptr, nint size, uint alignment);
            public unsafe nint FMemory_GetAllocSize(nint ptr);
            public unsafe nint FMemory_Malloc(nint size, uint alignment);
            public unsafe TType* FMemory_Malloc<TType>(uint alignment) where TType : unmanaged;
            public unsafe TType* FMemory_Malloc<TType>() where TType : unmanaged;
            public unsafe TType* FMemory_MallocMultiple<TType>(uint count, uint alignment) where TType : unmanaged;
            public unsafe TType* FMemory_MallocMultiple<TType>(uint count) where TType : unmanaged;
            public unsafe TType* FMemory_MallocZeroed<TType>() where TType : unmanaged;
            public unsafe TType* FMemory_MallocMultipleZeroed<TType>(uint count) where TType : unmanaged;

            // Array modification
            public unsafe bool TArray_Insert<TArrayType>(TArray<TArrayType>* arr, TArrayType entry) where TArrayType : unmanaged;
            public unsafe bool TArray_Insert<TArrayType>(TArray<TArrayType>* arr, TArrayType entry, int index) where TArrayType : unmanaged;
            public unsafe bool TArray_Delete<TArrayType>(TArray<TArrayType>* arr, int index) where TArrayType : unmanaged;

            // Map modification (no hashing atm)
            public unsafe bool TMap_Insert<KeyType, ValueType>(TMap<KeyType, ValueType>* map, KeyType key, ValueType val)
                where KeyType : unmanaged, IEquatable<KeyType>
                where ValueType : unmanaged;

            public unsafe bool TMap_InsertNoInit<KeyType, ValueType>(TMap<KeyType, ValueType>* map, KeyType key, ValueType val)
                where KeyType : unmanaged, IEquatable<KeyType>, IMapHashable
                where ValueType : unmanaged;
        }
    }
}
