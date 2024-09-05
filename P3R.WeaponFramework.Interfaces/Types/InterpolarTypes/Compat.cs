using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ObjEmitterT = Unreal.ObjectsEmitter.Interfaces.Types;
using NativeTypeT = p3rpc.nativetypes.Interfaces;
using System.Reflection;

namespace P3R.WeaponFramework.Interfaces.Types.InterpolarTypes
{
    internal class Compat
    {
        public Type[] ObjEmitterTypes;
        public Type[] NativeTypeTypes;

        public Compat()
        {
            ObjEmitterTypes = [];
            NativeTypeTypes = [];
        }

        public void Init()
        {
            var objEmitter = Assembly.GetAssembly(typeof(ObjEmitterT.UObject))!;
            var nativeType = Assembly.GetAssembly(typeof(NativeTypeT.UObject))!;
            ObjEmitterTypes = objEmitter.GetTypes();
            NativeTypeTypes = nativeType.GetTypes();
        }
    }
    public static class TypeConversions
    {
        #region setup
        internal static Type Unreal = typeof(ObjEmitterT.UObject);
        internal static Type Native = typeof(NativeTypeT.UObject);
        internal static Assembly? GetAssembly(this Type type) => Assembly.GetAssembly(type);
        internal static Type[]? GetSiblings(this Type type) => GetAssembly(type)?.GetExportedTypes();
        private static string[]? GetNames(this Type type)
        {
            var types = GetSiblings(type);
            if (types == null) return null;
            string[] names = new string[types.Length];
            for (int i = 0;  i < types.Length; i++)
            {
                string name = types[i].Name;
                names[i] = name;
            }
            return names;
        }
        #endregion
        public static Type[]? NativeTypes => GetSiblings(Native);
        public static Type[]? UnrealTypes => GetSiblings(Unreal);
        public static string[]? NativeNames => GetNames(Native);
        public static string[]? UnrealNames => GetNames(Unreal);

        public static object? ToNative<TNative, TUnreal>(this TUnreal unreal)
        {
            if (UnrealTypes == null || NativeTypes == null)
                return default(TNative);
            var unrealExists = UnrealTypes.Contains(typeof(TUnreal));
            var nativeExists = NativeTypes.Contains(typeof(TNative));
            if( !unrealExists || !nativeExists )
                return default(TNative);
            var value = (object?)unreal;
            return (TNative?)value;
        }
    }
}
