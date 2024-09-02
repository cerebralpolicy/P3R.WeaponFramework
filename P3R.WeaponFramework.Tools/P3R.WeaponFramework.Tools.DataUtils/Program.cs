namespace P3R.WeaponFramework.Tools.DataUtils
{
    internal class Program
    {
        internal static WeaponConfig ExampleConfig = new()
        {
            Name = "Your Weapon Name",
            Base = new()
            {
                MeshPath = "[OPTIONAL - Path to base skeleton]",
                AnimPath = "[OPTIONAL - Path to weapon animations?]",
            },
            Mesh = new()
            {
                MeshPath = "[OPTIONAL - Path to weapon mesh not in the weapon folder.]"
            },
            Stats = new()
            {
                AttrId = 0,
                Attack = 30,
                Accuracy = 95,
            }
        };
        public enum FileType
        {
            Json,
            Yaml
        }
        static void Main(string[] args)
        {
            bool endApp = false;
            while (!endApp)
            {
                string fileType = "";
                string path = "";

                Console.WriteLine("Choose output file type:");
                fileType = Console.ReadLine()!;

                FileType type = Enum.Parse<FileType>(fileType, true);

                Console.WriteLine("Specify output directory:");
                path = Console.ReadLine()!;
                if (type == FileType.Json) {
                    var obj = Subroutines.GetCharaWeapons();
                    Subroutines.JsonFileSerializer.SerializeFile(path, obj);
                }
                if (type == FileType.Yaml) 
                {
                    var obj = ExampleConfig;
                    var file = Path.Join(path, $"{nameof(ExampleConfig)}.yaml");
                    Subroutines.YamlSerializer.SerializeFile(file, obj);
                }
                endApp = true;
            }
        }
    }
}
