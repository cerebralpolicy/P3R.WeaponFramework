namespace P3R.WeaponFramework.Tools.DataUtils
{
    internal class WeaponConfig
    {
        public WeaponConfig()
        {
        }

        public WeaponConfig(string? name, WeaponPartsData @base, WeaponPartsData mesh, WeaponStats? stats)
        {
            Name = name;
            Base = @base;
            Mesh = mesh;
            Stats = stats;
        }

        public string? Name { get; set; }
        public WeaponPartsData Base { get; set; } = new();
        public WeaponPartsData Mesh { get; set; } = new();

        public WeaponStats? Stats { get; set; } = new();
    }
    internal class WeaponPartsData
    {
        public string? MeshPath { get; set; }
        public string? AnimPath { get; set; }
    }
}
