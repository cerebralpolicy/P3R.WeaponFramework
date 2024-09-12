using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Interfaces
{
    public unsafe static class PtrHandler
    {
        public static T* ToType<T>(this nint ptr) where T : struct => (T*)(ptr);
        public static nint ToPtr<T>(this T obj) => (nint)(&obj);
    }
}
