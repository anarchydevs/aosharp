using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class Connection_t
    {
        [DllImport("Connection.dll", EntryPoint = "?Send@Connection_t@@QAEHIIPBX@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int Send(IntPtr pThis, uint unk, int size, IntPtr pDataBlock);
    }
}
