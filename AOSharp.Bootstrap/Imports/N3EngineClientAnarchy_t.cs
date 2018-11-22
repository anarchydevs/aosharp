using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Bootstrap
{
    public class N3EngineClientAnarchy_t
    {
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_MovementChanged@n3EngineClientAnarchy_t@@QAEXW4MovementAction_e@Movement_n@@MM_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void MovementChanged(IntPtr pThis, MovementAction action, float unk1, float unk2, bool unk3);

        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetContainerInventoryList@n3EngineClientAnarchy_t@@QBEPBV?$list@VInventoryEntry_t@@V?$allocator@VInventoryEntry_t@@@std@@@std@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetContainerInventoryList(IntPtr pThis, IntPtr identity);

        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_TradeskillCombine@n3EngineClientAnarchy_t@@QBEXABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr TradeskillCombine(IntPtr pThis, IntPtr source, IntPtr destination);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate IntPtr DTradeskillCombine(IntPtr pThis, IntPtr source, IntPtr destination);

        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_TextCommand@n3EngineClientAnarchy_t@@QAE_NHPBDABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr TextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate IntPtr DTextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);
    }
}
