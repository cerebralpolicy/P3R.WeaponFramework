using P3R.WeaponFramework.Interfaces.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Interfaces
{
    public delegate void TMapAction<MapType, ElementType>(MapType map, ElementType element)
        where MapType : IEnumerable<TMapElement<int, ElementType>>
        where ElementType : unmanaged;
}
