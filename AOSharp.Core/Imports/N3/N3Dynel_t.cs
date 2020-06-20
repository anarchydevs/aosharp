using AOSharp.Common.GameData;
using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core.Imports
{
    public class N3Dynel_t
    {
        [DllImport("N3.dll", EntryPoint = "?SetRelRot@n3Dynel_t@@QAEXABVQuaternion_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe void SetRelRot(IntPtr pThis, Quaternion* rot);
    }
}
