namespace P3R.WeaponFramework.Interfaces.Types;

public interface IWeapon : IEquatable<WeaponItem>
{
    #region Status
    bool IsVanilla { get; set; }
    bool IsEnabled { get; set; }
    #endregion
    #region Weapon Table Entry
    Character Character { get; set; }
    int WeaponId { get; set; }
    string? Name { get; set; }
    int ModelId { get; set; }
    EWeaponModelSet WeaponModelId { get; set; }
    WeaponConfig Config { get; set; }
    WeaponStats Stats { get; set; }
    #endregion
    EquipFlag WeaponType { get; set; }
    string? OwnerModId { get; set; }
    string Description { get; set; }
    int WeaponItemId { get; set; }

    EArmatureType? TargetArmature { get; }

    void SetWeaponItemId(int weaponItemId);
}