using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core.Imports
{
    public class ChatGUIModule_t
    {
        [DllImport("GUI.dll", EntryPoint = "?GetInstanceIfAny@ChatGUIModule_c@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetInstance();
    }
}
