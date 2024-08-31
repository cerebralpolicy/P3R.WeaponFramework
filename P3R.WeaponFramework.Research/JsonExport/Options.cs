using System.Text.Json;

namespace P3R.WeaponFramework.Research;

public static partial class JsonExport
{
    private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
    { 
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
    };
    private static readonly JsonWriterOptions writerOptions = new JsonWriterOptions()
    {
        Indented = true,
    };
}