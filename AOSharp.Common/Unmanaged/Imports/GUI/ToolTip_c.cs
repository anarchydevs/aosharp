using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class ToolTip_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0ToolTip_c@@QAE@ABVString@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern IntPtr Constructor(IntPtr pThis, IntPtr string1, IntPtr string2);

        public static IntPtr Create(string string1, string string2)
        {
            IntPtr pNew = MSVCR100.New(0x74);
            IntPtr pString1 = StdString.Create(string1);
            IntPtr pString2 = StdString.Create(string2);

            IntPtr pToolTip = Constructor(pNew, pString1, pString2);

            StdString.Dispose(pString1);
            StdString.Dispose(pString2);

            return pToolTip;
        }
    }
}
