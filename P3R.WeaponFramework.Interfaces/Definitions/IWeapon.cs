namespace P3R.WeaponFramework.Interfaces.Types;

public interface IWeapon : IEquatable<WeaponItem>
{
    Character Character { get; set; }
    WeaponConfig Config { get; set; }
    string Description { get; set; }
    bool IsVanilla { get; set; }
    bool IsEnabled { get; set; }
    int ModelId { get; set; }
    string Name { get; set; }
    string? OwnerModId { get; set; }
    WeaponStats Stats { get; set; }
    int WeaponId { get; set; }
    int WeaponItemId { get; set; }
    WeaponModelSet WeaponModelId { get; set; }
    EquipFlag WeaponType { get; set; }
    void SetWeaponItemId(int weaponItemId);
}