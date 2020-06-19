using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core.Imports
{
    internal class Psapi
    {
        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out MODULEINFO lpmodinfo, int cb);

        [StructLayout(LayoutKind.Sequential)]
        public struct MODULEINFO
        {
            public IntPtr lpBaseOfDll;
            public uint SizeOfImage;
            public IntPtr EntryPoint;
        }
    }
}
