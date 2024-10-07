using AtlusScriptLibrary.Common.IO;
using AtlusScriptLibrary.Common.Libraries;
using AtlusScriptLibrary.Common.Text;
using AtlusScriptLibrary.FlowScriptLanguage;
using AtlusScriptLibrary.FlowScriptLanguage.Decompiler;
using AtlusScriptLibrary.MessageScriptLanguage;
using AtlusScriptLibrary.MessageScriptLanguage.Decompiler;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using FSL = AtlusScriptLibrary.FlowScriptLanguage;
using MSL = AtlusScriptLibrary.MessageScriptLanguage;
using Path = System.IO.Path;


namespace P3R.WeaponFramework.DataGUI.Subroutines;


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
}

internal class ItemHelpFile : IReadOnlyList<ItemHelpEntry>
{
    static string BasePath = Path.Join(Assembly.GetExecutingAssembly().Location,"AtlusData");
    static string[] PathEndings = [".uasset", "_unrwrapped.bmd", "_unrwrapped.bmd.msg", "_unrwrapped.bmd.msg.h"];
    private ItemType type;
    private string uasset;
    private string unwrapped;
    private string message;
    private string header;
    private bool isEnglish;

    public List<ItemHelpEntry> Entries { get; private set; } = [];

    public int Count => ((IReadOnlyCollection<Subroutines.ItemHelpEntry>)Entries).Count;

    public ItemHelpEntry this[int index] => ((IReadOnlyList<Subroutines.ItemHelpEntry>)Entries)[index];

    public ItemHelpFile(ItemType type, AssetDirectory directory, bool english = false)
    {
        isEnglish = english;
        string embed = string.Empty;
        this.type = type;
        if (directory == AssetDirectory.Astrea)
        {
            if (english)
                embed = AstreaAssetPathEN(type);
            else
                embed = AstreaAssetPath(type);
        }
        else
        {
            if (english)
                embed = Xrd777AssetPathEN(type);
            else
                embed = Xrd777AssetPath(type);
        }
        uasset = embed + PathEndings[0];
        unwrapped = embed + PathEndings[1];
        message = embed + PathEndings[2];
        header = embed + PathEndings[3];
    }
    public void ReadAsset()
    {
        List<string> entries = [];
        var lineEnding = isEnglish ? "[n][e]" : "[n][f 1 5 0][e]";
        using var stream = File.OpenRead(message);
        if (stream == null)
            throw new NullReferenceException(nameof(stream));
        using var reader = new StreamReader(stream);
        int entryTally = 0;
        while (!reader.EndOfStream)
        {
            string entry = "";
            var line = reader.ReadLine();
            if (line?.StartsWith("[msg Item") == true||string.IsNullOrEmpty(line))
            {
                if (!string.IsNullOrEmpty(entry))
                { 
                    entries.Add(entry);
                    entryTally++;
                }
                entry = string.Empty; 
            }
            else
            {
                entry = string.Concat(entry, line);
            }
        }
        // Add placeholder entries.
        for (var i = entryTally; i < 2048; i++)
        {
            entries.Add(isEnglish ? $"[f 0 5 65278][f 2 1]A weapon added with Weapon Framework.{lineEnding}" : $"[f 0 5 65278][f 2 1]ウェポン・フレームワークで追加された武器{lineEnding}");
        }
        // Wrap entires
        foreach (var entry in entries)
        {
            var index = entries.IndexOf(entry);
            var id = $"[Msg Item_{index:X3}]";
            var info = new ItemHelpEntry(index, id, entry);
            Entries.Add(info);
        }
    }

    static string itemFileName(ItemType type) => $"BMD_Item{type}Help";
    static string AstreaAssetPath(ItemType type) => Path.Join(BasePath, "P3R", "Content", "Astrea", "Help", itemFileName(type));
    static string AstreaAssetPathEN(ItemType type) => Path.Join(BasePath, "P3R", "Content", "Astrea", "L10N", "en", "Help", itemFileName(type));
    static string Xrd777AssetPath(ItemType type) => Path.Join(BasePath, "P3R", "Content", "Xrd777", "Help", itemFileName(type));
    static string Xrd777AssetPathEN(ItemType type) => Path.Join(BasePath, "P3R", "Content", "Xrd777", "L10N", "en", "Help", itemFileName(type));

