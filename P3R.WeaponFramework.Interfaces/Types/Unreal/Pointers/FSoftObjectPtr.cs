using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Sequential)]
public struct FSoftObjectPtr<T> where T : unmanaged
{
    public TPersistentObjectPtr<FSoftObjectPath<T>> baseObj;
}

[StructLayout(LayoutKind.Sequential)]
public struct TSoftClassProperty<T> where T : unmanaged
{
    public FSoftObjectPath<T> baseObj;
}