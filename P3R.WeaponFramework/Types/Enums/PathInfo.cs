namespace P3R.WeaponFramework.Types;

public struct PathInfo
{
    private string basePath;
    private string shellPath;

    public string BasePath => basePath;
    public string ShellPath => shellPath;

    public PathInfo(EArmature armature)
    {
        var info = new Armature(armature);
        basePath = info.BasePath;
        shellPath = info.ShellPath;
    }
    public PathInfo(Armature info)
    {
        basePath = info.BasePath;
        shellPath = info.ShellPath;
    }

    public PathInfo(string basePath, string shellPath)
    {
        this.basePath = basePath;
        this.shellPath = shellPath;
    }
}
