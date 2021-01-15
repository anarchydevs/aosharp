using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class PowerBarView_c
    {
        [DllImport("GUI.dll", EntryPoint = "?SetValue@PowerbarView_c@@UAEXABVVariant@@_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void SetValue(IntPtr pThis, IntPtr pVariant, bool unk);

        [DllImport("GUI.dll", EntryPoint = "?GetValue@PowerbarView_c@@UBE?AVVariant@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetValue(IntPtr pThis, IntPtr pVariant);
    }
}
