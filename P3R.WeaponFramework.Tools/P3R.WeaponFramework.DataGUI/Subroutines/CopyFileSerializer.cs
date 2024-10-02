using System.IO;
using System.Reflection;

namespace P3R.WeaponFramework.DataGUI;

internal static partial class Subroutines
{
    internal static class CopyFileSerializer
    {
        public static T DeserializeLine<T>(string text)
            where T : new()
        {
            var obj = new T();
            var ctors = typeof(T).GetConstructors();
            var possibleFields = typeof(T).GetFields();
            var possibleFieldNames = possibleFields.Select(x => x.Name).ToArray();
            var args = text.Split(',');
            Dictionary<string, object?> map = new Dictionary<string, object?>();
            foreach (var arg in args)
            {
                var temp = arg.Split('=');
                var field = temp[0].ToLowerInvariant();
                var value = temp[1];
                map[field] = value;
            }
            foreach (var field in possibleFields)
            {
                var key = field.Name.ToLowerInvariant();
                if (!map.ContainsKey(key))
                    continue;
                field.SetValue(obj, map[key]);
            }
            return obj;
        }
        public static List<T> DeserializeDataAsset<T>(string file)
            where T : new()
        {
            int lineIndex = 0;
            List<T> list = [];
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(file)!;
            using (StreamReader reader = new(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null || line.StartsWith("Begin Object"))
                        continue;
                    else
                    {
                        var entry = line.TrimStart();
                        var declaration = $"Data({lineIndex})=(";
                        var define = entry.Replace(declaration, "").TrimEnd(')');
                        var obj = DeserializeLine<T>(define);
                        list.Add(obj);
                        lineIndex++;
                        continue;
                    }
                }
            }
            return list;
        }
    }
}
