using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.DescriptionGenerator;

public enum ItemType
{
    Accs,
    AddEffect,
    Armor,
    Common,
    Evitem,
    Material,
    Shoes,
    SkillCard,
    Weapon,
}

public enum AssetDirectory
{
    Astrea,
    Xrd777,
}

internal class ItemHelpEntry
{
    private int index;
    private string id;
    private string message;

    public ItemHelpEntry(int index, string id, string message)
    {
        this.index = index;
        this.id = id;
        this.message = message;
    }

    public string AsMessage()
    {
        var sb = new StringBuilder();
        sb.Append(id);
        sb.AppendLine(message);
        sb.AppendLine();
        return sb.ToString();
    }
    public string AsHeaderLine() => $"const int {id}                        ={index};";
}

internal class ItemHelpFile : IReadOnlyList<ItemHelpEntry>
{
    static string BasePath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AtlusData");
    static string OutPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "NewData");
    static string[] PathEndings = [".uasset", "_unwrapped.bmd", "_unwrapped.bmd.msg", "_unwrapped.bmd.msg.h"];
    private ItemType type;
    private string uasset;
    private string unwrapped;
    private string message;
    public string messageOut { get; private set; }
    private string header;
    public string headerOut { get; private set; }
    private bool isEnglish;

    public List<ItemHelpEntry> Entries { get; private set; } = [];

    public int Count => ((IReadOnlyCollection<ItemHelpEntry>)Entries).Count;

    public ItemHelpEntry this[int index] => ((IReadOnlyList<ItemHelpEntry>)Entries)[index];

    public ItemHelpFile(ItemType type, AssetDirectory directory, bool english = false)
    {
        isEnglish = english;
        string resource = string.Empty;
        string output = string.Empty;
        this.type = type;
        if (directory == AssetDirectory.Astrea)
        {
            if (english)
                resource = AstreaAssetPathEN(BasePath,type);
            else
                resource = AstreaAssetPath(BasePath,type);
        }
        else
        {
            if (english)
                resource = Xrd777AssetPathEN(BasePath,type);
            else
                resource = Xrd777AssetPath(BasePath,type);
        }
        if (directory == AssetDirectory.Astrea)
        {
            if (english)
                output = AstreaAssetPathEN(OutPath, type);
            else
                output = AstreaAssetPath(OutPath, type);
        }
        else
        {
            if (english)
                output = Xrd777AssetPathEN(OutPath, type);
            else
                output = Xrd777AssetPath(OutPath, type);
        }
        uasset = resource + PathEndings[0];
        unwrapped = resource + PathEndings[1];
        message = resource + PathEndings[2];
        messageOut = output + PathEndings[2];
        header = resource + PathEndings[3];
        headerOut = output + PathEndings[3];
    }
    public void ReadAssets()
    {
        List<string> entries = [];
        var lineEnding = isEnglish ? "[n][e]" : "[n][f 1 5 0][e]";
        using var stream = File.OpenRead(message);
        if (stream == null)
            throw new NullReferenceException(nameof(stream));
        using var reader = new StreamReader(stream);
        string tempentry = string.Empty;
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line?.StartsWith("[m") == true)
            {
                if (tempentry != string.Empty)
                    entries.Add(tempentry);
                tempentry = string.Empty;
            }
            else if(line?.StartsWith("[") == true)
            {
                tempentry = string.IsNullOrEmpty(tempentry) ? line : string.Concat(tempentry, line);
            }
        }
        // Add placeholder entries.
        for (var i = entries.Count; i < 2048; i++)
        {
            entries.Add(isEnglish ? $"[f 0 5 65278][f 2 1]A weapon added with Weapon Framework.{lineEnding}" : $"[f 0 5 65278][f 2 1]ウェポン・フレームワークで追加された武器{lineEnding}");
        }
        // Wrap entires
        for (var i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            var id = $"[msg Item_{i:X3}]";
            var info = new ItemHelpEntry(i, id, entry);
            Entries.Add(info);
        }
    }
    public void SerializeAsUnwrapped()
    {
        var msg = new StringBuilder();
        var hdr = new StringBuilder();
        foreach (var entry in Entries)
        {
            msg.AppendLine(entry.AsMessage());
            hdr.AppendLine(entry.AsHeaderLine());
        }

        if (File.Exists(messageOut))
            File.Delete(messageOut);
        
        if (File.Exists(headerOut))
            File.Delete(headerOut);
        
        File.Create(messageOut);
        File.Create(headerOut);

        //Console.Write(msg.ToString());

        using (var writer = new StreamWriter(messageOut))
        {

            writer.Write(msg.ToString());
            writer.Flush();
            writer.Close();
        }
        using (var writer = new StreamWriter(headerOut))
        {
            writer.Write(hdr.ToString());
            writer.Flush();
            writer.Close();
        }
    }
    static string itemFileName(ItemType type) => $"BMD_Item{type}Help";
    static string AstreaAssetPath(string basePath, ItemType type) => Path.Join(basePath, "P3R", "Content", "Astrea", "Help", itemFileName(type));
    static string AstreaAssetPathEN(string basePath, ItemType type) => Path.Join(basePath, "P3R", "Content", "L10N", "en", "Astrea", "Help", itemFileName(type));
    static string Xrd777AssetPath(string basePath, ItemType type) => Path.Join(basePath, "P3R", "Content", "Xrd777", "Help", itemFileName(type));
    static string Xrd777AssetPathEN(string basePath, ItemType type) => Path.Join(basePath, "P3R", "Content", "L10N", "en", "Xrd777", "Help", itemFileName(type));

    public IEnumerator<ItemHelpEntry> GetEnumerator()
    {
        return ((IEnumerable<ItemHelpEntry>)Entries).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Entries).GetEnumerator();
    }
}

