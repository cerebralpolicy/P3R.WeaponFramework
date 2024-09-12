using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Sequential)]
public struct FSoftObjectPtr<T>
{
    public TPersistentObjectPtr<FSoftObjectPath> baseObj;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TSoftObjectPtr<T>
    where T : unmanaged
{
    public FSoftObjectPtr<T>* baseObj;

    /// <summary>
    /// Equivalent to <c>FORCEINLINE TSoftObjectPtr(const U* Object)
    /// : SoftObjectPtr(Object)</c><br/>
    /// Construct from an object already in memory
    /// </summary>
    /// <param name="obj"></param>
    public unsafe TSoftObjectPtr(T* obj)
    {
        baseObj = (FSoftObjectPtr<T>*)obj;
    }
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TSoftClassProperty
{
    public FSoftObjectPath* baseObj;
}