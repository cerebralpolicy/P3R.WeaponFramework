using System.Reflection;
using System.Text;
using Unreal.AtlusScript.Interfaces;

namespace P3R.WeaponFramework.Weapons;

internal class WeaponDescService
{
    private readonly IAtlusAssets atlusAssets;

    private readonly List<string> descEntries = new();

    public WeaponDescService(IAtlusAssets atlusAssets)
    {
        this.atlusAssets = atlusAssets;
        LoadDescEntries();
    }

    public void Init()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < this.descEntries.Count; i++)
        {
            sb.AppendLine($"[msg Item_{i:D3}]");
            sb.AppendLine($"[f 2 1]{this.descEntries[i]}[n][e]");
        }

        this.atlusAssets.AddAsset("BMD_ItemWeaponHelp", sb.ToString(), AssetType.BMD, AssetMode.Both);
    }
    public void SetWeaponDesc(int weaponItemId, string weaponDesc)
    => this.descEntries[weaponItemId] = weaponDesc;

    private void LoadDescEntries()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "P3R.WeaponFramework.Resources.EN.Descriptions.msg";
        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line?.StartsWith("[f 2 1]") == true)
            {
                if (!line.EndsWith("[n][e]"))
                {
                    line = $"{line}[n][e]";
                }

                this.descEntries.Add(line);
            }
        }

        // Add placeholder entries.
        for (int i = 0; i < 100; i++)
        {
            this.descEntries.Add("[f 2 1]Placeholder.[n][e]");
        }
    }

}