using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    internal class WeaponHolder_t
    {
        //55 8B EC 8B 45 08 56 8B F1 85 C0 78 05 
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate IntPtr GetWeaponDelegate(IntPtr pThis, int slot, int unk);
        internal static GetWeaponDelegate GetWeapon = Marshal.GetDelegateForFunctionPointer<GetWeaponDelegate>(Kernal32.GetModuleHandle("Gamecode.dll") + 0x6802F);

        //55 8B EC 83 EC 18 33 C0 56 8B F1 39 45 08 75 07  
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate byte IsDynelInWeaponRangeDelegate(IntPtr pThis, IntPtr pWeapon, IntPtr pDynel);
        internal static IsDynelInWeaponRangeDelegate IsDynelInWeaponRange = Marshal.GetDelegateForFunctionPointer<IsDynelInWeaponRangeDelegate>(Kernal32.GetModuleHandle("Gamecode.dll") + 0x6797E);

        //55 8B EC 51 56 8D 45 08 50 8B F1 8D 45 FC 50 8D 4E 0C E8 ? ? ? ? 8B 45 FC 3B 46 10 5E 74 05   
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate IntPtr GetDummyWeaponDelegate(IntPtr pThis, Stat stat);
        internal static GetDummyWeaponDelegate GetDummyWeapon = Marshal.GetDelegateForFunctionPointer<GetDummyWeaponDelegate>(Kernal32.GetModuleHandle("Gamecode.dll") + 0x6868D);
    }
}
