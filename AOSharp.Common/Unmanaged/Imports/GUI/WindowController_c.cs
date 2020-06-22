using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class WindowController_c
    {
        [DllImport("GUI.dll", EntryPoint = "?GetInstanceIfAny@WindowController_c@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstance();

        [DllImport("GUI.dll", EntryPoint = "?ViewDeleted@WindowController_c@@QAEXPAVView@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void ViewDeleted(IntPtr pThis, IntPtr pView);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DViewDeleted(IntPtr pThis, IntPtr pView);
    }
}
