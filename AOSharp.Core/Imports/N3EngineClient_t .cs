using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core
{
    public class N3EngineClient_t
    {
        [DllImport("N3.dll", EntryPoint = "?GetPlayfield@n3EngineClient_t@@SAPAVn3Playfield_t@@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetPlayfield();

        [DllImport("N3.dll", EntryPoint = "?GetClientControlDynel@n3EngineClient_t@@QBEPAVn3VisualDynel_t@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetClientControlDynel(IntPtr pThis);
    }
}
