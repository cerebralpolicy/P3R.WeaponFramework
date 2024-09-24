using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Interfaces.Types;

[Flags]
public enum EEpisodeFlag
{
    NONE = 1 << 0,
    VANILLA = 1 << 1,
    ASTREA = 1 << 2,
    BOTH = VANILLA | ASTREA
}
