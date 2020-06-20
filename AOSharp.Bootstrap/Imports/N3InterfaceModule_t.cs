using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Bootstrap.Imports
{
    public class N3InterfaceModule_t
    {
        [DllImport("Interfaces.dll", EntryPoint = "?GetInstance@N3InterfaceModule_t@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstance();

        [DllImport("Interfaces.dll", EntryPoint = "?N3Msg_CastNanoSpell@N3InterfaceModule_t@@QBEXABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern void CastNanoSpell(IntPtr pThis, Identity* nano, Identity target);

        public unsafe delegate void DCastNanoSpell(IntPtr pThis, Identity* nanoIdentity, Identity targetIdentity);
    }
}
