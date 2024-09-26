using P3R.WeaponFramework.Core;
using P3R.WeaponFramework.Weapons.Models;
using System.Reflection;
using System.Text;
using Unreal.AtlusScript.Interfaces;

namespace P3R.WeaponFramework.Weapons;

internal class WeaponDescService
{
    public delegate bool AstreaSaveCheck();
    public AstreaSaveCheck? IsAstreaSave;
    public bool AstreaSave
    {
        get
        {
            var save = IsAstreaSave;
            if (save == null)
                return false;
            else
                return save.Invoke();
        }
    }
    private readonly IAtlusAssets atlusAssets;
    private readonly List<string> entriesVanilla = [];
    private readonly List<string> entriesAstrea = [];
    public List<string> currentEntries {
        get
        {
            return AstreaSave ? entriesAstrea : entriesVanilla;
        }       
     }

    public WeaponDescService(IAtlusAssets atlusAssets)
    {
        ScanHooks.Listen("WF_IsAstreaSave", (hooks, result) => this.IsAstreaSave = hooks.CreateWrapper<AstreaSaveCheck>(result, out _));
        this.atlusAssets = atlusAssets;
        LoadDescEntries();
        LoadDescEntries(true);
    }
    public void Init()
    {
        var sb = new StringBuilder();
        if (AstreaSave)
        {
            for (int i = 0; i < entriesAstrea.Count; i++)
            {
                sb.AppendLine($"[msg Item_{i:D3}]");
                sb.AppendLine($"[f 2 1]{entriesAstrea[i]}[n][e]");
            }
        }
        else
        {
            Log.Verbose($"{entriesVanilla.Count}");
            for (int i = 0; i < entriesVanilla.Count; i++)
            {
                sb.AppendLine($"[msg Item_{i:D3}]");
                sb.AppendLine($"[f 2 1]{entriesVanilla[i]}[n][e]");
            }
        }
        this.atlusAssets.AddAsset("BMD_ItemWeaponHelp", sb.ToString(), AssetType.BMD, AssetMode.Both);
    }
    public void SetWeaponDesc(int weaponItemId, string weaponDesc)
    {
        if (AstreaSave)
        {
            entriesAstrea[weaponItemId] = weaponDesc;
        }
        else
        {
            entriesVanilla[weaponItemId] = weaponDesc;
        }
    }

    private void LoadDescEntries(bool isAstrea = false)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = isAstrea ? "P3R.WeaponFramework.Resources.Astrea.Descriptions.msg" : "P3R.WeaponFramework.Resources.Vanilla.Descriptions.msg";
        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        if (isAstrea)
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line?.StartsWith("[f") == true)
                {
                    if (!line.EndsWith("[n][e]"))
                    {
                        line = $"{line}[n][e]";
                    }
                    this.entriesAstrea.Add(line);
                }
            }

            // Add placeholder entries.
            for (int i = 0; i < GameWeapons.NUM_EPISODE_WEAPS; i++)
            {
                this.entriesAstrea.Add("[f 2 1]Placeholder.[n][e]");
            }
        }
        else
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line?.StartsWith("[f") == true)
                {
                    if (!line.EndsWith("[n][e]"))
                    {
                        line = $"{line}[n][e]";
                    }
                    this.entriesVanilla.Add(line);
                }
            }

            // Add placeholder entries.
            for (int i = 0; i < 100; i++)
            {
                this.entriesVanilla.Add("[f 2 1]Placeholder.[n][e]");
            }
        }
    }
}