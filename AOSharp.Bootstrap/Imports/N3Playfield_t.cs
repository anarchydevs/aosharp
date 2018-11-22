using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Bootstrap
{
    public class N3Playfield_t
    {
        [DllImport("N3.dll", EntryPoint = "?LineOfSight@n3Playfield_t@@QBE_NABVVector3_t@@0H_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern byte LineOfSight(IntPtr pThis, Vector3* pos1, Vector3* pos2, int zoneCell, bool unknown);

        [DllImport("N3.dll", EntryPoint = "?AddChildDynel@n3Playfield_t@@QAEXPAVn3Dynel_t@@ABVVector3_t@@ABVQuaternion_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void AddChildDynel(IntPtr pThis, IntPtr pDynel, IntPtr pos, IntPtr rot);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DAddChildDynel(IntPtr pThis, IntPtr pDynel, IntPtr pos, IntPtr rot);
    }
}
