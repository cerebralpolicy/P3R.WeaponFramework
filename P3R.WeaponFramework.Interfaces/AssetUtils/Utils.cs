﻿using P3R.WeaponFramework.Interfaces.Types;

namespace P3R.WeaponFramework.Interfaces;

public static class Constants
{
    public const string CostumeItemsData = "DatItemCostumeDataAsset";
    public const string CostumeNamesData = "DatItemCostumeNameDataAsset";
    public const string WeaponItemsData = "DatItemWeaponDataAsset";
    public const string WeaponNamesData = "DatItemWeaponNameDataAsset";
}

public static partial class IAssetUtils
{
    public static string? GetAssetFile(ECharacter chara, EWeaponModelSet model, WeaponAssetType type)
    {
        string? assetFile = type switch
        {
            WeaponAssetType.Base_Mesh => GetAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/SKEL_Wp{chara.Format()}"),
            WeaponAssetType.Weapon_Mesh => GetAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/Models/SK_Wp{chara.Format()}_{model.Format()}"),

            WeaponAssetType.Base_Anim => GetAssetPath($"/Game/Xrd777/Characters/Weapon/Wp{chara.Format()}/SKEL_Wp{chara.Format()}"),
            WeaponAssetType.Weapon_Anim => null,
            _ => throw new Exception(),
        };
        return assetFile;
    }
    public static string GetUnrealAssetPath(string assetFile)
    {
        var assetPath = GetAssetPath(assetFile);
        return $"{assetPath}.{Path.GetFileName(assetPath)}";
    }
    public static string Format(this ECharacter character) => ((int)character).ToString("0000");
    public static string Format(this EWeaponModelSet weaponModelSet) => ((int)weaponModelSet).ToString("000");
    public static string FormatAssetPath(string assetPath)
    {
        var formattedPath = assetPath.Replace("\\", "/").Replace(".uasset", string.Empty);
        if (!formattedPath.StartsWith("/Game/"))
        {
            formattedPath = $"/Game/{formattedPath}";
        }
        return formattedPath;
    }
    public static string GetAssetPath(string assetFile)
    {
        var adjustedPath = assetFile.Replace('\\', '/').Replace(".uasset", string.Empty);

        if (adjustedPath.IndexOf("Content") is int contentIndex && contentIndex > -1)
        {
            adjustedPath = adjustedPath.Substring(contentIndex + 8);
        }

        if (!adjustedPath.StartsWith("/Game/"))
        {
            adjustedPath = $"/Game/{adjustedPath}";
        }
        return adjustedPath;
    }
    public static Dictionary<uint, uint> ModelPairsUInt = new Dictionary<uint, uint>()
    {
        { 0 , 0 }, // So Fuuka doesn't fail
        { 10, 0 },
        { 11, 1 },
        { 12, 2 },
        { 13, 3 },
        { 14, 4 },
        { 15, 5 },
        { 16, 6 },
        { 17, 7 },
        { 18, 8 },
        { 19, 9 },
        { 20, 0 },
        { 21, 1 },
        { 22, 2 },
        { 23, 3 },
        { 24, 4 },
        { 25, 5 },
        { 26, 6 },
        { 27, 7 },
        { 28, 8 },
        { 30, 0 },
        { 31, 1 },
        { 32, 2 },
        { 33, 3 },
        { 34, 4 },
        { 35, 5 },
        { 36, 6 },
        { 37, 7 },
        { 38, 8 },
        { 39, 9 },
        { 40, 0 },
        { 41, 1 },
        { 42, 2 },
        { 43, 3 },
        { 44, 4 },
        { 45, 5 },
        { 46, 6 },
        { 47, 7 },
        { 48, 8 },
        { 50, 0 },
        { 51, 1 },
        { 52, 2 },
        { 53, 3 },
        { 54, 4 },
        { 55, 5 },
        { 56, 6 },
        { 57, 7 },
        { 80, 0 },
        { 81, 1 },
        { 82, 2 },
        { 83, 3 },
        { 84, 4 },
        { 85, 5 },
        { 86, 6 },
        { 87, 7 },
        { 88, 8 },
        { 89, 9 },
        { 90, 0 },
        { 91, 1 },
        { 92, 2 },
        { 93, 3 },
        { 94, 4 },
        { 95, 5 },
        { 96, 6 },
        { 97, 7 },
        { 100, 0 },
        { 101, 1 },
        { 102, 2 },
        { 103, 3 },
        { 104, 4 },
        { 105, 5 },
        { 326, 0 },
        { 327, 1 },
        { 584, 2 },
        { 585, 3 },
        { 586, 4 },
        { 587, 5 },
        { 588, 6 },
        { 589, 8 },
    };

