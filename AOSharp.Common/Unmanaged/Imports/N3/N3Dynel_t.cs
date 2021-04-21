using AOSharp.Common.GameData;
using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class N3Dynel_t
    {
        [DllImport("N3.dll", EntryPoint = "?SetRelRot@n3Dynel_t@@QAEXABVQuaternion_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe void SetRelRot(IntPtr pThis, Quaternion* rot);

        [DllImport("N3.dll", EntryPoint = "?GetZone@n3Dynel_t@@QBEPAVn3Zone_t@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetZone(IntPtr pThis);
    }
}
