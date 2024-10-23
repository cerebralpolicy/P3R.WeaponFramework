using Reloaded.Memory.Extensions;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.WeaponFramework.Weapons.Models;

public class ItemDefConverter : JsonConverter<ItemDef?>
{
    public override ItemDef? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var asString = reader.GetString();
        if(asString == null)
            return null;
        return new(asString);
    }

    public override void Write(Utf8JsonWriter writer, ItemDef? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
public class ItemDefTest
{
    private const int maxLen = 16;
    private readonly string self;
    string acronym;
    public void Preview()
    {
        Console.WriteLine(acronym);
    }
    public ItemDefTest(string modName, int modWeapId)
    {
        var nameParts = modName.Split('.');
        const string PREFIX = "IT_WEA_";
        string[] IgnoredNamespaces = ["p3r", "WeaponFramework", "WF", "Reloaded"];
        bool IgnorePart(string part) => IgnoredNamespaces.Contains(part, StringComparer.OrdinalIgnoreCase);
        string? ACRONYM = string.Empty;
        foreach (string part in nameParts)
        {
            if (IgnorePart(part))
                continue;
            if (!IgnorePart(part))
            {
                var chars = part.Where(char.IsUpper).ToArray();
                ACRONYM = new(chars);
                break;
            }
            ACRONYM = "WF";
        }
        acronym = ACRONYM!;
        var prelimSelf = PREFIX + ACRONYM;
        if (prelimSelf.Length > 14)
        {
            prelimSelf = prelimSelf.Substring(0, 14);
        }

        prelimSelf += $"{modWeapId:D2}";

        if (prelimSelf.Length < maxLen)
        {
            var padLen = (maxLen - prelimSelf.Length);
            prelimSelf.PadRight(padLen, '_');
        }

        self = prelimSelf;
    }
    public void Print()
    {
        Preview();
        Console.WriteLine(self);
    }
}
/// <summary>
/// Special string variant to handle ItemDefs
/// </summary>
[JsonConverter(typeof(ItemDefConverter))]
public unsafe class ItemDef :
    IComparable,
    IEnumerable,
    IConvertible,
    IEnumerable<char>,
    IComparable<ItemDef?>,
    IComparable<string?>,
    IEquatable<ItemDef?>,
    IEquatable<string?>,
    ICloneable,
    ISpanParsable<ItemDef>
{
    private const int maxLen = 16;
    private readonly string self;
    private bool validLen(string input) => input.Length == maxLen;
    #region Interfaces
    public int CompareTo(object? obj)
    {
        return self.CompareTo(obj);
    }

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable)self).GetEnumerator();
    }

    public TypeCode GetTypeCode()
    {
        return self.GetTypeCode();
    }

    public bool ToBoolean(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToBoolean(provider);
    }

    public byte ToByte(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToByte(provider);
    }

    public char ToChar(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToChar(provider);
    }

    public DateTime ToDateTime(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToDateTime(provider);
    }

    public decimal ToDecimal(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToDecimal(provider);
    }

    public double ToDouble(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToDouble(provider);
    }

    public short ToInt16(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToInt16(provider);
    }

    public int ToInt32(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToInt32(provider);
    }

    public long ToInt64(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToInt64(provider);
    }

    public sbyte ToSByte(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToSByte(provider);
    }

    public float ToSingle(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToSingle(provider);
    }

    public string ToString(IFormatProvider? provider)
    {
        return self.ToString(provider);
    }

    public object ToType(Type conversionType, IFormatProvider? provider)
    {
        return ((IConvertible)self).ToType(conversionType, provider);
    }

    public ushort ToUInt16(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToUInt16(provider);
    }

    public uint ToUInt32(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToUInt32(provider);
    }

    public ulong ToUInt64(IFormatProvider? provider)
    {
        return ((IConvertible)self).ToUInt64(provider);
    }

    IEnumerator<char> IEnumerable<char>.GetEnumerator()
    {
        return ((IEnumerable<char>)self).GetEnumerator();
    }

    public object Clone()
    {
        return self.Clone();
    }

    public static ItemDef Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (s.Length > maxLen)
        {
            throw new Exception();
        }
        return new(s.ToString());
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out ItemDef result)
    {
        if (s.Length <= maxLen)
        {
            result = new(s.ToString());
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    public static ItemDef Parse(string s, IFormatProvider? provider)
    {
        return new(s);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ItemDef result)
    {
        result = new(s);
        return result != null;
    }

    public int CompareTo(string? other)
    {
        return self.CompareTo(other);
    }

    public bool Equals(string? other)
    {
        return self.Equals(other);
    }

    public int CompareTo(ItemDef? other)
    {
        Debug.Assert(other != null);
        return self.CompareTo(other.self);
    }

    public bool Equals(ItemDef? other)
    {
        Debug.Assert(other != null);
        return self.Equals(other.self);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(self);
    }


    #endregion
    /// <summary>
    /// Creates an <see cref="ItemDef"/> from a given <see cref="string"/>.
    /// </summary>
    /// <param name="strIn">The source <see cref="string"/>.</param>
    public ItemDef(string? strIn)
    {
        var tempSelf = string.Empty;
        if (strIn != null)
        {
            if (!validLen(strIn))
            {
                if (strIn.Length > 16)
                {
                    var tempArr = strIn.ToCharArray();
                    for (int i = 0; i < maxLen - 1; i++)
                    {
                        tempArr[i] = strIn[i];
                    }
                    tempSelf = string.Concat(tempArr);
                }
                else if (strIn.Length < 16)
                {
                    var padLen = 16 - strIn.Length;
                    tempSelf = strIn.PadRight(padLen, '_');
                }
            }
            else if (validLen(strIn))
            {
                tempSelf = strIn;
            }
        }

        self = tempSelf;
    }
    public ItemDef(string modName, int modWeapId)
    {
        var nameParts = modName.Split('.');
        const string PREFIX = "IT_WEA_";
        string[] IgnoredNamespaces = ["p3r", "WeaponFramework", "WF", "Reloaded"];
        bool IgnorePart(string part) => IgnoredNamespaces.Contains(part, StringComparer.OrdinalIgnoreCase);
        string? ACRONYM = string.Empty;
        foreach (string part in nameParts)
        {
            if (IgnorePart(part))
                continue;
            if (!IgnorePart(part))
            {
                var chars = part.Where(char.IsUpper).ToArray();
                ACRONYM = new(chars);
                break;
            }
        }
        var prelimSelf = PREFIX + ACRONYM;
        if (prelimSelf.Length > 14)
        {
            prelimSelf = prelimSelf.Substring(0, 14);
        }

        prelimSelf += $"{modWeapId}";

        if (prelimSelf.Length < maxLen)
        {
            var padLen = (maxLen - prelimSelf.Length);
            prelimSelf.PadRight(padLen, '_');
        }

        self = prelimSelf;
    }
    
    public static implicit operator ItemDef?(string? str) => new(str);
    public static implicit operator string?(ItemDef? itemDef) => itemDef?.self;

    public ItemDef FromFString(Types.FString fString)
    {
        var conv = fString.ToString();
        Debug.Assert(conv != null);
        return new(conv);
    }

    public Types.FString ToFString(IUnreal unreal) => new(unreal, self);
    public Types.FString ToFString(IMemoryMethods memory) => new(memory, self);

    public static bool operator <(ItemDef left, ItemDef? right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(ItemDef left, ItemDef? right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(ItemDef left, ItemDef? right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(ItemDef left, ItemDef? right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <(ItemDef left, string? right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(ItemDef left, string? right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(ItemDef left, string? right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(ItemDef left, string? right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator ==(ItemDef? left, ItemDef? right)
    {
        return EqualityComparer<ItemDef>.Default.Equals(left, right);
    }

    public static bool operator !=(ItemDef? left, ItemDef? right)
    {
        return !(left == right);
    }

    public bool Equals(string? other, StringComparison comparison)
    {
        return self.Equals(other, comparison);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ItemDef);
    }

    public override string? ToString()
    {
        return self.ToString();
    }
}
