using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Tools.DataUtils.Extensions
{
    internal static class PathExtensions
    {
        public static string GetEmbeddedFileName(this string path) => Path.GetFileNameWithoutExtension(path).Split('.').Last();
    }
}
