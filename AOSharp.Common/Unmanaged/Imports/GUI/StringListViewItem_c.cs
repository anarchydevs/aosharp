using System;
using System.Runtime.InteropServices;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class StringListViewItem_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0StringListViewItem_c@@QAE@ABVVariant@@ABVString@@HH@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern IntPtr Constructor(IntPtr pThis, IntPtr pVariant, IntPtr pName, int unk1, int unk2);

        [DllImport("GUI.dll", EntryPoint = "??1StringListViewItem_c@@UAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int Deconstructor(IntPtr pThis);

        public static IntPtr Create(Variant variant, string name, int unk1, int unk2)
        {
            IntPtr pNew = MSVCR100.New(0x98);
            IntPtr pName = StdString.Create(name);

            IntPtr pView = Constructor(pNew, variant.Pointer, pName, unk1, unk2);

            StdString.Dispose(pName);

            return pView;
        }
    }
}
