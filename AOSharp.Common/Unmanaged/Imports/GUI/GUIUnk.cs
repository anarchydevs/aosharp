using SmokeLounge.AOtomation.Messaging.Messages;
using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class GUIUnk
    {
        [return: MarshalAs(UnmanagedType.U1)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate bool LoadViewFromXmlDelegate(out IntPtr pView, IntPtr pPathStr, IntPtr pUnkStr);
        public static LoadViewFromXmlDelegate LoadViewFromXml;

    }
}