    public static Dictionary<int, int> ModelPairsInt = new Dictionary<int, int>()
    {
        { 0, 0 }, // So Fuuka doesn't fail
        { 10, 0 },
        { 11, 1 },
        { 12, 10 },
        { 13, 11 },
        { 14, 20 },
        { 15, 21 },
        { 16, 50 },
        { 17, 51 },
        { 18, 60 },
        { 19, 61 },
        { 20, 0 },
        { 21, 1 },
        { 22, 10 },
        { 23, 11 },
        { 24, 20 },
        { 25, 21 },
        { 26, 50 },
        { 27, 51 },
        { 28, 60 },
        { 30, 0 },
        { 31, 1 },
        { 32, 10 },
        { 33, 11 },
        { 34, 20 },
        { 35, 21 },
        { 36, 50 },
        { 37, 51 },
        { 38, 60 },
        { 39, 61 },
        { 40, 0 },
        { 41, 1 },
        { 42, 10 },
        { 43, 11 },
        { 44, 20 },
        { 45, 21 },
        { 46, 50 },
        { 47, 51 },
        { 48, 60 },
        { 50, 0 },
        { 51, 1 },
        { 52, 10 },
        { 53, 11 },
        { 54, 20 },
        { 55, 21 },
        { 56, 50 },
        { 57, 51 },
        { 70, 0 },
        { 71, 1 },
        { 72, 10 },
        { 73, 11 },
        { 74, 20 },
        { 75, 21 },
        { 76, 50 },
        { 77, 51 },
        { 78, 60 },
        { 79, 61 },
        { 80, 0 },
        { 81, 1 },
        { 82, 10 },
        { 83, 11 },
        { 84, 20 },
        { 85, 21 },
        { 86, 50 },
        { 87, 51 },
        { 88, 60 },
        { 89, 61 },
        { 90, 0 },
        { 91, 1 },
        { 92, 10 },
        { 93, 11 },
        { 94, 20 },
        { 95, 21 },
        { 96, 50 },
        { 97, 51 },
        { 100, 0 },
        { 101, 1 },
        { 102, 10 },
        { 103, 11 },
        { 104, 20 },
        { 105, 21 },
        { 110, 0 },
        { 111, 1 },
        { 112, 10 },
        { 113, 11 },
        { 114, 20 },
        { 115, 21 },
        { 116, 50 },
        { 117, 51 },
        { 118, 60 },
        { 119, 61 },
        { 326, 0 },
        { 327, 1 },
        { 584, 10 },
        { 585, 11 },
        { 586, 20 },
        { 587, 21 },
        { 588, 50 },
        { 589, 60 },
    };

