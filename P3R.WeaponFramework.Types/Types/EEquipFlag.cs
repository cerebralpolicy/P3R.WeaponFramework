namespace P3R.WeaponFramework.Types;
[Flags]
public enum EEquipFlag : uint
{
    NONE = 1 << 0,
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
public enum EWeaponType : uint
{
    NONE = 1 << 0,
    Player = 1 << 1,
    Yukari = 1 << 2,
    Stupei = 1 << 3,
    Akihiko = 1 << 4,
    Mitsuru = 1 << 5,
    Aigis = 1 << 6,
    Ken = 1 << 7,
    Koromaru = 1 << 8,
    Shinjiro = 1 << 9,
    Metis = 1 << 10,
}

public struct EquipFlagDef
{
    
    public EquipFlagDef() { }

    public bool Player;
    public bool Yukari;
    public bool Stupei;
    public bool Akihiko;
    public bool Mitsuru;
    public bool Fuuka;
    public bool Ken;
    public bool Koromaru;
    public bool Shinjiro;
    public bool Metis;

    private bool isValid
    {
        get
        {
            bool[] nonPlayer = [Yukari, Stupei, Akihiko, Mitsuru, Fuuka, Ken, Koromaru, Shinjiro, Metis];
            if (nonPlayer.Count(x => (x == true)) > 1)
                return false;
            return true;
        }
    }
    public static implicit operator EquipFlagDef(EEquipFlag flag) 
    {
        bool HasFlag(EEquipFlag chara) 
        {
            return (chara & flag) == chara;
        }
        return new EquipFlagDef() 
        { 
            Player = HasFlag(EEquipFlag.Player),
            Yukari = HasFlag(EEquipFlag.Yukari),
            Stupei = HasFlag(EEquipFlag.Stupei),
            Akihiko = HasFlag(EEquipFlag.Akihiko),
            Mitsuru = HasFlag(EEquipFlag.Mitsuru),
            Fuuka = HasFlag(EEquipFlag.Fuuka),
            Ken = HasFlag(EEquipFlag.Ken),
            Koromaru = HasFlag(EEquipFlag.Koromaru),
            Shinjiro = HasFlag(EEquipFlag.Ken),
            Metis = HasFlag(EEquipFlag.Metis),
        };
    }
    public static implicit operator EEquipFlag?(EquipFlagDef flagDef)
    {
        if (!flagDef.isValid) return null;
        uint bitMask = 0;
        if (flagDef.Player)
            bitMask += (uint)EEquipFlag.Player;
        if (flagDef.Yukari)
            bitMask += (uint)EEquipFlag.Yukari;
        if (flagDef.Stupei)
            bitMask += (uint)EEquipFlag.Stupei;
        if (flagDef.Akihiko)
            bitMask += (uint)EEquipFlag.Akihiko;
        if (flagDef.Mitsuru)
            bitMask += (uint)EEquipFlag.Mitsuru;
        if (flagDef.Fuuka)
            bitMask += (uint)EEquipFlag.Fuuka;
        if (flagDef.Ken)
            bitMask += (uint)EEquipFlag.Ken;
        if (flagDef.Koromaru) 
            bitMask += (uint)EEquipFlag.Koromaru;
        if (flagDef.Shinjiro)
            bitMask += (uint)EEquipFlag.Shinjiro;
        if (flagDef.Metis)
            bitMask += (uint)EEquipFlag.Metis;
        return (EEquipFlag)bitMask;
    }
}