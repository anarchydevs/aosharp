using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    internal class WeaponHolder_t
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal delegate IntPtr GetWeaponDelegate(IntPtr pThis, EquipSlot slot, int unk);
        internal static GetWeaponDelegate GetWeapon;
 
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal delegate byte IsDynelInWeaponRangeDelegate(IntPtr pThis, IntPtr pWeapon, IntPtr pDynel);
        internal static IsDynelInWeaponRangeDelegate IsDynelInWeaponRange;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal delegate byte IsInRangeDelegate(IntPtr pThis);
        internal static IsInRangeDelegate IsInRange;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal delegate IntPtr GetDummyWeaponDelegate(IntPtr pThis, Stat stat);
        internal static GetDummyWeaponDelegate GetDummyWeapon;
    }
}
