namespace P3R.WeaponFramework.Types;
[Flags]
public enum EquipFlag : uint
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
    public static implicit operator EquipFlagDef(EquipFlag flag) 
    {
        bool HasFlag(EquipFlag chara) 
        {
            return (chara & flag) == chara;
        }
        return new EquipFlagDef() 
        { 
            Player = HasFlag(EquipFlag.Player),
            Yukari = HasFlag(EquipFlag.Yukari),
            Stupei = HasFlag(EquipFlag.Stupei),
            Akihiko = HasFlag(EquipFlag.Akihiko),
            Mitsuru = HasFlag(EquipFlag.Mitsuru),
            Fuuka = HasFlag(EquipFlag.Fuuka),
            Ken = HasFlag(EquipFlag.Ken),
            Koromaru = HasFlag(EquipFlag.Koromaru),
            Shinjiro = HasFlag(EquipFlag.Ken),
            Metis = HasFlag(EquipFlag.Metis),
        };
    }
    public static implicit operator EquipFlag?(EquipFlagDef flagDef)
    {
        if (!flagDef.isValid) return null;
        uint bitMask = 0;
        if (flagDef.Player)
            bitMask += (uint)EquipFlag.Player;
        if (flagDef.Yukari)
            bitMask += (uint)EquipFlag.Yukari;
        if (flagDef.Stupei)
            bitMask += (uint)EquipFlag.Stupei;
        if (flagDef.Akihiko)
            bitMask += (uint)EquipFlag.Akihiko;
        if (flagDef.Mitsuru)
            bitMask += (uint)EquipFlag.Mitsuru;
        if (flagDef.Fuuka)
            bitMask += (uint)EquipFlag.Fuuka;
        if (flagDef.Ken)
            bitMask += (uint)EquipFlag.Ken;
        if (flagDef.Koromaru) 
            bitMask += (uint)EquipFlag.Koromaru;
        if (flagDef.Shinjiro)
            bitMask += (uint)EquipFlag.Shinjiro;
        if (flagDef.Metis)
            bitMask += (uint)EquipFlag.Metis;
        return (EquipFlag)bitMask;
    }
}