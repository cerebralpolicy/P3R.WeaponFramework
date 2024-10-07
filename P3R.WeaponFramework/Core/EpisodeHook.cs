using P3R.WeaponFramework.Weapons.Models;
using System.Reflection;

namespace P3R.WeaponFramework.Core;

public unsafe class EpisodeHook
{
    public delegate bool IsAstreaSave();
    public IsAstreaSave? isAstreaSave;

    private Episode _astrea;
    private Episode _vanilla;

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
    public EpisodeHook(Episode astrea, Episode vanilla)
    {
        ScanHooks.Add(
        "WF_IsAstreaSave",
            "48 83 EC 28 E8 ?? ?? ?? ?? 48 85 C0 74 ?? E8 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? 3C 01 0F 94 C0 48 83 C4 28 C3 48 83 C4 28 C3",
            (hooks, result) => this.isAstreaSave = hooks.CreateWrapper<IsAstreaSave>(result, out _));
        _astrea = astrea;
        _vanilla = vanilla;
    }

    public Episode Astrea => _astrea;
    public Episode Vanilla => _vanilla;

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