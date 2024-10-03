using P3R.WeaponFramework.Core;
using P3R.WeaponFramework.Weapons.Models;
using System.Reflection;
using System.Text;
using Unreal.AtlusScript.Interfaces;

namespace P3R.WeaponFramework.Weapons;

internal class WeaponDescService
{
    private EpisodeHook episodeHook;
    private readonly IAtlusAssets atlusAssets;

    private List<string> Descriptions = new();

    public void AddDescription(string description) => Descriptions.Add(description);

    public WeaponDescService(EpisodeHook episodeHook, IAtlusAssets atlusAssets)
    {
        this.episodeHook = episodeHook;
        this.atlusAssets = atlusAssets;
    }
    public void Prime()
    {
        Descriptions.Clear();
        Descriptions = episodeHook.Descriptions;
    }
    public void Init()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Descriptions.Count; i++)
        {
            sb.AppendLine($"[msg Item_{i:D3}]");
            sb.AppendLine($"[uf 0 5 65278][uf 2 1]{Descriptions[i]}[n][e]");
            Log.Verbose($"Description {i:D3}: {Descriptions[i]}");

        }
        Log.Debug($"{Descriptions.Count} descriptions found.");
        var output = sb.ToString();
        this.atlusAssets.AddAsset("BMD_ItemWeaponHelp", output, AssetType.BMD, AssetMode.Both);
    }
    public void SetWeaponDesc(int weaponItemId, string weaponDesc) => Descriptions[weaponItemId] = weaponDesc;
}