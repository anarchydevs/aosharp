using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;

namespace AOSharp.Core
{
    internal class AVChatWindowNode_t
    {
        internal static IntPtr ChatWindowController = Kernal32.GetProcAddress(Kernal32.GetModuleHandle("GUI.dll"), "?s_pcInstance@ChatGUIModule_c@@0PAV1@A") + 0x1C;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate int ChatAppendTextSimpleDelegate(IntPtr pThis, IntPtr message, ChatColor color);
        internal static ChatAppendTextSimpleDelegate AppendText = Marshal.GetDelegateForFunctionPointer<ChatAppendTextSimpleDelegate>(Kernal32.GetModuleHandle("GUI.dll") + 0x9B7CB);
    }
}
