using P3R.WeaponFramework.Weapons.Models;
using System.Runtime.InteropServices;
using System.Text;

namespace P3R.WeaponFramework.Types;

public unsafe static class FWeaponItem
{

}

[StructLayout(LayoutKind.Explicit, Size = 0x44)]
public unsafe struct FWeaponItemList : IEquatable<FWeaponItemList>
{
    [FieldOffset(0x0000)] public FString ItemDef;
    [FieldOffset(0x0010)] public ushort SortNum;
  //[FieldOffset(0x0014)] public EWeaponType WeaponType;
    [FieldOffset(0x0014)] public uint WeaponType;
  //[FieldOffset(0x0018)] public EEquipFlag EquipID;
    [FieldOffset(0x0018)] public uint EquipID;
  //[FieldOffset(0x001C)] public EBtlDataAttr AttrID;
    [FieldOffset(0x001C)] public ushort AttrID;
    [FieldOffset(0x001E)] public ushort Rarity;
    [FieldOffset(0x0020)] public ushort Tier;
    [FieldOffset(0x0022)] public ushort Attack;
    [FieldOffset(0x0024)] public ushort Accuracy;
    [FieldOffset(0x0026)] public ushort Strength;
    [FieldOffset(0x0028)] public ushort Magic;
    [FieldOffset(0x002A)] public ushort Endurance;
    [FieldOffset(0x002C)] public ushort Agility;
    [FieldOffset(0x002E)] public ushort Luck;
  //[FieldOffset(0x0030)] public EItemSkillId SkillID;
    [FieldOffset(0x0030)] public uint SkillID;
    [FieldOffset(0x0034)] public uint Price;
    [FieldOffset(0x0038)] public uint SellPrice;
    [FieldOffset(0x003C)] public ushort GetFLG;
    [FieldOffset(0x003E)] public ushort ModelID;
    [FieldOffset(0x0040)] public uint Flags;

    
    public static implicit operator FWeaponItemList(Weapon weapon) => new FWeaponItemList(weapon);
    public FWeaponItemList(IMemoryMethods memory, Weapon weapon)
    {
        var fstring = new FString(memory,weapon.Name!);
        ItemDef = fstring;
        ushort NullableField(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (ushort)statField.Value;
        }
        uint NullablePrice(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (uint)statField.Value;
        }
        SortNum = (ushort)weapon.SortNum;
        WeaponType = (uint)weapon.Character.ToWeaponType();
        EquipID = (uint)weapon.Character.ToEquipID();
        var stats = weapon.Stats;
        Rarity = (ushort)stats.Rarity;
        Tier = (ushort)stats.Tier;
        Attack = (ushort)stats.Attack;
        Accuracy = (ushort)stats.Accuracy;
        Strength = NullableField(stats.Strength);
        Magic = NullableField(stats.Magic);
        Endurance = NullableField(stats.Endurance);
        Agility = NullableField(stats.Agility);
        Luck = NullableField(stats.Luck);
        Price = NullablePrice(stats.Price);
        SellPrice = NullablePrice(stats.SellPrice);
        SkillID = (uint)stats.SkillId;
        GetFLG = 0;
        ModelID = (ushort)weapon.ModelId;
        Flags = 0;
    }
    public FWeaponItemList(Weapon weapon)
    {
        ushort NullableField(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (ushort)statField.Value;
        }
        uint NullablePrice(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (uint)statField.Value;
        }
        SortNum = (ushort)weapon.SortNum;
        WeaponType = (uint)weapon.Character.ToWeaponType();
        EquipID = (uint)weapon.Character.ToEquipID();
        var stats = weapon.Stats;
        Rarity = (ushort)stats.Rarity;
        Tier = (ushort)stats.Tier;
        Attack = (ushort)stats.Attack;
        Accuracy = (ushort)stats.Accuracy;
        Strength = NullableField(stats.Strength);
        Magic = NullableField(stats.Magic);
        Endurance = NullableField(stats.Endurance);
        Agility = NullableField(stats.Agility);
        Luck = NullableField(stats.Luck);
        Price = NullablePrice(stats.Price);
        SellPrice = NullablePrice(stats.SellPrice);
        SkillID = (uint)stats.SkillId;
        GetFLG = 0;
        ModelID = (ushort)weapon.ModelId;
        Flags = 0;
    }
    private unsafe FWeaponItemList* AsUnsafe(FWeaponItemList list) => &list;
    private unsafe void ApplyData(FWeaponItemList* list)
    {
        ItemDef = list->ItemDef;
        SortNum = list->SortNum;
        WeaponType = list->WeaponType;
        EquipID = list->EquipID;
        Rarity = list->Rarity;
        Tier = list->Tier;
        Attack = list->Attack;
        Accuracy = list->Accuracy;
        Strength = list->Strength;
        Magic = list->Magic;
        Endurance = list->Endurance;
        Agility = list->Agility;
        Luck = list->Luck;
        Price = list->Price;
        SellPrice = list->SellPrice;
        SkillID = list->SkillID;
        GetFLG = list->GetFLG;
        ModelID = list->ModelID;
        Flags = list->Flags;
    }
    public unsafe FWeaponItemList* Malloc(IMemoryMethods memory)
    {
        var data = AsUnsafe(this);
        var unsafeItem = memory.FMemory_Malloc<FWeaponItemList>();
        unsafeItem->ApplyData(data);
        return unsafeItem;
    }
    public unsafe FWeaponItemList* Malloc(IMemoryMethods memory, Weapon weapon)
    {
        var unsafeItem = memory.FMemory_Malloc<FWeaponItemList>();
        unsafeItem->SetFromWeapon(weapon);
        return unsafeItem;
    }
    public void SetFromWeapon(Weapon weapon)
    {
        ushort NullableField(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (ushort)statField.Value;
        }
        uint NullablePrice(int? statField)
        {
            if (statField == null)
                return 0;
            else
                return (uint)statField.Value;
        }
        SortNum = (ushort)weapon.SortNum;
        WeaponType = (uint)weapon.Character.ToWeaponType();
        EquipID = (uint)weapon.Character.ToEquipID();
        var stats = weapon.Stats;
        Rarity = (ushort)stats.Rarity;
        Tier = (ushort)stats.Tier;
        Attack = (ushort)stats.Attack;
        Accuracy = (ushort)stats.Accuracy;
        Strength = NullableField(stats.Strength);
        Magic = NullableField(stats.Magic);
        Endurance = NullableField(stats.Endurance);
        Agility = NullableField(stats.Agility);
        Luck = NullableField(stats.Luck);
        Price = NullablePrice(stats.Price);
        SellPrice = NullablePrice(stats.SellPrice);
        SkillID = (uint)stats.SkillId;
        GetFLG = 0;
        ModelID = (ushort)weapon.ModelId;
        Flags = 0;
    }

