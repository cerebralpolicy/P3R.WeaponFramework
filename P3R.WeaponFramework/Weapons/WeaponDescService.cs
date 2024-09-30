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
        this.atlusAssets.AddAsset("BMD_ItemWeaponHelp", BuildStrings(), AssetType.BMD, AssetMode.Both);
    }
    public void SetWeaponDesc(int weaponItemId, string weaponDesc)
    {
        Descriptions[weaponItemId] = weaponDesc;
    }
}