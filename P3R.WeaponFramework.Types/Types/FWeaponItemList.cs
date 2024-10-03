using P3R.WeaponFramework.Weapons.Models;
using System.Runtime.InteropServices;
using System.Text;

namespace P3R.WeaponFramework.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x44)]
public struct FWeaponItemList
{
    [FieldOffset(0x0000)] public string ItemDef;
    [FieldOffset(0x0010)] public ushort SortNum;
    [FieldOffset(0x0014)] public EWeaponType WeaponType;
    [FieldOffset(0x0018)] public EEquipFlag EquipID;
    [FieldOffset(0x001C)] public EBtlDataAttr AttrID;
    [FieldOffset(0x001E)] public ushort Rarity;
    [FieldOffset(0x0020)] public ushort Tier;
    [FieldOffset(0x0022)] public ushort Attack;
    [FieldOffset(0x0024)] public ushort Accuracy;
    [FieldOffset(0x0026)] public ushort Strength;
    [FieldOffset(0x0028)] public ushort Magic;
    [FieldOffset(0x002A)] public ushort Endurance;
    [FieldOffset(0x002C)] public ushort Agility;
    [FieldOffset(0x002E)] public ushort Luck;
    [FieldOffset(0x0030)] public EItemSkillId SkillID;
    [FieldOffset(0x0034)] public uint Price;
    [FieldOffset(0x0038)] public uint SellPrice;
    [FieldOffset(0x003C)] public ushort GetFLG;
    [FieldOffset(0x003E)] public ushort ModelID;
    [FieldOffset(0x0040)] public uint Flags;

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("");
        var fields = typeof(FWeaponItemList).GetFields();
        foreach ( var field in fields )
        {
            var info = $"{field.Name} {field.GetValue(this)}\n";
            sb.Append(info);
        }
        return sb.ToString();
    }
}