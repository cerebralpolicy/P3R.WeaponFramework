using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Text;

namespace P3R.WeaponFramework.DataGUI;

internal static partial class Subroutines
{
    internal static class JsonFileSerializer
    {
        public static TextEncoderSettings ThisEncoder
        {
            get
            {
                var settings = new TextEncoderSettings();
                settings.AllowCharacter('\u0027');
                settings.AllowRange(UnicodeRanges.BasicLatin);
                return settings;
            }
        }

        public static Encoding Encoding = Encoding.UTF8;
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        private static Stream? GetStream<T>(string file, string resourceFolder = "RawResources")
        {
            var isAssemblyPath = !file.Contains(Path.DirectorySeparatorChar);
            if (isAssemblyPath)
            {
                string[] thisPath = file.Split('.');
                if (thisPath.Contains(resourceFolder))
                {
                    var index = Array.IndexOf(thisPath, resourceFolder);
                    var assemblyTrim = thisPath.Take(index - 1);
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
        public static void SerializeFile<T>(string path, T obj, string? name = null)
        {
            var fileName = name ?? nameof(obj);
            var outputFile = Path.Join(path, $"{fileName}.json");
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }
            var jsonOut = JsonSerializer.Serialize(obj, SerializerOptions);
            File.WriteAllText(outputFile, jsonOut);
        }
    }
}
