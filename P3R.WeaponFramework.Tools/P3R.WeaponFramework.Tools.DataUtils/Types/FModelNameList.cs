using System.Collections;
using System.Text.Json.Serialization;

namespace P3R.WeaponFramework.Tools.DataUtils;

public struct FModelNameList
{
    public FModelNameList()
    {
    }
    [JsonConstructor]
    public FModelNameList(string type, string name, string @class, FModelNameListInner properties)
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
    public FModelNameListInner Properties { get; set; } = [];

}
