using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Bootstrap
{
    public class N3EngineClientAnarchy_t
    {
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_TextCommand@n3EngineClientAnarchy_t@@QAE_NHPBDABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr TextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate IntPtr DTextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);

        [DllImport("Gamecode.dll", EntryPoint = "?RunEngine@n3EngineClientAnarchy_t@@UAEXM@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void RunEngine(IntPtr pThis, float unk);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DRunEngine(IntPtr pThis, float unk);


        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_SendInPlayMessage@n3EngineClientAnarchy_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool SendInPlayMessage(IntPtr pThis);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate bool DSendInPlayMessage(IntPtr pThis);
    }
}
