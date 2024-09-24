using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Interfaces.Types;


[StructLayout(LayoutKind.Sequential)]
public struct TSoftClassPtr<T> where T : unmanaged
{
    public FSoftObjectPtr baseObj;
}

[StructLayout(LayoutKind.Sequential, Size = 40)]
public unsafe struct SoftObjectProperty
{
    public FSoftObjectPtr baseObj;
}

[StructLayout(LayoutKind.Sequential)]
public struct FSoftObjectPtr
{
    public TPersistentObjectPtr<FSoftObjectPath> baseObj;
}

[StructLayout(LayoutKind.Sequential)]
public struct TPersistentObjectPtr<T> where T : unmanaged
{
    public FWeakObjectPtr WeakPtr;

    public int TagAtLastTest;

    public T ObjectId;
}

[StructLayout(LayoutKind.Sequential)]
public struct TSoftObjectPtr<T> where T : unmanaged
{
    public FSoftObjectPtr baseObj;
}

[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public unsafe struct FSoftObjectPath
{
    [FieldOffset(0x0000)] public FName AssetPathName;
    [FieldOffset(0x0008)] public FString SubPathString;

    public FSoftObjectPath(FName assetPathName) : this()
    {
        AssetPathName = assetPathName;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct FWeakObjectPtr
{
    public int ObjectIndex;
    public int ObjectSerialNumber;
}