using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    internal class ChatWindowNode_t
    {
        internal static IntPtr ChatWindowController = Kernel32.GetProcAddress(Kernel32.GetModuleHandle("GUI.dll"), "?s_pcInstance@ChatGUIModule_c@@0PAV1@A") + 0x1C;

        //B8 ? ? ? ? E8 ? ? ? ? 83 EC 70 53 56 57 8B F1 68 ? ? ? ? 8D 4D D8 FF 15 ? ? ? ? 33 DB 89 5D FC 39 5D 0C 74 6B 
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate int ChatAppendTextSimpleDelegate(IntPtr pThis, IntPtr message, ChatColor color);
        internal static ChatAppendTextSimpleDelegate AppendText = Marshal.GetDelegateForFunctionPointer<ChatAppendTextSimpleDelegate>(Kernel32.GetModuleHandle("GUI.dll") + FuncOffsets.AppendText); 
    }
}
