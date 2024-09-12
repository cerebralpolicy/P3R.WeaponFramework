using Project.Utils;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Unreal.ObjectsEmitter.Interfaces;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FString
{
    TArray<nint> Text;
    public FString(IUnreal unreal, string str)
    {
        Text.arr_max = str.Length + 1;
        Text.arr_num = Text.arr_max;
        Text.allocator_instance = (nint*)unreal.FMalloc(Text.arr_max * sizeof(nint), 0);
        var bytes = Encoding.Unicode.GetBytes(str + '\0');
        Marshal.Copy(bytes, 0, Text.arr_num,bytes.Length);
    }
    public FString(IMemoryMethods mem, string str)
    {
        Text.arr_max = str.Length + 1;
        Text.arr_num = Text.arr_max;
        Text.allocator_instance = (nint*)mem.FMemory_Malloc(Text.arr_max * sizeof(nint), 0);
        var bytes = Encoding.Unicode.GetBytes(str + '\0');
        Marshal.Copy(bytes, 0, Text.arr_num, bytes.Length);
    }
    public FString(string str)
    {
        Text.arr_max = str.Length + 1;
        Text.arr_num = Text.arr_max;
        Text.allocator_instance = (nint*)memAPI.WFMemory_Malloc(Text.arr_max * sizeof(nint), 0);
        var bytes = Encoding.Unicode.GetBytes(str + '\0');
        Marshal.Copy(bytes, 0, Text.arr_num, bytes.Length);

    }

    public static implicit operator Native.FString(FString fStr)
    {
        return new Native.FString()
        {
            text = fStr.Text,
        };
    }
    public static implicit operator Emitter.FString(FString fStr)
    {
    #pragma warning disable CS0618 // Type or member is obsolete - WF FStrings are FMalloc'd through the emitter
        var str = fStr.ToString();
        if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            throw new ArgumentNullException(nameof(str));
        return new Emitter.FString(str);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public readonly override string? ToString()
        => Marshal.PtrToStringUni((nint)Text.allocator_instance, Text.arr_num);

    public readonly void Dispose()
        => Marshal.FreeHGlobal((nint)Text.allocator_instance);

    public override int GetHashCode()
        => Text.allocator_instance->GetHashCode();
}