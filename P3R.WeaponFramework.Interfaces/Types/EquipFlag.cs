namespace P3R.WeaponFramework.Interfaces.Types
{
    [Flags]
    public enum EquipFlag
    {
        NONE = 0,
        Player = 1 << 1,
        Yukari = 1 << 2,
        Stupei = 1 << 3,
        Akihiko = 1 << 4,
        Mitsuru = 1 << 5,
        Fuuka = 1 << 6,
        Aigis = 1 << 7,
        Ken = 1 << 8,
        Koromaru = 1 << 9,
        Shinjiro = 1 << 10,
        Metis = 1 << 11,
    }
}
namespace P3R.WeaponFramework.Interfaces{
    using P3R.WeaponFramework.Interfaces.Types;
    public static partial class AssetUtils
    {
        public static EquipFlag ToEquipFlag(this Character character)
        => character switch
        {
            Character.NONE => EquipFlag.NONE,
            Character.Player => EquipFlag.Player,
            Character.Yukari => EquipFlag.Yukari,
            Character.Stupei => EquipFlag.Stupei,
            Character.Akihiko => EquipFlag.Akihiko,
            Character.Mitsuru => EquipFlag.Mitsuru,
            Character.Fuuka => EquipFlag.Fuuka,
            Character.Aigis => EquipFlag.Aigis,
            Character.Ken => EquipFlag.Ken,
            Character.Koromaru => EquipFlag.Koromaru,
            Character.Shinjiro => EquipFlag.Shinjiro,
            Character.Metis => EquipFlag.Metis,
            Character.FEMC => EquipFlag.Player,
            _ => throw new NotImplementedException(),
        };
        public static Character GetCharacter(this EquipFlag flag)
            => flag switch
            {
                EquipFlag.NONE => Character.NONE,
                EquipFlag.Player => Character.Player,
                EquipFlag.Yukari => Character.Yukari,
                EquipFlag.Stupei => Character.Stupei,
                EquipFlag.Akihiko => Character.Akihiko,
                EquipFlag.Mitsuru => Character.Mitsuru,
                EquipFlag.Fuuka => Character.Fuuka,
                EquipFlag.Aigis => Character.Aigis,
                EquipFlag.Ken => Character.Ken,
                EquipFlag.Koromaru => Character.Koromaru,
                EquipFlag.Shinjiro => Character.Shinjiro,
                EquipFlag.Metis => Character.Metis,
                _ => throw new NotImplementedException(),
            };
    }
}