using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class DummyItem_t
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public unsafe delegate int GetStatDelegate(IntPtr pThis, Stat stat, int detail);
        public static GetStatDelegate GetStat;
    }
}
