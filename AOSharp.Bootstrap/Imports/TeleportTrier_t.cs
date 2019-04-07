using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Bootstrap
{
    public class TeleportTrier_t
    {
        [DllImport("Gamecode.dll", EntryPoint = "?TeleportFailed@TeleportTrier_t@@QAEXXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern void TeleportFailed(IntPtr pThis);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DTeleportFailed(IntPtr pThis);
    }
}
