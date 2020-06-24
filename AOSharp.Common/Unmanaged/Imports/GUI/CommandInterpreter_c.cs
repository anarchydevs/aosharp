using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class CommandInterpreter_c
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public unsafe delegate IntPtr DGetCommand(IntPtr pThis, StdString* commandText, bool unk);
        public static DGetCommand GetCommand;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public unsafe delegate byte DProcessChatInput(IntPtr pThis, IntPtr pWindow, StdString* commandText);
        public static unsafe DProcessChatInput ProcessChatInput;
    }
}
