using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Types;
internal class P3RWF_Dictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
{
    public static P3RWF_Dictionary<TKey, TValue> LoadJson(Assembly assembly, string resource)
    {
        using var stream = assembly.GetManifestResourceStream(resource)!;
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<P3RWF_Dictionary<TKey, TValue>>(json)!;
    }
}