using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Interfaces
{
    public unsafe static class PointerUtils
    {
        public static T* AsType<T>(this nint ptr) => (T*)&ptr;
    }
}
