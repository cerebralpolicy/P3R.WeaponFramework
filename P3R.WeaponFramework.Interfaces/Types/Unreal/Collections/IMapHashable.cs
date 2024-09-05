#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

namespace P3R.WeaponFramework.Interfaces.Types;

public interface IMapHashable
{
    public uint GetTypeHash();
}
