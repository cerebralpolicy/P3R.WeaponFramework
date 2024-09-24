using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Tools.DataUtils;

internal struct AppCharWeaponTableRow
{
    public string Name;
    public Dictionary<int,AppCharWeaponMeshData> Data;
    public string Anim;

}

internal struct AppCharWeaponMeshData
{
    public string Mesh;
    public bool MultiEquip;
}

internal static partial class Subroutines
{
    
}
