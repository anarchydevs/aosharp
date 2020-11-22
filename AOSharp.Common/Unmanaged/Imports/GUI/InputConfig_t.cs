using AOSharp.Common.GameData;
using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class InputConfig_t
    {
        [DllImport("GUI.dll", EntryPoint = "?GetInstance@InputConfig_t@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstance ();

        [DllImport("GUI.dll", EntryPoint = "?GetCurrentTarget@InputConfig_t@@QBE?AVIdentity_t@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetCurrentTarget(IntPtr pThis, ref Identity identity);
    }
}
