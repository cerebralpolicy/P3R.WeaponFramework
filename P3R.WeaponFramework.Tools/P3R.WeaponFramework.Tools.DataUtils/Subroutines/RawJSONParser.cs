using System.Reflection;
using System.Text;
using System.Text.Json;

namespace P3R.WeaponFramework.Tools.DataUtils;

internal static partial class Subroutines {

    internal static class JsonFileSerializer {    
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        
        private static Stream? GetStream<T>(string file, string resourceFolder = "RawResources")
        {
            var isAssemblyPath = !file.Contains(Path.DirectorySeparatorChar);
            if (isAssemblyPath) { 
                string[] thisPath = file.Split('.');
                if (thisPath.Contains(resourceFolder))
                {
                    var index = Array.IndexOf(thisPath, resourceFolder);
                    var assemblyTrim = thisPath.Take(index-1);
                    var assemblyName = String.Join(".", assemblyTrim);
                    return Assembly.Load(assemblyName).GetManifestResourceStream(file);
                }
                else
                {
                    return typeof(T).Assembly.GetManifestResourceStream(file);
                }
            }
            else
            {
                return Assembly.GetExecutingAssembly().GetManifestResourceStream(file);
            }
        }

        public static T DeserializeFile<T>(string file)
        {
            using var stream = GetStream<T>(file)!;
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<T>(json)!;
        }
        public static void SerializeFile<T>(string path, T obj)
        {
            var outputFile = Path.Join(path, $"{nameof(obj)}.json");
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }
            var jsonOut = JsonSerializer.Serialize(obj, SerializerOptions);
            File.WriteAllText(outputFile, jsonOut);
        } 
    }
} 

