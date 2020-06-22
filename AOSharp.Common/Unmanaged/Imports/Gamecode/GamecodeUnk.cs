using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class GamecodeUnk
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate int AppendSystemTextDelegate(int unk, [MarshalAs(UnmanagedType.LPStr)] string message, ChatColor color);
        public static AppendSystemTextDelegate AppendSystemText;
    }
}