    public IEnumerator<ItemHelpEntry> GetEnumerator()
    {
        return ((IEnumerable<Subroutines.ItemHelpEntry>)Entries).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Entries).GetEnumerator();
    }
}

internal static class PathExtensions
{
    public static string GetEmbeddedFileName(this string path) => Path.GetFileNameWithoutExtension(path).Split('.').Last();
}

internal static class EmbeddedPath
{

    public static string Join(string? path1, string? path2) => string.Join('.',path1, path2);
    public static string Join(string? path1, string? path2, string? path3) => string.Join('.', path1, path2, path3);
    public static string Join(string? path1, string? path2, string? path3, string? path4) => string.Join('.',path1,path2,path3, path4);
    public static string Join(params string?[] paths) => string.Join('.',paths);
}


internal class AtlusScriptHandler
{
    private static Assembly ThisAssembly => Assembly.GetExecutingAssembly();
    private static Encoding Encoding => Encoding.UTF8;
    private const MSL.FormatVersion MSLFormatVersion = MSL.FormatVersion.Version1Reload;
    private const FSL.FormatVersion FSLFormatVersion = FSL.FormatVersion.Version4;
    private const FSL.FormatVersion FSLFormatVersionBE = FSL.FormatVersion.Version4BigEndian;
    private static Library Library => LibraryLookup.GetLibrary("p3re");
    //FROM UEWrapper.cs
    public void TryDecompile(string path, string outputfolder)
    {
        var embedded = IsEmbedded(path, out var resourceStream);
        var stream = resourceStream ?? File.OpenRead(path);
        var unreal = IsUEFile(path);
        var buffer = unreal ? UEWrapper.UnwrapStream(stream) : new BinaryReader(stream).ReadBytes((int)stream.Length);
        var filename = embedded ? path.GetEmbeddedFileName() : Path.GetFileNameWithoutExtension(path);
        var isBF = filename.StartsWith("BF");
        var ext = isBF ? ".bf" : ".bmd";
        var proxy = new MemoryStream(buffer);
        if (!isBF)
        {
            var script = MessageScript.FromStream(proxy, MSLFormatVersion, Encoding);
            using (var decompiler = new MessageScriptDecompiler(new FileTextWriter(Path.Combine(outputfolder, $"{filename}{ext}"))))
            {
                decompiler.Decompile(script);
            }
        }
        else
        {
            var script = FlowScript.FromStream(stream, Encoding, version: FSLFormatVersion, false);
            var decompiler = new FlowScriptDecompiler();
            decompiler.TryDecompile(script, Path.Combine(outputfolder, $"{filename}{ext}"));
        }
    }
    public static bool IsEmbedded(string path, out Stream? resourceStream)
    {
        if (ThisAssembly == null)
        {
            throw new TargetException("Cannot get assembly.");
        }
        var resources = ThisAssembly.GetManifestResourceNames();
        var check = resources.Contains(path);
        resourceStream = check ? ThisAssembly.GetManifestResourceStream(path) : null;
        return check;
    }
    public static bool IsUEFile(string path)
    {
        string[] extensions = [".uasset", ".uexp"];
        var extension = Path.GetExtension(path);
        return extensions.Contains(extension);
    }
    #region UEWrapper
    public class UEWrapper
    {

        public static string[] constantCommonImports =
        {
        "/Script/CoreUObject", "ArrayProperty", "Class", "mBuf", "None", "Package"
    };
        public static string[] constantBfImoprts =
        {
        "/Script/BfAssetPlugin", "ByteProperty", "BfAsset", "Default__BfAsset"
    };
        public static string[] constantBmdImoprts =
        {
        "/Script/BmdAssetPlugin", "Int8Property", "BmdAsset", "Default__BmdAsset"
    };

        public static uint AlgorithmHash = 0xC1640000;

        public static byte[] ExpSection3 = { 0xa, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };


