using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core.Imports
{
    internal class MSVCR100
    {
        [DllImport("MSVCR100.dll", EntryPoint = "??2@YAPAXI@Z", CharSet = CharSet.Auto)]
        internal static extern IntPtr New(int size);
    }
}
