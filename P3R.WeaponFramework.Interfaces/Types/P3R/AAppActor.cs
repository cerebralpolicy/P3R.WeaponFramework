using System;
using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x278)]
public unsafe struct AAppActor // : AActor
{
    [FieldOffset(0x230)] public EAppPauseObjectFlag AppPauseFlags;
}
