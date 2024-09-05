using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Sequential)]
public struct TPersistentObjectPtr<T> where T : unmanaged
{
    public FWeakObjectPtr WeakPtr;
    public int TagAtLastTest;
    public T ObjectId;
}
