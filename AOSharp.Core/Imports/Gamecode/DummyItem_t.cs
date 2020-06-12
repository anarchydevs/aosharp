using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    internal class DummyItem_t
    {
        //55 8B EC 8B 45 08 56 8B F1 85 C0 78 05 
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate int GetStatDelegate(IntPtr pThis, Stat stat, int detail);
        internal static GetStatDelegate GetStat = Marshal.GetDelegateForFunctionPointer<GetStatDelegate>(Kernel32.GetModuleHandle("Gamecode.dll") + FuncOffsets.GetStatDummyItem);
    }
}
