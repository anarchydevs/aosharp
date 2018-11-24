using System;
using System.Runtime.InteropServices;

namespace AOSharp.Bootstrap
{
    internal class Kernal32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