    public override bool Equals(object? obj)
    {
        return obj is FWeaponItemList list && Equals(list);
    }

    public bool Equals(FWeaponItemList other)
    {
        return SortNum == other.SortNum &&
               WeaponType == other.WeaponType &&
               EquipID == other.EquipID &&
               AttrID == other.AttrID &&
               Rarity == other.Rarity &&
               Tier == other.Tier &&
               Attack == other.Attack &&
               Accuracy == other.Accuracy &&
               Strength == other.Strength &&
               Magic == other.Magic &&
               Endurance == other.Endurance &&
               Agility == other.Agility &&
               Luck == other.Luck &&
               SkillID == other.SkillID &&
               Price == other.Price &&
               SellPrice == other.SellPrice &&
               GetFLG == other.GetFLG &&
               ModelID == other.ModelID &&
               Flags == other.Flags;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(SortNum);
        hash.Add(WeaponType);
        hash.Add(EquipID);
        hash.Add(AttrID);
        hash.Add(Rarity);
        hash.Add(Tier);
        hash.Add(Attack);
        hash.Add(Accuracy);
        hash.Add(Strength);
        hash.Add(Magic);
        hash.Add(Endurance);
        hash.Add(Agility);
        hash.Add(Luck);
        hash.Add(SkillID);
        hash.Add(Price);
        hash.Add(SellPrice);
        hash.Add(GetFLG);
        hash.Add(ModelID);
        hash.Add(Flags);
        return hash.ToHashCode();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"\nModelID: {ModelID}");
        sb.Append($"\n{nameof(EquipID)}: {(EEquipFlag)EquipID}");
        sb.Append($"\n{nameof(SortNum)}: {SortNum}");
        sb.Append("\nStats:");
        sb.Append($"\n\t{nameof(Attack)}: {Attack}");
        sb.Append($"\n\t{nameof(Accuracy)}: {Accuracy}");
        sb.Append($"\n\t{nameof(Strength)}: {Strength}");
        sb.Append($"\n\t{nameof(Magic)}: {Magic}");
        sb.Append($"\n\t{nameof(Endurance)}: {Endurance}");
        sb.Append($"\n\t{nameof(Agility)}: {Agility}");
        sb.Append($"\n\t{nameof(Luck)}: {Luck}");
        sb.Append($"\n\t{nameof(Price)}: {Price}");
        
        return sb.ToString();
    }

    public static bool operator ==(FWeaponItemList left, FWeaponItemList right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FWeaponItemList left, FWeaponItemList right)
    {
        return !(left == right);
    }
}