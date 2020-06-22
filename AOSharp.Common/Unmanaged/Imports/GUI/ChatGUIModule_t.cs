using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class ChatGUIModule_t
    {
        [DllImport("GUI.dll", EntryPoint = "?GetInstanceIfAny@ChatGUIModule_c@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstance();
    }
}
