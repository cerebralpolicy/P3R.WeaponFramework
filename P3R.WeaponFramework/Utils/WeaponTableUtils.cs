using P3R.WeaponFramework.Weapons.Models;
using System.Diagnostics.CodeAnalysis;

namespace P3R.WeaponFramework;

internal static class WeaponTableUtils
{
    public static bool Equivalent<T1, T2>(this IList<T1> List1, IList<T2> List2, [NotNullWhen(true)] out int? count)
    {
        count = null;
        
        var check = List1.Count == List2.Count && List1.Count != 0;
        if (check)
            count = List1.Count;
        return check;
    }

    public static List<ShellType> AigisShells() => [ShellType.Aigis_SmallArms, ShellType.Aigis_LongArms, ShellType.Aigis_SmallArms_Astrea, ShellType.Aigis_LongArms_Astrea];
    public static List<ShellType> AigisSmallArms() => [ShellType.Aigis_SmallArms, ShellType.Aigis_SmallArms_Astrea];
    public static List<ShellType> AigisLongArms() => [ShellType.Aigis_LongArms, ShellType.Aigis_LongArms_Astrea];
    public static List<EArmature> AigisArmatures() => [EArmature.Wp0007_01, EArmature.Wp0007_02, EArmature.Wp0007_03, EArmature.Wp0012_01, EArmature.Wp0012_02, EArmature.Wp0012_03];
    public static bool IsAigisShell(this ShellType shell) => AigisShells().Contains(shell);
    public static bool IsAigisSmallArm(this ShellType shell) => AigisSmallArms().Contains(shell);
    public static bool IsAigisLongArm(this ShellType shell) => AigisLongArms().Contains(shell);
    public static bool IsAigisArmature(EArmature armature) => AigisArmatures().Contains(armature);
    
}