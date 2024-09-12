using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

// For g_namePool
[StructLayout(LayoutKind.Explicit, Size = 0x8)]
public unsafe struct FName : IEquatable<FName>, IEquatable<Native.FName>, IEquatable<Emitter.FName>, IMapHashable
{
    [FieldOffset(0x0)] public uint pool_location;
    [FieldOffset(0x4)] public uint field04;

    public FName(uint pool_location, uint field04)
    {
        this.pool_location = pool_location;
        this.field04 = field04;
    }

    public bool Equals(FName other) => pool_location == other.pool_location;

    public bool Equals(Native.FName other) => pool_location == other.pool_location;

    public bool Equals(Emitter.FName other) => pool_location == other.pool_location;

    public static implicit operator API.FName(Native.FName name) => new(name.pool_location, name.field04);
    public static implicit operator API.FName(Emitter.FName name) => new(name.pool_location, name.unk1);
    public static implicit operator Native.FName(API.FName name) => new() { pool_location = name.pool_location, field04 = name.field04 };
    public static implicit operator Emitter.FName(API.FName name) => new() { pool_location = name.pool_location, unk1 = name.field04 };

    public unsafe static implicit operator FName*(FName name) => &name;
    public unsafe static implicit operator FName(FName* name) => *name;
    public unsafe static implicit operator Native.FName*(FName name) => (Native.FName*)&name;
    public unsafe static implicit operator Emitter.FName*(FName name) => (Emitter.FName*)&name;
    public unsafe static implicit operator FName(Native.FName* name) => (FName*)&name;
    public unsafe static implicit operator FName(Emitter.FName* name) => (FName*)&name;

    public uint GetTypeHash()
    {
        uint block = pool_location >> 0x10;
        uint offset = pool_location & 0xffff;
        return (block << 19) + block + (offset << 0x10) + offset + (offset >> 4) + field04;
    }


}
