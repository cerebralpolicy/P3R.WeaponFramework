using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;
using FName = P3R.WeaponFramework.Types.FName;

namespace P3R.WeaponFramework;
public static partial class AssetUtils
{
    public unsafe static class UnsafeUtils
    {
        public static T? PtrToStruct<T>(T* ptr) => Marshal.PtrToStructure<T>((nint)ptr);

        public static void StructToPtr<T>(T stct, T* ptr, bool fDeleteOld)
        {
            Debug.Assert(stct != null);
            Marshal.StructureToPtr<T>(stct, (nint)ptr, fDeleteOld);
        }

        public static bool TryCastAsSkeletalMesh(UnrealObject uObj, IUnreal unreal, [NotNullWhen(true)] out USkeletalMesh* mesh)
        {
            USkeletalMesh* skeletalMesh = (USkeletalMesh*)uObj.Self;
            USkeleton? skeleton = null;
            var o = uObj.Self;
            try
            {
                skeleton = *(skeletalMesh->Skeleton);
            }
            catch (Exception e)
            {
                var msg = new StringBuilder();
                msg.Append(e.Source);
                msg.AppendLine(e.StackTrace);
                Log.Error(e, msg.ToString());
            }
            if (skeleton == null)
            {
                mesh = null;
                return false;
            }
            mesh = skeletalMesh;
            return true;
        }
        public static string? GetRowStructName(DataTable table, IUnreal unreal) 
        {
            try
            {
                var t = table.Self;
                var rowStruct = t->RowStruct;
                var fname = rowStruct->baseObj.NamePrivate;
                var str = unreal.GetName(fname);
                return str;
            }
            catch (Exception e)
            {
                var msg = new StringBuilder();
                msg.Append(e.Source);
                msg.AppendLine(e.StackTrace);
                Log.Error(e, msg.ToString());
                return null;
            }
        }
    }
}
