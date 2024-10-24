﻿using P3R.WeaponFramework.Types;
using System.Runtime.InteropServices;

namespace P3R.WeaponFramework.Tools.DataUtils;

public struct WeaponRaw
{
    public WeaponRaw(string name, int sortNum, int weaponType, EEquipFlag equipID, WeaponStats weaponStats, int getFLG, int modelID, int flags)
    {
        Name = name;
        SortNum = sortNum;
        WeaponType = weaponType;
        EquipID = equipID;
        WeaponStats = weaponStats;
        GetFLG = getFLG;
        ModelID = modelID;
        Flags = flags;
    }

    public string Name { get; set; }
    public int SortNum { get; set; }
    public int WeaponType { get; set; }
    public EEquipFlag EquipID { get; set; }
    public WeaponStats WeaponStats { get; set; }
    public int GetFLG { get; set; }
    public int ModelID { get; set; }
    public int Flags { get; set; }
}