        public static int FORMATTING_SIZE = 0x25; // from beginning of "uexp" portion to start of bf block
        public static byte[] UnwrapStream(Stream stream)
        {
            var endianReader = new EndianBinaryReader(stream, Endianness.LittleEndian); // UE stuff is in little endian
            var packageHeader = new FPackageSummaryHeader(endianReader);
            endianReader.SeekBegin(packageHeader.ExportMapOffset);
            var exportMapEntry = new FExportMapEntry(endianReader);
            var bfSize = exportMapEntry.CookedSerialSize - (ulong)FORMATTING_SIZE; // length of data preceeding BF
            endianReader.SeekBegin(packageHeader.GraphDataOffset + packageHeader.GraphDataSize + FORMATTING_SIZE);
            return endianReader.ReadBytes((int)bfSize);
        }
        public static bool UnwrapAsset(string dir, string name, string ext, Stream stream, out string outName)
        {
            var endianReader = new EndianBinaryReader(stream, Endianness.LittleEndian); // UE stuff is in little endian
            var packageHeader = new FPackageSummaryHeader(endianReader);
            endianReader.SeekBegin(packageHeader.ExportMapOffset);
            var exportMapEntry = new FExportMapEntry(endianReader);
            var bfSize = exportMapEntry.CookedSerialSize - (ulong)FORMATTING_SIZE; // length of data preceeding BF
            endianReader.SeekBegin(packageHeader.GraphDataOffset + packageHeader.GraphDataSize + FORMATTING_SIZE);
            byte[] buffer = endianReader.ReadBytes((int)bfSize);
            outName = Path.Combine(dir, $"{name}_unwrapped{ext}");
            using (var fileOut = File.Create(outName)) { fileOut.Write(buffer, 0, buffer.Length); }
            return false;
        }

        public static void WriteFString16(EndianBinaryWriter writer, string text)
        {
            writer.Write((byte)0);
            if (text.Length > 0xff) throw new Exception($"Name \"{text}\" is too long to converted into FString");
            writer.Write((byte)text.Length);
            writer.Write(Encoding.ASCII.GetBytes(text));
        }

        public static bool WrapAsset(string inFileName, string patchFileName)
        {
            using
            (FileStream
                payloadFile = File.Open(inFileName, FileMode.Open), // the file that we've just compiled
                wrapperFile = File.Open(patchFileName, FileMode.Open),
                outFile = File.Create(inFileName + ".uasset")
            )
            {
                var wrapperReader = new EndianBinaryReader(wrapperFile, Endianness.LittleEndian); // .uasset
                var outFileEndian = new EndianBinaryWriter(outFile, Endianness.LittleEndian);
                var packageHeader = new FPackageSummaryHeader(wrapperReader);
                // everything up until ExportMap is the same
                wrapperReader.SeekBegin(0); // go back to beginning
                outFileEndian.Write(wrapperReader.ReadBytes((int)(packageHeader.ExportMapOffset)));
                var exportHeader = new FExportMapEntry(wrapperReader);
                exportHeader.CookedSerialSize = (ulong)payloadFile.Length + 0x25 + 0xc;
                exportHeader.Write(outFileEndian);
                // the rest of the package header is the same
                outFileEndian.Write(wrapperReader.ReadBytes((int)(packageHeader.GraphDataOffset + packageHeader.GraphDataSize - outFileEndian.Position)));
                outFileEndian.Write(wrapperReader.ReadBytes(0x10)); // Read first 0x10 bytes (same)
                outFileEndian.Write((int)(payloadFile.Length + 0x4));
                wrapperReader.SeekCurrent(4); // sizeof(uint)
                outFileEndian.Write(wrapperReader.ReadBytes(0xd));
                outFileEndian.Write((int)payloadFile.Length);
                byte[] payloadData = new byte[payloadFile.Length];
                payloadFile.Read(payloadData, 0, (int)payloadFile.Length);
                outFileEndian.Write(payloadData);
                outFileEndian.Write(ExpSection3);
            }
            return true;
        }
    }

    public class FPackageObjectIndex
    {
        public static int SerializedLength = 0x8;
    }

