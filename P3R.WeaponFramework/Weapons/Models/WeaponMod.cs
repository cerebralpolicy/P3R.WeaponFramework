namespace P3R.WeaponFramework.Weapons.Models;

public record WeaponMod(string ModId, string ModDir)
{
    public string ContentDir { get; } = Path.Join(ModDir, "UnrealEssentials", "P3R", "Content");
    public string WeaponsDir { get; } = Path.Join(ModDir, "UnrealEssentials", "P3R", "Content", "Weapons");
};