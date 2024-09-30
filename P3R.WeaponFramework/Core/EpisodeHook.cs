﻿using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework.Core;

internal unsafe class EpisodeHookBase
{
    public delegate bool IsAstreaSave();
    public IsAstreaSave? isAstreaSave;

    public static Episode Vanilla = new(FEpisode.Vanilla);
    public static Episode Astrea = new(FEpisode.Astrea);

    public Episode GetCurrentEpisode()
    {
        if (AstreaSave)
            return Astrea;
        else
            return Vanilla;
    }

    public List<Weapon> Weapons => GetCurrentEpisode().Weapons;
    public List<string> Descriptions => GetCurrentEpisode().Descriptions;

    public bool InvokeAstreaSave() { return isAstreaSave != null && isAstreaSave.Invoke(); }
    public bool AstreaSave => InvokeAstreaSave();
    public EpisodeHookBase()
    {
        ScanHooks.Add(
        "WF_IsAstreaSave",
            "48 83 EC 28 E8 ?? ?? ?? ?? 48 85 C0 74 ?? E8 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? 3C 01 0F 94 C0 48 83 C4 28 C3 48 83 C4 28 C3",
            (hooks, result) => this.isAstreaSave = hooks.CreateWrapper<IsAstreaSave>(result, out _));
    }

    public void IfAstrea(Action astreaCallback, Action vanillaCallback)
    {
        if (InvokeAstreaSave())
        {
            astreaCallback();
        }
        else
        {
            vanillaCallback();
        }
    }

}