using Unreal.AtlusScript.Interfaces;

namespace P3R.WeaponFramework.Types;

[Flags]
public enum EpisodeFlag
{
    NONE = 1 << 0,
    VANILLA = 1 << 1,
    ASTREA = 1 << 2,
    BOTH = VANILLA | ASTREA
}

public static class EpisodeFlags
{
    public static AssetMode AtlusMode(this EpisodeFlag flag)
        => flag switch
        {
            EpisodeFlag.VANILLA => AssetMode.Default,
            EpisodeFlag.ASTREA => AssetMode.Astrea,
            EpisodeFlag.BOTH => AssetMode.Both,
            _ => throw new NotImplementedException()
        };
    public static string WeaponResource(this EpisodeFlag flag)
    {
        return flag.GetResources().Weapons;
    }
    public static string DescriptionResource(this EpisodeFlag flag)
    {
        return flag.GetResources().Descriptions;
    }
    private static Resources GetResources(this EpisodeFlag flag)
        => flag switch
        {
            EpisodeFlag.VANILLA => VANILLA,
            EpisodeFlag.ASTREA => ASTREA,
            _ => throw new NotImplementedException()
        };
    public static Resources VANILLA { get; } = new("P3R.WeaponFramework.Resources.Weapons.json", "P3R.WeaponFramework.Resources.EN.Descriptions.json");
    public static Resources ASTREA  { get; } = new("P3R.WeaponFramework.Resources.Weapons_Astrea.json", "P3R.WeaponFramework.Resources.EN.Descriptions_Astrea.json");
    public struct Resources(string weapons, string descriptions)
    {
        public string Weapons = weapons;
        public string Descriptions = descriptions;
    }
}