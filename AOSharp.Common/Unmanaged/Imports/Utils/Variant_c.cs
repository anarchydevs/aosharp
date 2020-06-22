using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    internal class Variant_c
    {
        [DllImport("Utils.dll", EntryPoint = "?AsBool@Variant@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern bool AsBool(IntPtr pThis);

        [DllImport("Utils.dll", EntryPoint = "?AsInt32@Variant@@QBEJXZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern int AsInt32(IntPtr pThis);

        [DllImport("Utils.dll", EntryPoint = "??0Variant@@QAE@H@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern IntPtr Constructor(IntPtr pThis, int value);

        [DllImport("Utils.dll", EntryPoint = "??1Variant@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern int Deconstructor(IntPtr pThis);
    }
}
