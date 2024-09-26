namespace P3R.WeaponFramework.Hooks;

public record ShellWeapon(Character Character, WeaponModelSet WeaponModelSet)
{

    public string MeshPath { get; } = GetAssetFile(Character, WeaponModelSet, WeaponAssetType.Weapon_Mesh)!;
}