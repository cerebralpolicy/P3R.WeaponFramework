namespace P3R.WeaponFramework.Hooks;

public record ShellWeapon(ECharacter Character, EWeaponModelSet WeaponModelSet)
{

    public string MeshPath { get; } = GetAssetFile(Character, WeaponModelSet, WeaponAssetType.Weapon_Mesh)!;
}