using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Types;

[Flags]
public enum SkillFlag : ushort
{
    HPBoost10 = 1,

    ConfuseBoostHigh = 21,
    FearBoostHigh = 24,

    AllStats1 = 42,
}