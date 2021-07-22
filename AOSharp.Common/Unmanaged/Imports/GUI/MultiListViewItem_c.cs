using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class MultiListViewItem_c
    {
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("GUI.dll", EntryPoint = "?IsSelected@MultiListViewItem_c@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool IsSelected(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?GetID@MultiListViewItem_c@@QBEABVVariant@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetID(IntPtr pThis);
    }
}
