using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class MSVCR100
    {
        [DllImport("MSVCR100.dll", EntryPoint = "??2@YAPAXI@Z", CharSet = CharSet.Auto)]
        public static extern IntPtr New(int size);
    }
}
