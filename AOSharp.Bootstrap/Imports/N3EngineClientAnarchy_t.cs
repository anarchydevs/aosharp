using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Bootstrap.Imports
{
    public class N3EngineClientAnarchy_t
    {
        [DllImport("N3.dll", EntryPoint = "?GetPlayfield@n3EngineClient_t@@SAPAVn3Playfield_t@@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetPlayfield();

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

        [DllImport("Gamecode.dll", EntryPoint = "?PlayfieldInit@n3EngineClientAnarchy_t@@UAEXI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void PlayfieldInit(IntPtr pThis, uint id);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DPlayfieldInit(IntPtr pThis, uint id);

        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_PerformSpecialAction@n3EngineClientAnarchy_t@@QAE_NABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool PerformSpecialAction(IntPtr pThis, IntPtr identity);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate bool DPerformSpecialAction(IntPtr pThis, IntPtr identity);

        //CastNanoSpell
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_CastNanoSpell@n3EngineClientAnarchy_t@@QAEXABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern void CastNanoSpell(IntPtr pThis, Identity* nano, Identity* target);
        public unsafe delegate void DCastNanoSpell(IntPtr pThis, Identity* nanoIdentity, Identity targetIdentity);

        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsPerk@n3EngineClientAnarchy_t@@QBE_NI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool IsPerk(IntPtr pThis, uint id);
    }
}
