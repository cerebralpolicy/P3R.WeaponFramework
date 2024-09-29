namespace P3R.WeaponFramework.Hooks;

public record ShellWeapon(ECharacter Character, WeaponModelSet WeaponModelSet)
{

    public string MeshPath { get; } = GetAssetFile(Character, WeaponModelSet, WeaponAssetType.Weapon_Mesh)!;
}