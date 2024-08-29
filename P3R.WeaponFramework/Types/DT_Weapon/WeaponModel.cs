using static P3R.WeaponFramework.Weapons.WeaponExtensions;

namespace P3R.WeaponFramework.Weapons;

public struct WeaponModel
{
    public WeaponModel(int id)
    {
        IsVanillaAsset = true;
        ModelID = id;
        Asset = DefineVanillaAsset(ModelID);
    }

    private static AssetFNames DefineVanillaAsset(int id)
    {
        var character = (Character)(id % 10);
        var modelSet = ((WeaponModelID)(id - character)).ToWeapModelSet();
        var path = Assets.GetExistingModelPath(character, modelSet);
        return new(path);
    }

    public static implicit operator AssetFNames(WeaponModel model) => model.Asset;
    public static implicit operator int(WeaponModel model) => model.ModelID;
    public static implicit operator WeaponModel(int modelID) => new WeaponModel(modelID);

    private bool IsVanillaAsset { get; }
    public int ModelID { get; set; }
    public AssetFNames Asset { get; }
}
