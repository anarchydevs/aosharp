using System;
using System.Runtime.InteropServices;

namespace AOSharp.Bootstrap
{
    public class Connection_t
    {
        [DllImport("Connection.dll", EntryPoint = "?Send@Connection_t@@QAEHIIPBX@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int Send(IntPtr pThis, uint unk, int size, IntPtr pDataBlock);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate int DSend(IntPtr pThis, uint unk, int size, IntPtr pDataBlock);
    }
}
