using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core.Imports
{
    public class CheckBox_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0CheckBox_c@@QAE@ABVString@@0_N1@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern IntPtr Constructor(IntPtr pThis, IntPtr string1, IntPtr string2, bool defaultValue, bool horizontalSpacer);

        [DllImport("GUI.dll", EntryPoint = "??1CheckBox_c@@UAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern int Deconstructor(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?GetValue@CheckBox_c@@UBE?AVVariant@@XZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern IntPtr GetValue(IntPtr pThis, IntPtr unk);

        public static IntPtr Create(string name, string text, bool defaultValue, bool horizontalSpacer)
        {
            IntPtr pNew = MSVCR100.New(0x158);
            IntPtr pName = StdString.Create(name);
            IntPtr pText = StdString.Create(text);

            IntPtr pView = Constructor(pNew, pName, pText, defaultValue, horizontalSpacer);

            StdString.Dispose(pName);
            StdString.Dispose(pText);

            return pView;
        }
    }
}
