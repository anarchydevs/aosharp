using System;
using System.Runtime.InteropServices;

namespace AOSharp.Bootstrap
{
    public class RoomSurface_t
    {
        [DllImport("N3.dll", EntryPoint = "?GetLineIntersection@n3RoomSurface_t@@UBE_NABVVector3_t@@0AAV2@1_NPAVLocalitySource_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern byte GetLineIntersection(IntPtr pThis, IntPtr pos1, IntPtr pos2, IntPtr retPos1, IntPtr retPos2, byte unk, IntPtr plocalitySource);
    }
}
