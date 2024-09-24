using System.IO;

namespace P3R.WeaponFramework.Tools.DataUtils
{
    internal class Program
    {
        public const char tab = '\t';
        internal static WeaponConfig ExampleConfig = new()
        {
            Name = "Your Weapon Name",
            Shell = ShellType.Player,
            Model = new()
            {
                MeshPath1 = "",
                MeshPath2 = ""
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
                string inputAtlusScriptFormat = "";

                ExampleConfig.SetConfigPrices();

                Console.WriteLine("Choose output file type:");
                fileType = Console.ReadLine()!;

                FileType type = Enum.Parse<FileType>(fileType, true);

                Console.WriteLine("Specify output directory:");
                path = Console.ReadLine()!;
                if (type == FileType.Json) {
                    var weapons = Subroutines.GetCharaWeapons();
                    var weaponsAstrea = Subroutines.GetCharaWeapons(Episode.Astrea);
                    Subroutines.JsonFileSerializer.SerializeFile(path, weapons, "Weapons");
                    Subroutines.JsonFileSerializer.SerializeFile(path, weaponsAstrea, "WeaponsAstrea");
                }
                if (type == FileType.Yaml) 
                {
                    MakeExampleConfig(path);   
                }
                endApp = true;
            }
        }
        static string Comment(string text, int level, bool header = false) => $"{new('\t', level)}# {(header ? text.ToUpperInvariant() : text)}";

        static async void MakeExampleConfig(string path)
        {
            #region local functions
            #endregion
            var obj = ExampleConfig;
            var file = Path.Join(path, $"{nameof(ExampleConfig)}.yaml");
            using (StreamWriter writer = new StreamWriter(file))
            {
                //Subroutines.YamlSerializer.SerializeObject(obj, writer);
                const int ALIGN = 24;
                await writer.WriteLineAsync(Comment("EXAMPLE", 0, true));
                var text = Subroutines.YamlSerializer.SerializeObject(obj);
                writer.WriteLine(text);
                var spacer = Comment("Notes",0,true);
                writer.WriteLine(spacer);
                var shellHead = Comment("Shell types",1,true);
                writer.WriteLine(shellHead);
                foreach (var shell in ShellTypes.shells)
                {
                    var name = shell.Name;
                    var desc = shell.Description;
                    string entry = $"shell: {name}{new string(' ', ALIGN+3 - name!.Length)}{desc}";
                    writer.WriteLine(Comment(entry, 2));
                }
                var attrHead = Comment("Attr_id types",1,true);
                writer.WriteLine(attrHead);
                foreach (var attr in Enum.GetValues<EBtlDataAttr>())
                {
                    var id = (int)attr;
                    var name = Enum.GetName(typeof(EBtlDataAttr), id);
                    string entry = $"skill_id: {name}{new string(' ', ALIGN - name!.Length)}(idx: {id}){(id > 9 ? "I recommend against using this" : "")}";
                    writer.WriteLine(Comment(entry, 2));
                }
                var skillHead = Comment("Skill_id types",1,true);
                writer.WriteLine(skillHead);
                foreach (var skill in Enum.GetValues<EItemSkillId>())
                {
                    var id = (int)skill;
                    var name = Enum.GetName(typeof(EItemSkillId), id);
                    string entry = $"skill_id: {name}{new string(' ',ALIGN-name!.Length)}(idx: {id})";
                    writer.WriteLine(Comment(entry,2));
                }
                await writer.FlushAsync();
            }
        }
    }
}
