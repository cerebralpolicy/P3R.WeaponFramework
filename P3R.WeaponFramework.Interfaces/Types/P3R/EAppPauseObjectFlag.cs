namespace P3R.WeaponFramework.Interfaces.Types;

public enum EAppPauseObjectFlag : byte
{
    Unknown = 0,
    CampUI = 1,
    CharacterModel = 2,
    FieldLocal = 4,
    SystemUI = 8,
    FacilityUI = 16,
    FieldSound = 32,
    FieldCrowd = 64,
    Always = 255,
};
