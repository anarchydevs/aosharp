using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core
{
    public class N3Room_t
    {
        [DllImport("N3.dll", EntryPoint = "?GetRoomRect@n3Room_t@@QBEXAAM000@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void GetRoomRect(IntPtr pThis, out float x, out float x2, out float y, out float y2);
    }
}
