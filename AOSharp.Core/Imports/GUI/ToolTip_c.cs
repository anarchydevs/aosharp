using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core.Imports
{
    public class ToolTip_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0ToolTip_c@@QAE@ABVString@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        internal unsafe static extern IntPtr Constructor(IntPtr pThis, IntPtr string1, IntPtr string2);

        public unsafe static IntPtr Create(string string1, string string2)
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
