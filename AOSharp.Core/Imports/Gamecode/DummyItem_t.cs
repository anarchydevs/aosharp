using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    internal class DummyItem_t
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate int GetStatDelegate(IntPtr pThis, Stat stat, int detail);
        internal static GetStatDelegate GetStat;
    }
}
