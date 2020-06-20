using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core.Imports
{
    public class ViewSelector_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0ViewSelector_c@@QAE@ABVRect@@VString@@HII@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern unsafe IntPtr Constructor(IntPtr pThis, Rect* rect, IntPtr pName, int garbage1, int garbage2, int garbage3, int garbage4, int garbage5, int garbage6, int unk1, int unk2, int unk3);

        [DllImport("GUI.dll", EntryPoint = "??1ViewSelector_c@@UAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern int Deconstructor(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?SetListView@ViewSelector_c@@QAEXPAVListViewBase_c@@@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern void SetListView(IntPtr pThis, IntPtr pListViewBase);

        [DllImport("GUI.dll", EntryPoint = "?GetListView@ViewSelector_c@@QBEPAVListViewBase_c@@XZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern IntPtr GetListView(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?AppendView@ViewSelector_c@@QAEXPAVView@@@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern void AppendView(IntPtr pThis, IntPtr pView);

        public static unsafe IntPtr Create(Rect rect, string name, int unk1, int unk2, int unk3)
        {
            IntPtr pNew = MSVCR100.New(0x178);
            IntPtr pName = StdString.Create(name);

            IntPtr pView = Constructor(pNew, &rect, pName, 0, 0, 0, 0, 0, 0, unk1, unk2, unk3);

            StdString.Dispose(pName);

            return pView;
        }
    }
}
