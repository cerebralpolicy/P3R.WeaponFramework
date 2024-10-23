using System.Collections;
using System.Text.Json.Serialization;

namespace P3R.WeaponFramework.Tools.DataUtils;

public struct FModelWeaponList
{
    public FModelWeaponList()
    {
    }
    [JsonConstructor]
    public FModelWeaponList(string type, string name, string @class, FModelWeaponListInner properties)
    {
        Type = type;
        Name = name;
        Class = @class;
        Properties = properties;
    }

    [JsonPropertyName("Type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("Class")]
    public string Class { get; set; } = string.Empty;
    [JsonPropertyName("Properties")]
    public FModelWeaponListInner Properties { get; set; } = [];

}
