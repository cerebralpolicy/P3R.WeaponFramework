﻿using System.Runtime.InteropServices;


namespace P3R.WeaponFramework.Interfaces.Types;

[StructLayout(LayoutKind.Sequential)]
public struct FUniqueObjectGuid
{
    public FGuid Guid;
}
