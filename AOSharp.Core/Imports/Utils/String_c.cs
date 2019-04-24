using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core.Imports
{
    public class String_c
    {
        [DllImport("Utils.dll", EntryPoint = "??0String@@QAE@PBDH@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern IntPtr Constructor(IntPtr pThis, byte[] str, int len);

        [DllImport("Utils.dll", EntryPoint = "??1String@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern int Deconstructor(IntPtr pThis);
    }
}
