using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core.Imports
{
    public class TilemapSurface_t
    {
        [DllImport("N3.dll", EntryPoint = "?GetLineIntersection@n3TilemapSurface_t@@UBE_NABVVector3_t@@0AAV2@1_NPAVLocalitySource_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern byte GetLineIntersection(IntPtr pThis, IntPtr pos1, IntPtr pos2, IntPtr retPos1, IntPtr retPos2, byte unk, IntPtr plocalitySource);
    }
}
