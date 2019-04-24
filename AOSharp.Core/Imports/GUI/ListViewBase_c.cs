using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core.Imports
{
    public class ListViewBase_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0ListViewBase_c@@QAE@ABVRect@@ABVString@@HII@Z", CallingConvention = CallingConvention.ThisCall)]
        internal unsafe static extern IntPtr Constructor(IntPtr pThis, Rect* rect, IntPtr pName, int unk1, int unk2, int unk3);

        [DllImport("GUI.dll", EntryPoint = "??1ListViewBase_c@@UAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern int Deconstructor(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?AppendItem@ListViewBase_c@@QAEXPAVListViewBaseItem_c@@@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern void AppendItem(IntPtr pThis, IntPtr pItem);

        public unsafe static IntPtr Create(Rect rect, string name, int unk1, int unk2, int unk3)
        {
            IntPtr pNew = MSVCR100.New(0x190);
            IntPtr pName = StdString.Create(name);

            IntPtr pView = Constructor(pNew, &rect, pName, unk1, unk2, unk3);

            StdString.Dispose(pName);

            return pView;
        }
    }
}
