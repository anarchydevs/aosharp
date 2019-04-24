using System;
using System.Runtime.InteropServices;

namespace AOSharp.Bootstrap.Imports
{
    public class OptionPanelModule_c
    {
        [DllImport("GUI.dll", EntryPoint = "?ModuleActivated@OptionPanelModule_c@@UAEX_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void ModuleActivated(IntPtr pThis, bool unk);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DModuleActivated(IntPtr pThis, bool unk);
    }
}
