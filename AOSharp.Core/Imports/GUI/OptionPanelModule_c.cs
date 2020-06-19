using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    internal class OptionPanelModule_c
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate IntPtr GetOptionWindowDelegate(IntPtr pThis);
        internal static GetOptionWindowDelegate GetOptionWindow; 
    }
}
