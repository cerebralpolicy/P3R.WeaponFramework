using P3R.WeaponFramework.Core;
using P3R.WeaponFramework.Weapons.Models;
using System.Reflection;
using System.Text;
using Unreal.AtlusScript.Interfaces;

namespace P3R.WeaponFramework.Weapons;

internal class WeaponDescService : EpisodeHookBase
{

    private readonly IAtlusAssets atlusAssets;
    
    public WeaponDescService(IAtlusAssets atlusAssets) : base()
    {

        this.atlusAssets = atlusAssets;
    }

    public string BuildStrings()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Descriptions.Count; i++)
        {
            sb.AppendLine($"[msg Item_{i:D3}]");
            sb.AppendLine($"[f 2 1]{Descriptions[i]}[n][e]");
        }
        return sb.ToString();
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
    public void SetWeaponDesc(int weaponItemId, string weaponDesc)
    {
        Descriptions[weaponItemId] = weaponDesc;
    }
}