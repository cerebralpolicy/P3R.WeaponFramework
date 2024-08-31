﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Interfaces.Types;

public enum EItemSkillRaw : ushort
{
    NONE = 0,
    HP_10 = 1,
    HP_20 = 2,
    HP_30 = 3,
    HP_40 = 4,
    HP_50 = 5,
    HP_100 = 6,
    SP_10 = 7,
    SP_20 = 8,
    SP_30 = 9,
    SP_40 = 10,
    SP_50 = 11,
    SP_100 = 12,
    Freeze_Low = 13,
    Freeze_Med = 14,
    Freeze_High = 15,
    Shock_Low = 16,
    Shock_Med = 17,
    Shock_High = 18,
    Confuse_Low = 19,
    Confuse_Med = 20,
    Confuse_High = 21,
    Fear_Low = 22,
    Fear_Med = 23,
    Fear_High = 24,
    Distress_Low = 25,
    Distress_Med = 26,
    Distress_High = 27,
    Charm_Low = 28,
    Charm_Med = 29,
    Charm_High = 30,
    Rage_Low = 31,
    Rage_Med = 32,
    Rage_High = 33,
    Poison_Low = 34,
    Poison_Med = 35,
    Poison_High = 36,
    Random_Ailment_Low = 37,
    Random_Ailment_Med = 38,
    Random_Ailment_High = 39,
    Critical_Rate_Up_Low = 40,
    Critical_Rate_Up_Med = 41,
    Critical_Rate_Up_High = 42,
    Slash_Boost = 43,
    Strike_Boost = 44,
    Pierce_Boost = 45,
    Fire_Boost = 46,
    Ice_Boost = 47,
    Elec_Boost = 48,
    Wind_Boost = 49,
    Light_Boost = 50,
    Dark_Boost = 51,
    Counter = 52,
    Counterstrike = 53,
    High_Counter = 54,
    Resist_Freeze = 55,
    Resist_Shock = 56,
    Resist_Confuse = 57,
    Resist_Fear = 58,
    Resist_Distress = 59,
    Resist_Charm = 60,
    Resist_Rage = 61,
    Resist_Poison = 62,
    Resist_Dizzy = 63,
    Null_Freeze = 64,
    Null_Shock = 65,
    Null_Confuse = 66,
    Null_Fear = 67,
    Null_Distress = 68,
    Null_Charm = 69,
    Null_Rage = 70,
    Null_Poison = 71,
    Null_Dizzy = 72,
    Auto_Tarukaja = 73,
    Auto_Rakukaja = 74,
    Auto_Sukukaja = 75,
    Reduce_Slash_Dmg_Low = 76,
    Reduce_Slash_Dmg_Med = 77,
    Reduce_Slash_Dmg_High = 78,
    Reduce_Strike_Dmg_Low = 79,
    Reduce_Strike_Dmg_Med = 80,
    Reduce_Strike_Dmg_High = 81,
    Reduce_Pierce_Dmg_Low = 82,
    Reduce_Pierce_Dmg_Med = 83,
    Reduce_Pierce_Dmg_High = 84,
    Reduce_Phys_Dmg_Low = 85,
    Reduce_Phys_Dmg_Med = 86,
    Reduce_Phys_Dmg_High = 87,
    Reduce_Fire_Dmg_Low = 88,
    Reduce_Fire_Dmg_Med = 89,
    Reduce_Fire_Dmg_High = 90,
    Reduce_Ice_Dmg_Low = 91,
    Reduce_Ice_Dmg_Med = 92,
    Reduce_Ice_Dmg_High = 93,
    Reduce_Elec_Dmg_Low = 94,
    Reduce_Elec_Dmg_Med = 95,
    Reduce_Elec_Dmg_High = 96,
    Reduce_Wind_Dmg_Low = 97,
    Reduce_Wind_Dmg_Med = 98,
    Reduce_Wind_Dmg_High = 99,
    Reduce_Light_Dmg_Low = 100,
    Reduce_Light_Dmg_Med = 101,
    Reduce_Light_Dmg_High = 102,
    Reduce_Dark_Dmg_Low = 103,
    Reduce_Dark_Dmg_Med = 104,
    Reduce_Dark_Dmg_High = 105,
    Reduce_Magic_Dmg_Low = 106,
    Reduce_Magic_Dmg_Med = 107,
    Reduce_Magic_Dmg_High = 108,
    Slash_Evasion_Low = 109,
    Slash_Evasion_Med = 110,
    Slash_Evasion_High = 111,
    Strike_Evasion_Low = 112,
    Strike_Evasion_Med = 113,
    Strike_Evasion_High = 114,
    Pierce_Evasion_Low = 115,
    Pierce_Evasion_Med = 116,
    Pierce_Evasion_High = 117,
    Physical_Evasion_Low = 118,
    Physical_Evasion_Med = 119,
    Physical_Evasion_High = 120,
    Fire_Evasion_Low = 121,
    Fire_Evasion_Med = 122,
    Fire_Evasion_High = 123,
    Ice_Evasion_Low = 124,
    Ice_Evasion_Med = 125,
    Ice_Evasion_High = 126,
    Electricity_Evasion_Low = 127,
    Electricity_Evasion_Med = 128,
    Electricity_Evasion_High = 129,
    Wind_Evasion_Low = 130,
    Wind_Evasion_Med = 131,
    Wind_Evasion_High = 132,
    Light_Evasion_Low = 133,
    Light_Evasion_Med = 134,
    Light_Evasion_High = 135,
    Dark_Evasion_Low = 136,
    Dark_Evasion_Med = 137,
    Dark_Evasion_High = 138,
    Magic_Evasion_Low = 139,
    Magic_Evasion_Med = 140,
    Magic_Evasion_High = 141,
    Exp_x150 = 142,
    Exp_x115 = 143,
    Slash_Damage_Up = 144,
    Strike_Damage_Up = 145,
    Pierce_Damage_Up = 146,
    Fire_Damage_Up = 147,
    Ice_Damage_Up = 148,
    Electricity_Damage_Up = 149,
    Wind_Damage_Up = 150,
    Light_Damage_Up = 151,
    Dark_Damage_Up = 152,
    Slash_Defense_Up = 153,
    Strike_Defense_Up = 154,
    Pierce_Defense_Up = 155,
    Fire_Defense_Up = 156,
    Ice_Defense_Up = 157,
    Electricity_Defense_Up = 158,
    Wind_Defense_Up = 159,
    Light_Defense_Up = 160,
    Dark_Defense_Up = 161,
    Attack_When_Enraged = 162,
    Regenerate_1 = 163,
    Regenerate_2 = 164,
    Regenerate_3 = 165,
    Invigorate_1 = 166,
    Invigorate_2 = 167,
    Invigorate_3 = 168,
    Null_Light_Instakill = 169,
    Null_Dark_Instakill = 170,
    Ali_Dance = 171,
    Null_All_Except_Almighty = 172,
    Firm_Stance = 173,
    Soul_Chain = 174,
    All_Out_Attack_Dmg_Up = 175,
    Null_Slash = 177,
    Null_Strike = 178,
    Null_Pierce = 179,
    Null_Fire = 180,
    Null_Ice = 181,
    Null_Elec = 182,
    Null_Wind = 183,
    Null_Light = 184,
    Null_Dark = 185,
    Slash_Amp = 186,
    Strike_Amp = 187,
    Pierce_Amp = 188,
    Fire_Amp = 189,
    Ice_Amp = 190,
    Elec_Amp = 191,
    Wind_Amp = 192,
    Light_Amp = 193,
    Dark_Amp = 194,
    Resist_Ailments = 195,
    Unshaken_Will = 196,
    Fast_Heal = 197,
    Insta_Heal = 198,
    Life_Aid = 199,
    Endure = 200,
    Enduring_Soul = 201,
    Arms_Master = 202,
    Spell_Master = 203,
    Apt_Pupil = 204,
    Sharp_Student = 205,
    Freeze_Boost = 206,
    Shock_Boost = 207,
    Confuse_Boost = 208,
    Fear_Boost = 209,
    Distress_Boost = 210,
    Charm_Boost = 211,
    Rage_Boost = 212,
    Poison_Boost = 213,
    Survive_Light = 214,
    Endure_Light = 215,
    Survive_Dark = 216,
    Endure_Dark = 217,
    Survival_Trick = 218,
    Auto_Mataru = 219,
    Auto_Maraku = 220,
    Auto_Masuku = 221,
    Auto_Rebellion = 222,
    Anti_Fire_Master = 223,
    Anti_Ice_Master = 224,
    Anti_Electric_Master = 225,
    Anti_Wind_Master = 226,
    Soul_Shift = 227,
    Soul_Link = 229,
    Shift_Boost = 230,
    Shift_Amp = 231,
    Dmg_On_Downed_Enemy = 232,
    Dizzy_Boost = 233,
    Ignore_Enemy_Resistances = 234,
    Orgia_Duration_1_Turn = 235,
    Faster_Overheat_Recovery = 236,
    SP_HP_100_100 = 248,
    Auto_Tarukabellion = 249,
    Auto_Charge = 250,
    Auto_Concentrate = 251,
    Auto_Heat_Riser = 252,
    Regenerate_3_Invigorate_3 = 253,
    Magic_Ability = 254,
    Null_All_Ailments = 255,
}
public enum EItemSkill
{
    NONE = 0,
    HP_10 = 1,
    HP_20 = 2,
    HP_30 = 3,
    HP_40 = 4,
    HP_50 = 5,
    HP_100 = 6,
    SP_10 = 7,
    SP_20 = 8,
    SP_30 = 9,
    SP_40 = 10,
    SP_50 = 11,
    SP_100 = 12,
    Freeze_Low = 13,
    Freeze_Med = 14,
    Freeze_High = 15,
    Shock_Low = 16,
    Shock_Med = 17,
    Shock_High = 18,
    Confuse_Low = 19,
    Confuse_Med = 20,
    Confuse_High = 21,
    Fear_Low = 22,
    Fear_Med = 23,
    Fear_High = 24,
    Distress_Low = 25,
    Distress_Med = 26,
    Distress_High = 27,
    Charm_Low = 28,
    Charm_Med = 29,
    Charm_High = 30,
    Rage_Low = 31,
    Rage_Med = 32,
    Rage_High = 33,
    Poison_Low = 34,
    Poison_Med = 35,
    Poison_High = 36,
    Random_Ailment_Low = 37,
    Random_Ailment_Med = 38,
    Random_Ailment_High = 39,
    Critical_Rate_Up_Low = 40,
    Critical_Rate_Up_Med = 41,
    Critical_Rate_Up_High = 42,
    Slash_Boost = 43,
    Strike_Boost = 44,
    Pierce_Boost = 45,
    Fire_Boost = 46,
    Ice_Boost = 47,
    Elec_Boost = 48,
    Wind_Boost = 49,
    Light_Boost = 50,
    Dark_Boost = 51,
    Counter = 52,
    Counterstrike = 53,
    High_Counter = 54,
    Resist_Freeze = 55,
    Resist_Shock = 56,
    Resist_Confuse = 57,
    Resist_Fear = 58,
    Resist_Distress = 59,
    Resist_Charm = 60,
    Resist_Rage = 61,
    Resist_Poison = 62,
    Resist_Dizzy = 63,
    Null_Freeze = 64,
    Null_Shock = 65,
    Null_Confuse = 66,
    Null_Fear = 67,
    Null_Distress = 68,
    Null_Charm = 69,
    Null_Rage = 70,
    Null_Poison = 71,
    Null_Dizzy = 72,
    Auto_Tarukaja = 73,
    Auto_Rakukaja = 74,
    Auto_Sukukaja = 75,
    Reduce_Slash_Dmg_Low = 76,
    Reduce_Slash_Dmg_Med = 77,
    Reduce_Slash_Dmg_High = 78,
    Reduce_Strike_Dmg_Low = 79,
    Reduce_Strike_Dmg_Med = 80,
    Reduce_Strike_Dmg_High = 81,
    Reduce_Pierce_Dmg_Low = 82,
    Reduce_Pierce_Dmg_Med = 83,
    Reduce_Pierce_Dmg_High = 84,
    Reduce_Phys_Dmg_Low = 85,
    Reduce_Phys_Dmg_Med = 86,
    Reduce_Phys_Dmg_High = 87,
    Reduce_Fire_Dmg_Low = 88,
    Reduce_Fire_Dmg_Med = 89,
    Reduce_Fire_Dmg_High = 90,
    Reduce_Ice_Dmg_Low = 91,
    Reduce_Ice_Dmg_Med = 92,
    Reduce_Ice_Dmg_High = 93,
    Reduce_Elec_Dmg_Low = 94,
    Reduce_Elec_Dmg_Med = 95,
    Reduce_Elec_Dmg_High = 96,
    Reduce_Wind_Dmg_Low = 97,
    Reduce_Wind_Dmg_Med = 98,
    Reduce_Wind_Dmg_High = 99,
    Reduce_Light_Dmg_Low = 100,
    Reduce_Light_Dmg_Med = 101,
    Reduce_Light_Dmg_High = 102,
    Reduce_Dark_Dmg_Low = 103,
    Reduce_Dark_Dmg_Med = 104,
    Reduce_Dark_Dmg_High = 105,
    Reduce_Magic_Dmg_Low = 106,
    Reduce_Magic_Dmg_Med = 107,
    Reduce_Magic_Dmg_High = 108,
    Slash_Evasion_Low = 109,
    Slash_Evasion_Med = 110,
    Slash_Evasion_High = 111,
    Strike_Evasion_Low = 112,
    Strike_Evasion_Med = 113,
    Strike_Evasion_High = 114,
    Pierce_Evasion_Low = 115,
    Pierce_Evasion_Med = 116,
    Pierce_Evasion_High = 117,
    Physical_Evasion_Low = 118,
    Physical_Evasion_Med = 119,
    Physical_Evasion_High = 120,
    Fire_Evasion_Low = 121,
    Fire_Evasion_Med = 122,
    Fire_Evasion_High = 123,
    Ice_Evasion_Low = 124,
    Ice_Evasion_Med = 125,
    Ice_Evasion_High = 126,
    Electricity_Evasion_Low = 127,
    Electricity_Evasion_Med = 128,
    Electricity_Evasion_High = 129,
    Wind_Evasion_Low = 130,
    Wind_Evasion_Med = 131,
    Wind_Evasion_High = 132,
    Light_Evasion_Low = 133,
    Light_Evasion_Med = 134,
    Light_Evasion_High = 135,
    Dark_Evasion_Low = 136,
    Dark_Evasion_Med = 137,
    Dark_Evasion_High = 138,
    Magic_Evasion_Low = 139,
    Magic_Evasion_Med = 140,
    Magic_Evasion_High = 141,
    Exp_x150 = 142,
    Exp_x115 = 143,
    Slash_Damage_Up = 144,
    Strike_Damage_Up = 145,
    Pierce_Damage_Up = 146,
    Fire_Damage_Up = 147,
    Ice_Damage_Up = 148,
    Electricity_Damage_Up = 149,
    Wind_Damage_Up = 150,
    Light_Damage_Up = 151,
    Dark_Damage_Up = 152,
    Slash_Defense_Up = 153,
    Strike_Defense_Up = 154,
    Pierce_Defense_Up = 155,
    Fire_Defense_Up = 156,
    Ice_Defense_Up = 157,
    Electricity_Defense_Up = 158,
    Wind_Defense_Up = 159,
    Light_Defense_Up = 160,
    Dark_Defense_Up = 161,
    Attack_When_Enraged = 162,
    Regenerate_1 = 163,
    Regenerate_2 = 164,
    Regenerate_3 = 165,
    Invigorate_1 = 166,
    Invigorate_2 = 167,
    Invigorate_3 = 168,
    Null_Light_Instakill = 169,
    Null_Dark_Instakill = 170,
    Ali_Dance = 171,
    Null_All_Except_Almighty = 172,
    Firm_Stance = 173,
    Soul_Chain = 174,
    All_Out_Attack_Dmg_Up = 175,
    Null_Slash = 177,
    Null_Strike = 178,
    Null_Pierce = 179,
    Null_Fire = 180,
    Null_Ice = 181,
    Null_Elec = 182,
    Null_Wind = 183,
    Null_Light = 184,
    Null_Dark = 185,
    Slash_Amp = 186,
    Strike_Amp = 187,
    Pierce_Amp = 188,
    Fire_Amp = 189,
    Ice_Amp = 190,
    Elec_Amp = 191,
    Wind_Amp = 192,
    Light_Amp = 193,
    Dark_Amp = 194,
    Resist_Ailments = 195,
    Unshaken_Will = 196,
    Fast_Heal = 197,
    Insta_Heal = 198,
    Life_Aid = 199,
    Endure = 200,
    Enduring_Soul = 201,
    Arms_Master = 202,
    Spell_Master = 203,
    Apt_Pupil = 204,
    Sharp_Student = 205,
    Freeze_Boost = 206,
    Shock_Boost = 207,
    Confuse_Boost = 208,
    Fear_Boost = 209,
    Distress_Boost = 210,
    Charm_Boost = 211,
    Rage_Boost = 212,
    Poison_Boost = 213,
    Survive_Light = 214,
    Endure_Light = 215,
    Survive_Dark = 216,
    Endure_Dark = 217,
    Survival_Trick = 218,
    Auto_Mataru = 219,
    Auto_Maraku = 220,
    Auto_Masuku = 221,
    Auto_Rebellion = 222,
    Anti_Fire_Master = 223,
    Anti_Ice_Master = 224,
    Anti_Electric_Master = 225,
    Anti_Wind_Master = 226,
    Soul_Shift = 227,
    Soul_Link = 229,
    Shift_Boost = 230,
    Shift_Amp = 231,
    Dmg_On_Downed_Enemy = 232,
    Dizzy_Boost = 233,
    Ignore_Enemy_Resistances = 234,
    Orgia_Duration_1_Turn = 235,
    Faster_Overheat_Recovery = 236,
    SP_HP_100_100 = 248,
    Auto_Tarukabellion = 249,
    Auto_Charge = 250,
    Auto_Concentrate = 251,
    Auto_Heat_Riser = 252,
    Regenerate_3_Invigorate_3 = 253,
    Magic_Ability = 254,
    Null_All_Ailments = 255,
}