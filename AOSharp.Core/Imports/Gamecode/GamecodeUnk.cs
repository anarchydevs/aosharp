using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    internal class GamecodeUnk
    {
        //B8 ? ? ? ? E8 ? ? ? ? 83 EC 28 53 56 FF 15 ? ? ? ? FF 75 0C 33 DB 8D 4D CC 8B F0 C7 45 ? ? ? ? ? 89 5D DC 88 5D CC E8 ? ? ? ? 8D 86 ? ? ? ? 8B 08 89 5D FC 3B CB 74 75 89 45 EC A1 ? ? ? ? 89 4D E8 8B 08 89 4D F0 8D 4D E8 89 08 C6 45 FC 01 8B 4D E8 8B 41 14 89 45 E8 8B 41 0C 2B C3 74 32 48 74 25 48 74 14 48 75 2E FF 75 10 8B 01 8D 55 CC 52 FF 75 08 FF 50 04 EB 1D 8B 01 8D 55 CC 52 FF 75 08 FF 50 04 EB 0F FF 75 08 
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal unsafe delegate int AppendSystemTextDelegate(int unk, [MarshalAs(UnmanagedType.LPStr)] string message, ChatColor color);
        internal static AppendSystemTextDelegate AppendSystemText = Marshal.GetDelegateForFunctionPointer<AppendSystemTextDelegate>(Kernel32.GetModuleHandle("Gamecode.dll") + FuncOffsets.AppendSystemText);
    }
}
