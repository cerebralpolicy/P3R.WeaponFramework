using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Tools.DataUtils;

public struct Weapon
{
    public Weapon(ECharacter character, int uniqueID, string name, int weaponType, int modelId, int weaponModelId, WeaponStats stats)
    {
        Character = character;
        UniqueID = uniqueID;
        Name = name;
        WeaponType = weaponType;
        ModelId = modelId;
        WeaponModelId = weaponModelId;
        Stats = stats;
    }

    public ECharacter Character { get; set; }
    public int UniqueID { get; set; }
    public string Name { get; set; }
    public int WeaponType { get; set; }
    public int ModelId { get; set; }
    public int WeaponModelId { get; set; }
    public WeaponStats Stats { get; set; }


}
