using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace P3R.WeaponFramework.Tools.DataUtils;

internal static partial class Subroutines {
    internal static class JsonFileSerializer {
        public static TextEncoderSettings ThisEncoder
        {
            get {
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
        public static void SerializeFile<T>(string path, T obj, Episode episode = Episode.Xrd777, string? name = "Weapons")
        {
            var fileName = name ?? nameof(obj);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filePath = Path.Join(path, episode.ToString(), $"{fileName}.json");
            var jsonOut = JsonSerializer.Serialize(obj, SerializerOptions);
            if (File.Exists(filePath))
            {
                var fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
                fs.SetLength(0);
                fs.Flush();
                fs.Close();
            }
            else
            {
                File.Create(filePath).Close();
            }
            var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            lock (file)
            {
                var buffer = new StreamWriter(file);
                buffer.Write(jsonOut);
                buffer.Flush();
                file.Close();
            }
        } 
    }
} 

