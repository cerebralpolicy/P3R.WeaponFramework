using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct FGuid
{
    [FieldOffset(0x0)] public uint A;
    [FieldOffset(0x4)] public uint B;
    [FieldOffset(0x8)] public uint C;
    [FieldOffset(0xc)] public uint D;

    public override string ToString() => $"{A:X}-{B:X}-{C:X}-{D:X}";
}
