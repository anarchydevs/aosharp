using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core
{
    public class N3EngineClient_t
    {
        [DllImport("N3.dll", EntryPoint = "?GetPlayfield@n3EngineClient_t@@SAPAVn3Playfield_t@@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetPlayfield();
    }
}