    public static Dictionary<string, string> NamePairs = new Dictionary<string, string>()
    {
        {"IT_WEA_BLANK", "Unused"},
        {"IT_WEA_SWORD_01_", "Shortsword"},
        {"IT_WEA_SWORD_02_", "Long Wakizashi"},
        {"IT_WEA_SWORD_03_", "Saber"},
        {"IT_WEA_SWORD_04_", "Gladius"},
        {"IT_WEA_SWORD_05_", "Nikkari Aoe"},
        {"IT_WEA_SWORD_06_", "Fudo Masamune"},
        {"IT_WEA_SWORD_07_", "Iron Edge"},
        {"IT_WEA_SWORD_08_", "Unused"},
        {"IT_WEA_SWORD_09_", "Silver Saber"},
        {"IT_WEA_SWORD_10_", "Legendary Cleaver"},
        {"IT_WEA_SWORD_11_", "Steel Pipe"},
        {"IT_WEA_SWORD_12_", "Aroundight"},
        {"IT_WEA_SWORD_13_", "Tizona"},
        {"IT_WEA_SWORD_14_", "Kaneshige"},
        {"IT_WEA_SWORD_15_", "Sin Blade"},
        {"IT_WEA_SWORD_16_", "Shishiou"},
        {"IT_WEA_SWORD_17_", "Omokage"},
        {"IT_WEA_SWORD_18_", "Kogarasumaru"},
        {"IT_WEA_SWORD_19_", "Tajikarao Sword"},
        {"IT_WEA_SWORD_20_", "Qi Xing Dao"},
        {"IT_WEA_SWORD_21_", "Pulsing Blade"},
        {"IT_WEA_SWORD_22_", "Gimlet"},
        {"IT_WEA_SWORD_23_", "Excalibur"},
        {"IT_WEA_SWORD_24_", "Deus Xiphos"},
        {"IT_WEA_SWORD_25_", "Lucifer's Blade"},
        {"IT_WEA_SWORD_26_", "Unused"},
        {"IT_WEA_SWORD_27_", "Nameless Katana"},
        {"IT_WEA_SWORD_28_", "Unused"},
        {"IT_WEA_SWORD_29_", "Holy Knight Sword"},
        {"IT_WEA_SWORD_30_", "Unused"},
        {"IT_WEA_SWORD_31_", "Dual Sword"},
        {"IT_WEA_SWORD_32_", "Yggdrasword"},
        {"IT_WEA_SWORD_33_", "Translucent Blade"},
        {"IT_WEA_SWORD_34_", "Envenomed Blade"},
        {"IT_WEA_SWORD_35_", "Onimaru Kunitsuna"},
        {"IT_WEA_BOW_01___", "Practice Bow"},
        {"IT_WEA_BOW_02___", "Short Bow"},
        {"IT_WEA_BOW_03___", "Siren's Song"},
        {"IT_WEA_BOW_04___", "Shigetou-yumi"},
        {"IT_WEA_BOW_05___", "RainBow"},
        {"IT_WEA_BOW_06___", "Uta"},
        {"IT_WEA_BOW_07___", "Pleiades"},
        {"IT_WEA_BOW_08___", "Bow of Affection"},
        {"IT_WEA_BOW_09___", "Toy Bow"},
        {"IT_WEA_BOW_10___", "Kamatha"},
        {"IT_WEA_BOW_11___", "Poison Arrow Bow"},
        {"IT_WEA_BOW_12___", "Bold Bow"},
        {"IT_WEA_BOW_13___", "Higo-yumi"},
        {"IT_WEA_BOW_14___", "Heavenly Windbow"},
        {"IT_WEA_BOW_15___", "Composite Bow"},
        {"IT_WEA_BOW_16___", "Magic Bow"},
        {"IT_WEA_BOW_17___", "Great Bow"},
        {"IT_WEA_BOW_18___", "Lightning Bow"},
        {"IT_WEA_BOW_19___", "Hero's Bow"},
        {"IT_WEA_BOW_20___", "Bow of Serenity"},
        {"IT_WEA_BOW_21___", "Yoichi's Bow"},
        {"IT_WEA_BOW_22___", "Dhanush"},
        {"IT_WEA_BOW_23___", "Maki's Resolve"},
        {"IT_WEA_BOW_24___", "Sarnga"},
        {"IT_WEA_BOW_25___", "Quintessence Bow"},
        {"IT_WEA_BOW_26___", "Source Yumi"},
        {"IT_WEA_BOW_27___", "Unused"},
        {"IT_WEA_BOW_28___", "Unused"},
        {"IT_WEA_BOW_29___", "Dreadnought"},
        {"IT_WEA_BOW_30___", "Kishin Bow"},
        {"IT_WEA_BOW_31___", "Circe's Bow"},
        {"IT_WEA_BOW_32___", "Whirlwind Bow"},
        {"IT_WEA_BOW_33___", "Gale Bow"},
        {"IT_WEA_BOW_34___", "Unused"},
        {"IT_WEA_BOW_35___", "Calamity Bow"},
        {"IT_WEA_LSOWRD_01", "Imitation Katana"},
        {"IT_WEA_LSOWRD_02", "Bastard Sword"},
        {"IT_WEA_LSOWRD_03", "Kishido Blade"},
        {"IT_WEA_LSOWRD_04", "Great Sword"},
        {"IT_WEA_LSOWRD_05", "Raikou"},
        {"IT_WEA_LSOWRD_06", "Two-Handed Sword"},
        {"IT_WEA_LSOWRD_07", "All-Purpose Katana"},
        {"IT_WEA_LSOWRD_08", "Unused"},
        {"IT_WEA_LSOWRD_09", "Spiked Bat"},
        {"IT_WEA_LSOWRD_10", "Zanbatou"},
        {"IT_WEA_LSOWRD_11", "Assassin's Blade"},
        {"IT_WEA_LSOWRD_12", "Tsubaki-maru"},
        {"IT_WEA_LSOWRD_13", "Unused"},
        {"IT_WEA_LSOWRD_14", "Buster Blade"},
        {"IT_WEA_LSOWRD_15", "Mikazuki Munechika"},
        {"IT_WEA_LSOWRD_16", "Kaketsushinto"},
        {"IT_WEA_LSOWRD_17", "Deathbringer"},
        {"IT_WEA_LSOWRD_18", "Caladbolg"},
        {"IT_WEA_LSOWRD_19", "Apocalypse"},
        {"IT_WEA_LSOWRD_20", "Tobi-botaru"},
        {"IT_WEA_LSOWRD_21", "Elixir Sword"},
        {"IT_WEA_LSOWRD_22", "Orochito"},
        {"IT_WEA_LSOWRD_23", "Balmung"},
        {"IT_WEA_LSOWRD_24", "Laevateinn"},
        {"IT_WEA_LSOWRD_25", "Masakado's Katana"},
        {"IT_WEA_LSOWRD_26", "Unused"},
        {"IT_WEA_LSOWRD_27", "Claymore"},
        {"IT_WEA_LSOWRD_28", "Unused"},
        {"IT_WEA_LSOWRD_29", "Juzumaru"},
        {"IT_WEA_LSOWRD_30", "Dojigiri Yasutsuna"},
        {"IT_WEA_LSOWRD_31", "Berserker"},
        {"IT_WEA_LSOWRD_32", "Crimson Greatsword"},
        {"IT_WEA_LSOWRD_33", "Blazing Greatsword"},
        {"IT_WEA_LSOWRD_34", "Qingdi Blade"},
        {"IT_WEA_LSOWRD_35", "Unused"},
        {"IT_WEA_KNUCKL_01", "Brass Gloves"},
        {"IT_WEA_KNUCKL_02", "Bladefist"},
        {"IT_WEA_KNUCKL_03", "Sonic Fist"},
        {"IT_WEA_KNUCKL_04", "Kaiser Knuckles"},
        {"IT_WEA_KNUCKL_05", "Beast Fangs"},
        {"IT_WEA_KNUCKL_06", "Champion Gloves"},
        {"IT_WEA_KNUCKL_07", "Jack's Gloves"},
        {"IT_WEA_KNUCKL_08", "Meteor Knuckles"},
        {"IT_WEA_KNUCKL_09", "Gusto Gloves"},
        {"IT_WEA_KNUCKL_10", "Fists of Fury"},
        {"IT_WEA_KNUCKL_11", "Wicked Cestus"},
        {"IT_WEA_KNUCKL_12", "Blood Baghnakh"},
        {"IT_WEA_KNUCKL_13", "Titanic Knuckles"},
        {"IT_WEA_KNUCKL_14", "Pugilist's Fists"},
        {"IT_WEA_KNUCKL_15", "Crusher Fist"},
        {"IT_WEA_KNUCKL_16", "Wings of Vanth"},
        {"IT_WEA_KNUCKL_17", "Supreme Gloves"},
        {"IT_WEA_KNUCKL_18", "Diamond Knuckles"},
        {"IT_WEA_KNUCKL_19", "Golden Gloves"},
        {"IT_WEA_KNUCKL_20", "Dragon Fangs"},
        {"IT_WEA_KNUCKL_21", "Double Ziggurat"},
        {"IT_WEA_KNUCKL_22", "Sabazios"},
        {"IT_WEA_KNUCKL_23", "Evil Gloves"},
        {"IT_WEA_KNUCKL_24", "Root Cestus"},
        {"IT_WEA_KNUCKL_25", "Unused"},
        {"IT_WEA_KNUCKL_26", "Unused"},
        {"IT_WEA_KNUCKL_27", "Heaven's Fists"},
        {"IT_WEA_KNUCKL_28", "Spirit Gloves"},
        {"IT_WEA_KNUCKL_29", "Rapid Bands"},
        {"IT_WEA_KNUCKL_30", "Thunder Knuckles"},
        {"IT_WEA_KNUCKL_31", "Jack's Gauntlets"},
        {"IT_WEA_KNUCKL_32", "Unused"},
        {"IT_WEA_KNUCKL_33", "Unused"},
        {"IT_WEA_KNUCKL_34", "Unused"},
        {"IT_WEA_KNUCKL_35", "Unused"},
        {"IT_WEA_RAPIER_01", "SEES Rapier"},
        {"IT_WEA_RAPIER_02", "Quarter Pike"},
        {"IT_WEA_RAPIER_03", "Flamberge"},
        {"IT_WEA_RAPIER_04", "Unused"},
        {"IT_WEA_RAPIER_05", "Night Falcon"},
        {"IT_WEA_RAPIER_06", "Elegant Fleuret"},
        {"IT_WEA_RAPIER_07", "Ga Boo"},
        {"IT_WEA_RAPIER_08", "Malice Mary"},
        {"IT_WEA_RAPIER_09", "Serpent Sword"},
        {"IT_WEA_RAPIER_10", "Skrep"},
        {"IT_WEA_RAPIER_11", "Ithuriel Spear"},
        {"IT_WEA_RAPIER_12", "Espada Ropera"},
        {"IT_WEA_RAPIER_13", "Noble Saber"},
        {"IT_WEA_RAPIER_14", "Main Gauche"},
        {"IT_WEA_RAPIER_15", "Witch Saber"},
        {"IT_WEA_RAPIER_16", "Rose Flamberge"},
        {"IT_WEA_RAPIER_17", "Damascus Rapier"},
        {"IT_WEA_RAPIER_18", "Brionac"},
        {"IT_WEA_RAPIER_19", "Longinus"},
        {"IT_WEA_RAPIER_20", "Kokuseki Senjin"},
        {"IT_WEA_RAPIER_21", "Snow Queen Whip"},
        {"IT_WEA_RAPIER_22", "Cocytus"},
        {"IT_WEA_RAPIER_23", "Unused"},
        {"IT_WEA_RAPIER_24", "Unused"},
        {"IT_WEA_RAPIER_25", "Unused"},
        {"IT_WEA_RAPIER_26", "Unused"},
        {"IT_WEA_RAPIER_27", "Unused"},
        {"IT_WEA_RAPIER_28", "Ice Saber"},
        {"IT_WEA_RAPIER_29", "Charlotte"},
        {"IT_WEA_RAPIER_30", "Illuminati"},
        {"IT_WEA_RAPIER_31", "Aristocracy"},
        {"IT_WEA_RAPIER_32", "Unused"},
        {"IT_WEA_RAPIER_33", "Erinys"},
        {"IT_WEA_RAPIER_34", "Unused"},
        {"IT_WEA_RAPIER_35", "Unused"},
        {"IT_WEA_ARMAS_01_", "Albireo"},
        {"IT_WEA_ARMAS_02_", "Six-Shot"},
        {"IT_WEA_ARMAS_03_", "Grenade Launcher"},
        {"IT_WEA_ARMAS_04_", "Orgone Rifle"},
        {"IT_WEA_ARMAS_05_", "Heavy Cannon X"},
        {"IT_WEA_ARMAS_06_", "Ingels Cannon"},
        {"IT_WEA_ARMAS_07_", "Five-Barrel Medusa"},
        {"IT_WEA_ARMAS_08_", "Railgun"},
        {"IT_WEA_ARMAS_09_", "Supersonic Minigun"},
        {"IT_WEA_ARMAS_10_", "Rocket Punch"},
        {"IT_WEA_ARMAS_11_", "Maxima Sniper"},
        {"IT_WEA_ARMAS_12_", "Angel Shot"},
        {"IT_WEA_ARMAS_13_", "Infanterie"},
        {"IT_WEA_ARMAS_14_", "Megido Fire"},
        {"IT_WEA_ARMAS_15_", "Antimatter Cannon"},
        {"IT_WEA_ARMAS_16_", "Metatronius"},
        {"IT_WEA_ARMAS_17_", "Flash Grenade"},
        {"IT_WEA_ARMAS_18_", "Kiss of Athena"},
        {"IT_WEA_ARMAS_19_", "Frigid Grenade"},
        {"IT_WEA_ARMAS_20_", "Blast Magnum"},
        {"IT_WEA_ARMAS_21_", "Pandemonium"},
        {"IT_WEA_ARMAS_22_", "Nucleus Rifle"},
        {"IT_WEA_ARMAS_23_", "Unused"},
        {"IT_WEA_ARMAS_24_", "Unused"},
        {"IT_WEA_ARMAS_25_", "Kyriotes"},
        {"IT_WEA_DAGGER_01", "SEES Knife"},
        {"IT_WEA_DAGGER_02", "Bone"},
        {"IT_WEA_DAGGER_03", "Blitz Kunai"},
        {"IT_WEA_DAGGER_04", "Lucky Knife"},
        {"IT_WEA_DAGGER_05", "Silver Moon"},
        {"IT_WEA_DAGGER_06", "Karasu-maru"},
        {"IT_WEA_DAGGER_07", "Sword Breaker"},
        {"IT_WEA_DAGGER_08", "Raven Claw"},
        {"IT_WEA_DAGGER_09", "Shadowrend"},
        {"IT_WEA_DAGGER_10", "Pesh Kabz"},
        {"IT_WEA_DAGGER_11", "Howl"},
        {"IT_WEA_DAGGER_12", "Athame"},
        {"IT_WEA_DAGGER_13", "Full Moon Kunai"},
        {"IT_WEA_DAGGER_14", "Grand Slasher"},
        {"IT_WEA_DAGGER_15", "Vajra"},
        {"IT_WEA_DAGGER_16", "Elementary Mask"},
        {"IT_WEA_DAGGER_17", "Unused"},
        {"IT_WEA_DAGGER_18", "Unused"},
        {"IT_WEA_DAGGER_19", "Carnage Knife"},
        {"IT_WEA_DAGGER_20", "Dagger of Protection"},
        {"IT_WEA_DAGGER_21", "Rai Kunimitsu"},
        {"IT_WEA_DAGGER_22", "Underworld Kunai"},
        {"IT_WEA_DAGGER_23", "Hazakura"},
        {"IT_WEA_DAGGER_24", "Paring Knife"},
        {"IT_WEA_DAGGER_25", "Tyrant's Knife"},
        {"IT_WEA_SPEAR_01_", "SEES Longspear"},
        {"IT_WEA_SPEAR_02_", "Omega Spear"},
        {"IT_WEA_SPEAR_03_", "Glaive"},
        {"IT_WEA_SPEAR_04_", "Sexy Lance"},
        {"IT_WEA_SPEAR_05_", "Rhongowennan"},
        {"IT_WEA_SPEAR_06_", "Lance of Death"},
        {"IT_WEA_SPEAR_07_", "Scrub Brush"},
        {"IT_WEA_SPEAR_08_", "Ranseur"},
        {"IT_WEA_SPEAR_09_", "Ningen Mukotsu"},
        {"IT_WEA_SPEAR_10_", "Ote-gine"},
        {"IT_WEA_SPEAR_11_", "Voulge"},
        {"IT_WEA_SPEAR_12_", "Poison Glaive"},
        {"IT_WEA_SPEAR_13_", "Nihon-gou"},
        {"IT_WEA_SPEAR_14_", "Romulus's Spear"},
        {"IT_WEA_SPEAR_15_", "Tonbo-kiri"},
        {"IT_WEA_SPEAR_16_", "Gae Bolg"},
        {"IT_WEA_SPEAR_17_", "Gungnir"},
        {"IT_WEA_SPEAR_18_", "Pinaka"},
        {"IT_WEA_SPEAR_19_", "Unused"},
        {"IT_WEA_SPEAR_20_", "Unused"},
        {"IT_WEA_SPEAR_21_", "Unused"},
        {"IT_WEA_SPEAR_22_", "King Spear"},
        {"IT_WEA_SPEAR_23_", "Bolt Lance"},
        {"IT_WEA_SPEAR_24_", "Warlock Lance"},
        {"IT_WEA_SPEAR_25_", "Blessed Lance"},
        {"IT_WEA_BLUNT_01_", "SEES Battle-Axe"},
        {"IT_WEA_BLUNT_02_", "Ogre Hammer"},
        {"IT_WEA_BLUNT_03_", "Night Stalker"},
        {"IT_WEA_BLUNT_04_", "Bus Stop Sign"},
        {"IT_WEA_BLUNT_05_", "Guillotine Axe"},
        {"IT_WEA_BLUNT_06_", "Megaton Rod"},
        {"IT_WEA_BLUNT_07_", "Unused"},
        {"IT_WEA_BLUNT_08_", "Unused"},
        {"IT_WEA_BLUNT_09_", "Unused"},
        {"IT_WEA_BLUNT_10_", "Charun's Hammer"},
        {"IT_WEA_BLUNT_11_", "Gaea's Grace"},
        {"IT_WEA_BLUNT_12_", "Unused"},
        {"IT_WEA_BLUNT_13_", "Celtis"},
        {"IT_WEA_BLUNT_14_", "Golden Crusher"},
        {"IT_WEA_BLUNT_15_", "Mjolnir"},
        {"IT_WEA_BLUNT_16_", "Corpse Rod"},
        {"IT_WEA_BLUNT_17_", "Unused"},
        {"IT_WEA_BLUNT_18_", "Unused"},
        {"IT_WEA_BLUNT_19_", "Unused"},
        {"IT_WEA_BLUNT_20_", "Unused"},
        {"IT_WEA_SPEAR_26_", "Fauchard"},
        {"IT_WEA_SPEAR_27_", "Unused"},
        {"IT_WEA_SPEAR_28_", "Unused"},
        {"IT_WEA_SPEAR_29_", "Unused"},
        {"IT_WEA_SPEAR_30_", "Unused"},
        {"IT_WEA_FUKA_01__", "No equipment"},
        {"IT_WEA_ID_0x115_", "Unused"},
        {"IT_WEA_ID_0x116_", "Unused"},
        {"IT_WEA_ID_0x117_", "Unused"},
        {"IT_WEA_NEW_W_HER", "SEES Sword"},
        {"IT_WEA_NEW_W_YUK", "SEES Longbow"},
        {"IT_WEA_NEW_W_JUN", "SEES Greatsword"},
        {"IT_WEA_NEW_W_SAN", "SEES Knuckles"},
        {"IT_WEA_NEW_W_MIT", "Unused"},
        {"IT_WEA_NEW_W_FUK", "Unused"},
        {"IT_WEA_NEW_W_AEG", "Unused"},
        {"IT_WEA_NEW_W_AMA", "Unused"},
        {"IT_WEA_NEW_W_KOR", "Unused"},
        {"IT_WEA_NEW_W_ARA", "Unused"},
        {"IT_WEA_SWORD_36_", "Myohou Muramasa"},
        {"IT_WEA_SWORD_37_", "Myoho Masamura"},
        {"IT_WEA_SWORD_38_", "Outenta Mitsuyo"},
        {"IT_WEA_SWORD_39_", "Fatal Blade"},
        {"IT_WEA_SWORD_40_", "Amakuni"},
        {"IT_WEA_ID_0x127_", "Unused"},
        {"IT_WEA_ID_0x128_", "Unused"},
        {"IT_WEA_ID_0x129_", "Unused"},
        {"IT_WEA_ID_0x12A_", "Unused"},
        {"IT_WEA_ID_0x12B_", "Unused"},
        {"IT_WEA_ID_0x12C_", "Unused"},
        {"IT_WEA_ID_0x12D_", "Unused"},
        {"IT_WEA_ID_0x12E_", "Unused"},
        {"IT_WEA_ID_0x12F_", "Unused"},
        {"IT_WEA_ID_0x130_", "Unused"},
        {"IT_WEA_ID_0x131_", "Unused"},
        {"IT_WEA_ID_0x132_", "Unused"},
        {"IT_WEA_ID_0x133_", "Unused"},
        {"IT_WEA_ID_0x134_", "Unused"},
        {"IT_WEA_ID_0x135_", "Unused"},
        {"IT_WEA_ID_0x136_", "Unused"},
        {"IT_WEA_ID_0x137_", "Unused"},
        {"IT_WEA_ID_0x138_", "Unused"},
        {"IT_WEA_ID_0x139_", "Unused"},
        {"IT_WEA_ID_0x13A_", "Unused"},
        {"IT_WEA_ID_0x13B_", "Unused"},
        {"IT_WEA_ID_0x13C_", "Unused"},
        {"IT_WEA_ID_0x13D_", "Unused"},
        {"IT_WEA_ID_0x13E_", "Unused"},
        {"IT_WEA_ID_0x13F_", "Unused"},
        {"IT_WEA_ID_0x140_", "Unused"},
        {"IT_WEA_ID_0x141_", "Unused"},
        {"IT_WEA_ID_0x142_", "Unused"},
        {"IT_WEA_ID_0x143_", "Unused"},
        {"IT_WEA_ID_0x144_", "Unused"},
        {"IT_WEA_ID_0x145_", "Unused"},
        {"IT_WEA_ID_0x146_", "Unused"},
        {"IT_WEA_ID_0x147_", "Unused"},
        {"IT_WEA_ID_0x148_", "Unused"},
        {"IT_WEA_ID_0x149_", "Unused"},
        {"IT_WEA_ID_0x14A_", "Unused"},
        {"IT_WEA_ID_0x14B_", "Unused"},
        {"IT_WEA_ID_0x14C_", "Unused"},
        {"IT_WEA_ID_0x14D_", "Unused"},
        {"IT_WEA_ID_0x14E_", "Unused"},
        {"IT_WEA_ID_0x14F_", "Unused"},
        {"IT_WEA_ID_0x150_", "Unused"},
        {"IT_WEA_ID_0x151_", "Unused"},
        {"IT_WEA_ID_0x152_", "Unused"},
        {"IT_WEA_ID_0x153_", "Unused"},
        {"IT_WEA_ID_0x154_", "Unused"},
        {"IT_WEA_ID_0x155_", "Unused"},
        {"IT_WEA_ID_0x156_", "Unused"},
        {"IT_WEA_ID_0x157_", "Unused"},
        {"IT_WEA_ID_0x158_", "Unused"},
        {"IT_WEA_ID_0x159_", "Unused"},
        {"IT_WEA_ID_0x15A_", "Unused"},
        {"IT_WEA_ID_0x15B_", "Unused"},
        {"IT_WEA_ID_0x15C_", "Unused"},
        {"IT_WEA_ID_0x15D_", "Unused"},
        {"IT_WEA_ID_0x15E_", "Unused"},
        {"IT_WEA_ID_0x15F_", "Unused"},
        {"IT_WEA_ID_0x160_", "Unused"},
        {"IT_WEA_ID_0x161_", "Unused"},
        {"IT_WEA_ID_0x162_", "Unused"},
        {"IT_WEA_ID_0x163_", "Unused"},
        {"IT_WEA_ID_0x164_", "Unused"},
        {"IT_WEA_ID_0x165_", "Unused"},
        {"IT_WEA_ID_0x166_", "Unused"},
        {"IT_WEA_ID_0x167_", "Unused"},
        {"IT_WEA_ID_0x168_", "Unused"},
        {"IT_WEA_ID_0x169_", "Unused"},
        {"IT_WEA_ID_0x16A_", "Unused"},
        {"IT_WEA_ID_0x16B_", "Unused"},
        {"IT_WEA_ID_0x16C_", "Unused"},
        {"IT_WEA_ID_0x16D_", "Unused"},
        {"IT_WEA_ID_0x16E_", "Unused"},
        {"IT_WEA_ID_0x16F_", "Unused"},
        {"IT_WEA_ID_0x170_", "Unused"},
        {"IT_WEA_ID_0x171_", "Unused"},
        {"IT_WEA_ID_0x172_", "Unused"},
        {"IT_WEA_ID_0x173_", "Unused"},
        {"IT_WEA_ID_0x174_", "Unused"},
        {"IT_WEA_ID_0x175_", "Unused"},
        {"IT_WEA_ID_0x176_", "Unused"},
        {"IT_WEA_ID_0x177_", "Unused"},
        {"IT_WEA_ID_0x178_", "Unused"},
        {"IT_WEA_ID_0x179_", "Unused"},
        {"IT_WEA_ID_0x17A_", "Unused"},
        {"IT_WEA_ID_0x17B_", "Unused"},
        {"IT_WEA_ID_0x17C_", "Unused"},
        {"IT_WEA_ID_0x17D_", "Unused"},
        {"IT_WEA_ID_0x17E_", "Unused"},
        {"IT_WEA_ID_0x17F_", "Unused"},
        {"IT_WEA_ID_0x180_", "Unused"},
        {"IT_WEA_ID_0x181_", "Unused"},
        {"IT_WEA_ID_0x182_", "Unused"},
        {"IT_WEA_ID_0x183_", "Unused"},
        {"IT_WEA_ID_0x184_", "Unused"},
        {"IT_WEA_ID_0x185_", "Unused"},
        {"IT_WEA_ID_0x186_", "Unused"},
        {"IT_WEA_ID_0x187_", "Unused"},
        {"IT_WEA_ID_0x188_", "Unused"},
        {"IT_WEA_ID_0x189_", "Unused"},
        {"IT_WEA_ID_0x18A_", "Unused"},
        {"IT_WEA_ID_0x18B_", "Unused"},
        {"IT_WEA_ID_0x18C_", "Unused"},
        {"IT_WEA_ID_0x18D_", "Unused"},
        {"IT_WEA_ID_0x18E_", "Unused"},
        {"IT_WEA_ID_0x18F_", "Unused"},
        {"IT_WEA_ID_0x190_", "Unused"},
        {"IT_WEA_ID_0x191_", "Unused"},
        {"IT_WEA_ID_0x192_", "Unused"},
        {"IT_WEA_ID_0x193_", "Unused"},
        {"IT_WEA_ID_0x194_", "Unused"},
        {"IT_WEA_ID_0x195_", "Unused"},
        {"IT_WEA_ID_0x196_", "Unused"},
        {"IT_WEA_ID_0x197_", "Unused"},
        {"IT_WEA_ID_0x198_", "Unused"},
        {"IT_WEA_ID_0x199_", "Unused"},
        {"IT_WEA_ID_0x19A_", "Unused"},
        {"IT_WEA_ID_0x19B_", "Unused"},
        {"IT_WEA_ID_0x19C_", "Unused"},
        {"IT_WEA_ID_0x19D_", "Unused"},
        {"IT_WEA_ID_0x19E_", "Unused"},
        {"IT_WEA_ID_0x19F_", "Unused"},
        {"IT_WEA_ID_0x1A0_", "Unused"},
        {"IT_WEA_ID_0x1A1_", "Unused"},
        {"IT_WEA_ID_0x1A2_", "Unused"},
        {"IT_WEA_ID_0x1A3_", "Unused"},
        {"IT_WEA_ID_0x1A4_", "Unused"},
        {"IT_WEA_ID_0x1A5_", "Unused"},
        {"IT_WEA_ID_0x1A6_", "Unused"},
        {"IT_WEA_ID_0x1A7_", "Unused"},
        {"IT_WEA_ID_0x1A8_", "Unused"},
        {"IT_WEA_ID_0x1A9_", "Unused"},
        {"IT_WEA_ID_0x1AA_", "Unused"},
        {"IT_WEA_ID_0x1AB_", "Unused"},
        {"IT_WEA_ID_0x1AC_", "Unused"},
        {"IT_WEA_ID_0x1AD_", "Unused"},
        {"IT_WEA_ID_0x1AE_", "Unused"},
        {"IT_WEA_ID_0x1AF_", "Unused"},
        {"IT_WEA_ID_0x1B0_", "Unused"},
        {"IT_WEA_ID_0x1B1_", "Unused"},
        {"IT_WEA_ID_0x1B2_", "Unused"},
        {"IT_WEA_ID_0x1B3_", "Unused"},
        {"IT_WEA_ID_0x1B4_", "Unused"},
        {"IT_WEA_ID_0x1B5_", "Unused"},
        {"IT_WEA_ID_0x1B6_", "Unused"},
        {"IT_WEA_ID_0x1B7_", "Unused"},
        {"IT_WEA_ID_0x1B8_", "Unused"},
        {"IT_WEA_ID_0x1B9_", "Unused"},
        {"IT_WEA_ID_0x1BA_", "Unused"},
        {"IT_WEA_ID_0x1BB_", "Unused"},
        {"IT_WEA_ID_0x1BC_", "Unused"},
        {"IT_WEA_ID_0x1BD_", "Unused"},
        {"IT_WEA_ID_0x1BE_", "Unused"},
        {"IT_WEA_ID_0x1BF_", "Unused"},
        {"IT_WEA_ID_0x1C0_", "Unused"},
        {"IT_WEA_ID_0x1C1_", "Unused"},
        {"IT_WEA_ID_0x1C2_", "Unused"},
        {"IT_WEA_ID_0x1C3_", "Unused"},
        {"IT_WEA_ID_0x1C4_", "Unused"},
        {"IT_WEA_ID_0x1C5_", "Unused"},
        {"IT_WEA_ID_0x1C6_", "Unused"},
        {"IT_WEA_ID_0x1C7_", "Unused"},
        {"IT_WEA_ID_0x1C8_", "Unused"},
        {"IT_WEA_ID_0x1C9_", "Unused"},
        {"IT_WEA_ID_0x1CA_", "Unused"},
        {"IT_WEA_ID_0x1CB_", "Unused"},
        {"IT_WEA_ID_0x1CC_", "Unused"},
        {"IT_WEA_ID_0x1CD_", "Unused"},
        {"IT_WEA_ID_0x1CE_", "Unused"},
        {"IT_WEA_ID_0x1CF_", "Unused"},
        {"IT_WEA_ID_0x1D0_", "Unused"},
        {"IT_WEA_ID_0x1D1_", "Unused"},
        {"IT_WEA_ID_0x1D2_", "Unused"},
        {"IT_WEA_ID_0x1D3_", "Unused"},
        {"IT_WEA_ID_0x1D4_", "Unused"},
        {"IT_WEA_ID_0x1D5_", "Unused"},
        {"IT_WEA_ID_0x1D6_", "Unused"},
        {"IT_WEA_ID_0x1D7_", "Unused"},
        {"IT_WEA_ID_0x1D8_", "Unused"},
        {"IT_WEA_ID_0x1D9_", "Unused"},
        {"IT_WEA_ID_0x1DA_", "Unused"},
        {"IT_WEA_ID_0x1DB_", "Unused"},
        {"IT_WEA_ID_0x1DC_", "Unused"},
        {"IT_WEA_ID_0x1DD_", "Unused"},
        {"IT_WEA_ID_0x1DE_", "Unused"},
        {"IT_WEA_ID_0x1DF_", "Unused"},
        {"IT_WEA_ID_0x1E0_", "Unused"},
        {"IT_WEA_ID_0x1E1_", "Unused"},
        {"IT_WEA_ID_0x1E2_", "Unused"},
        {"IT_WEA_ID_0x1E3_", "Unused"},
        {"IT_WEA_ID_0x1E4_", "Unused"},
        {"IT_WEA_ID_0x1E5_", "Unused"},
        {"IT_WEA_ID_0x1E6_", "Unused"},
        {"IT_WEA_ID_0x1E7_", "Unused"},
        {"IT_WEA_ID_0x1E8_", "Unused"},
        {"IT_WEA_ID_0x1E9_", "Unused"},
        {"IT_WEA_ID_0x1EA_", "Unused"},
        {"IT_WEA_ID_0x1EB_", "Unused"},
        {"IT_WEA_ID_0x1EC_", "Unused"},
        {"IT_WEA_ID_0x1ED_", "Unused"},
        {"IT_WEA_ID_0x1EE_", "Unused"},
        {"IT_WEA_ID_0x1EF_", "Unused"},
        {"IT_WEA_ID_0x1F0_", "Unused"},
        {"IT_WEA_ID_0x1F1_", "Unused"},
        {"IT_WEA_ID_0x1F2_", "Unused"},
        {"IT_WEA_ID_0x1F3_", "Unused"},
        {"IT_WEA_ID_0x1F4_", "Unused"},
        {"IT_WEA_ID_0x1F5_", "Unused"},
        {"IT_WEA_ID_0x1F6_", "Unused"},
        {"IT_WEA_ID_0x1F7_", "Unused"},
        {"IT_WEA_ID_0x1F8_", "Unused"},
        {"IT_WEA_ID_0x1F9_", "Unused"},
        {"IT_WEA_ID_0x1FA_", "Unused"},
        {"IT_WEA_ID_0x1FB_", "Unused"},
        {"IT_WEA_ID_0x1FC_", "Unused"},
        {"IT_WEA_ID_0x1FD_", "Unused"},
        {"IT_WEA_ID_0x1FE_", "Unused"},
        {"IT_WEA_ID_0x1FF_", "Unused"},
    };
}