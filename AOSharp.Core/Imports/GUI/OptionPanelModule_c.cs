using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    internal class OptionPanelModule_c
    {
        //55 8B EC 51 33 C0 39 41 10 74 10 8B 41 0C 8B 00 8D 4D FC 89 45 FC E8 ? ? ? ? 
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate IntPtr GetOptionsWindowDelegate(IntPtr pThis);
        internal static GetOptionsWindowDelegate GetOptionWindow = Marshal.GetDelegateForFunctionPointer<GetOptionsWindowDelegate>(Kernel32.GetModuleHandle("GUI.dll") + FuncOffsets.GetOptionWindow); 
    }
}
