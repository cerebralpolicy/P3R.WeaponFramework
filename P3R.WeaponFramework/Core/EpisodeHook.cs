﻿namespace P3R.WeaponFramework.Core;

internal unsafe class EpisodeHook
{
    public delegate bool IsAstreaSave();
    public IsAstreaSave? isAstreaSave;
    public EpisodeHook()
    {
        ScanHooks.Add(
                "WF_IsAstreaSave",
                    "48 83 EC 28 E8 ?? ?? ?? ?? 48 85 C0 74 ?? E8 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? 3C 01 0F 94 C0 48 83 C4 28 C3 48 83 C4 28 C3",
                    (hooks, result) => this.isAstreaSave = hooks.CreateWrapper<IsAstreaSave>(result, out _));
    }
}
