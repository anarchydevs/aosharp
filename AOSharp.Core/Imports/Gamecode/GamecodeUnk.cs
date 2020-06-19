using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    internal class GamecodeUnk
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal unsafe delegate int AppendSystemTextDelegate(int unk, [MarshalAs(UnmanagedType.LPStr)] string message, ChatColor color);
        internal static AppendSystemTextDelegate AppendSystemText;
    }
}