internal static class Generator
{
    
    public static void SerializeAll()
    {
        var astrea = new ItemHelpFile(ItemType.Weapon, AssetDirectory.Astrea);
        var astreaEN = new ItemHelpFile(ItemType.Weapon, AssetDirectory.Astrea, true);
        var xrd777 = new ItemHelpFile(ItemType.Weapon, AssetDirectory.Xrd777);
        var xrd777EN = new ItemHelpFile(ItemType.Weapon,AssetDirectory.Xrd777, true);
        astrea.ReadAssets();
        astreaEN.ReadAssets();
        xrd777.ReadAssets();
        xrd777EN.ReadAssets();


        Console.WriteLine("Writing messages");
        astrea.WriteMsg();
        Console.Write(".");
        astreaEN.WriteMsg();
        Console.Write(".");
        xrd777.WriteMsg();
        Console.Write(".");
        xrd777EN.WriteMsg();
        Console.Write(". Done.");
        Console.Write("\n");
        Console.WriteLine("Writing headers");
        astrea.WriteHeader();
        Console.Write(".");
        astreaEN.WriteHeader();
        Console.Write(".");
        xrd777.WriteHeader();
        Console.Write(".");
        xrd777EN.WriteHeader();
        Console.Write(". Done.");
    }
    public static void WriteMsg(this ItemHelpFile itemHelps)
    {
        var filePath = itemHelps.messageOut;
        var sb = new StringBuilder();
        foreach(var entry in itemHelps)
        {
            sb.AppendLine(entry.AsMessage());
        }
        var fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
        fs.SetLength(0);
        fs.Flush();
        fs.Close();
        var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        var buffer = new StreamWriter(file);
        buffer.Write(sb.ToString());
        buffer.Flush();
        file.Close();
    }
    public static void WriteHeader(this ItemHelpFile itemHelps)
    {
        var filePath = itemHelps.headerOut;
        var sb = new StringBuilder();
        foreach(var entry in itemHelps)
        {
            sb.AppendLine(entry.AsHeaderLine());
        }
        var fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
        fs.SetLength(0);
        fs.Flush();
        fs.Close();
        var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        var buffer = new StreamWriter(file);
        buffer.Write(sb.ToString());
        buffer.Flush();
        file.Close();
    }
}
