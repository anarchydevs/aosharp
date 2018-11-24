using System;
using System.Runtime.InteropServices;

namespace AOSharp.Bootstrap
{
    internal class ChatUnnamed
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate int ChatAppendTextSimpleDelegate(IntPtr pThis, byte* message, int unk);
        internal static ChatAppendTextSimpleDelegate ChatAppendTextSimple = Marshal.GetDelegateForFunctionPointer<ChatAppendTextSimpleDelegate>(Kernal32.GetModuleHandle("GUI.dll") + 0x9B7CB);
    }
}
