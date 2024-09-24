using AtlusScriptLibrary.Common.IO;
using AtlusScriptLibrary.Common.Libraries;
using AtlusScriptLibrary.Common.Text;
using AtlusScriptLibrary.FlowScriptLanguage;
using AtlusScriptLibrary.FlowScriptLanguage.Decompiler;
using AtlusScriptLibrary.MessageScriptLanguage;
using AtlusScriptLibrary.MessageScriptLanguage.Decompiler;
using P3R.WeaponFramework.Tools.DataUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using FSL = AtlusScriptLibrary.FlowScriptLanguage;
using MSL = AtlusScriptLibrary.MessageScriptLanguage;


namespace P3R.WeaponFramework.Tools.DataUtils;

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
            var script = MessageScript.FromStream(proxy,MSLFormatVersion,Encoding);
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
