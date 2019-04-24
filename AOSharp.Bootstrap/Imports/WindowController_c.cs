using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Bootstrap.Imports
{
    public class WindowController_c
    {
        [DllImport("GUI.dll", EntryPoint = "?ViewDeleted@WindowController_c@@QAEXPAVView@@@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern void ViewDeleted(IntPtr pThis, IntPtr pView);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DViewDeleted(IntPtr pThis, IntPtr pView);
    }
}
