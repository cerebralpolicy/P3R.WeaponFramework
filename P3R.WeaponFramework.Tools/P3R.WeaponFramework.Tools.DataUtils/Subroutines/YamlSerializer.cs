using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace P3R.WeaponFramework.Tools.DataUtils;

internal static partial class Subroutines {
    internal static class YamlSerializer
    {
        private static readonly IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        private static readonly ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        public static T DeserializeFile<T>(string file)
            => deserializer.Deserialize<T>(File.ReadAllText(file));

        public static void SerializeFile<T>(string file, T obj)
            => File.WriteAllText(file, serializer.Serialize(obj));
    }
} 

