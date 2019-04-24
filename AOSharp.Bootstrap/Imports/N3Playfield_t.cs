using System;
using System.Runtime.InteropServices;

namespace AOSharp.Bootstrap.Imports
{
    public class N3Playfield_t
    {
        [DllImport("N3.dll", EntryPoint = "?AddChildDynel@n3Playfield_t@@QAEXPAVn3Dynel_t@@ABVVector3_t@@ABVQuaternion_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void AddChildDynel(IntPtr pThis, IntPtr pDynel, IntPtr pos, IntPtr rot);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DAddChildDynel(IntPtr pThis, IntPtr pDynel, IntPtr pos, IntPtr rot);
    }
}