    public class FExportBundleHeader
    {
        public static int SerializedLength = 0x8;
    }

    public class FExportBundleEntry
    {
        public static int SerializedLength = 0x8;
    }

    public class FPackageSummaryHeader
    {
        public static int SerializedLength = 0x40;

        public ulong Name;
        public ulong SourceName;
        public uint PackageFlags;
        public uint CookedHeaderSize;
        public uint NameMapNamesOffset;
        public uint NameMapNamesSize;
        public uint NameMapHashesOffset;
        public uint NameMapHashesSize;
        public uint ImportMapOffset;
        public uint ExportMapOffset;
        public uint ExportBundlesOffset;
        public uint GraphDataOffset;
        public uint GraphDataSize;
        public uint Padding;
        public FPackageSummaryHeader(EndianBinaryReader reader)
        {
            Name = reader.ReadUInt64(); // Name
            SourceName = reader.ReadUInt64(); // SourceName
            PackageFlags = reader.ReadUInt32(); // PackageFlags
            CookedHeaderSize = reader.ReadUInt32(); // CookedHeaderSize
            NameMapNamesOffset = reader.ReadUInt32(); // NameMapNamesOffset
            NameMapNamesSize = reader.ReadUInt32(); // NameMapNamesSize
            NameMapHashesOffset = reader.ReadUInt32(); // NameMapHashesOffset
            NameMapHashesSize = reader.ReadUInt32(); // NameMapHashesSIze
            ImportMapOffset = reader.ReadUInt32(); // ImportMapOffset
            ExportMapOffset = reader.ReadUInt32(); // ExportMapOffset
            ExportBundlesOffset = reader.ReadUInt32(); // ExportBudlesOffset
            GraphDataOffset = reader.ReadUInt32(); // GraphDataOffset
            GraphDataSize = reader.ReadUInt32(); // GraphDataSize
            Padding = reader.ReadUInt32(); // Padding
        }
    }

    public class FExportMapEntry
    {
        public static int SerializedLength = 0x48;

        public ulong CookedSerialOffset;
        public ulong CookedSerialSize;
        public ulong ObjectName;
        public ulong OuterIndex;
        public ulong ClassIndex;
        public ulong SuperIndex;
        public ulong TemplateIndex;
        public ulong GlobalImportIndex;
        public int ObjectFlags;
        public byte FilterFlags;
        public byte[] unk;
        public FExportMapEntry(EndianBinaryReader reader)
        {
            CookedSerialOffset = reader.ReadUInt64(); // CookedSerialOffset
            CookedSerialSize = reader.ReadUInt64(); // CookedSerialSize
            ObjectName = reader.ReadUInt64();
            OuterIndex = reader.ReadUInt64();
            ClassIndex = reader.ReadUInt64();
            SuperIndex = reader.ReadUInt64();
            TemplateIndex = reader.ReadUInt64();
            GlobalImportIndex = reader.ReadUInt64();
            ObjectFlags = reader.ReadInt32();
            FilterFlags = reader.ReadByte();
            unk = reader.ReadBytes(3);
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(CookedSerialOffset);
            writer.Write(CookedSerialSize);
            writer.Write(ObjectName);
            writer.Write(OuterIndex);
            writer.Write(ClassIndex);
            writer.Write(SuperIndex);
            writer.Write(TemplateIndex);
            writer.Write(GlobalImportIndex);
            writer.Write(ObjectFlags);
            writer.Write(FilterFlags);
            writer.Write(unk);
        }
    }
    /*
    public class FString // FString32NoHash in UTOC Emulator
    {
        public static unsafe string Read(EndianBinaryReader reader)
        {
            int Length = reader.ReadInt32();
            byte[] bytes = reader.ReadBytes(Length);
            return Marshal.PtrToStringAnsi((IntPtr)(&bytes));
        }
    }

    public class FField
    {
        public uint Type;
        public uint Name;
        public uint Flags;
        public FField(EndianBinaryReader reader)
        {
            Type = reader.ReadUInt32();
            Name = reader.ReadUInt32();
            Flags = reader.ReadUInt32();
        }
    }
    */
    #endregion
}
