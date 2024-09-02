using P3R.WeaponFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Research.Types
{
    public struct Weapon
    {
        public Character Character { get; set; }
        public string? Name { get; set; }
        public uint ModelId { get; set; }
        public uint WeaponModelId { get; set; }
        public WeaponStats Stats { get; set; }

        public Weapon(FWeaponItemList weaponItemList)
        { 
            Character = weaponItemList.EquipID.GetCharacter();
            ModelId = weaponItemList.ModelID;
            WeaponModelId = Assets.ModelPairsUInt[ModelId];
            Stats = new(weaponItemList);
        }

        public static implicit operator Weapon(FWeaponItemList weaponItemList)
        {
            return new Weapon(weaponItemList);
        }
    }
}
