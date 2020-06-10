using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    internal class WeaponHolder_t
    {
        //55 8B EC 8B 45 08 56 8B F1 85 C0 78 05 
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate IntPtr GetWeaponDelegate(IntPtr pThis, EquipSlot slot, int unk);
        internal static GetWeaponDelegate GetWeapon = Marshal.GetDelegateForFunctionPointer<GetWeaponDelegate>(Kernel32.GetModuleHandle("Gamecode.dll") + FuncOffsets.GetWeapon);

        //55 8B EC 83 EC 18 33 C0 56 8B F1 39 45 08 75 07  
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate byte IsDynelInWeaponRangeDelegate(IntPtr pThis, IntPtr pWeapon, IntPtr pDynel);
        internal static IsDynelInWeaponRangeDelegate IsDynelInWeaponRange = Marshal.GetDelegateForFunctionPointer<IsDynelInWeaponRangeDelegate>(Kernel32.GetModuleHandle("Gamecode.dll") + FuncOffsets.IsDynelInWeaponRange);

        //55 8B EC 83 EC 18 56 8B F1 8B 4E 08 E8 ? ? ? ? 8B 48 5C 89 4D E8 8B 40 60 89 45 EC 8D 45 E8 50 FF 15 ? ? ? ? 59 89 45 F8 85 C0 0F 84 ? ? ? ? 
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate byte IsInRangeDelegate(IntPtr pThis);
        internal static IsInRangeDelegate IsInRange = Marshal.GetDelegateForFunctionPointer<IsInRangeDelegate>(Kernel32.GetModuleHandle("Gamecode.dll") + FuncOffsets.IsInRange);

        //55 8B EC 51 56 8D 45 08 50 8B F1 8D 45 FC 50 8D 4E 0C E8 ? ? ? ? 8B 45 FC 3B 46 10 5E 74 05   
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate IntPtr GetDummyWeaponDelegate(IntPtr pThis, Stat stat);
        internal static GetDummyWeaponDelegate GetDummyWeapon = Marshal.GetDelegateForFunctionPointer<GetDummyWeaponDelegate>(Kernel32.GetModuleHandle("Gamecode.dll") + FuncOffsets.GetDummyWeapon);
    }
}
