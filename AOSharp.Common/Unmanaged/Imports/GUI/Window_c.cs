using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class Window_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0Window@@QAE@ABVRect@@ABVString@@1W4WindowStyle_e@@I@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern unsafe IntPtr Constructor(IntPtr pThis, Rect* rect, IntPtr string1, IntPtr string2, WindowStyle windowStyle, WindowFlags flags);

        [DllImport("GUI.dll", EntryPoint = "??1Window@@UAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int Deconstructor(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?Show@Window@@QAEX_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void Show(IntPtr pThis, bool visible);

        [DllImport("GUI.dll", EntryPoint = "?MoveToCenter@Window@@QAEXXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern void MoveToCenter(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?GetTabView@Window@@QBEPAVTabView@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetTabView(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?GetBounds@Window@@QBE?AVRect@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetBounds(IntPtr pThis, IntPtr pRect);

        [DllImport("GUI.dll", EntryPoint = "?AppendTab@Window@@QAEHABVString@@PAVView@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr AppendTab(IntPtr pThis, IntPtr pName, IntPtr pView);

        [DllImport("GUI.dll", EntryPoint = "?AddChild@Window@@QAEXPAVView@@_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr AppendChild(IntPtr pThis, IntPtr pView, bool unk);

        [DllImport("GUI.dll", EntryPoint = "?SetTitle@Window@@QAEXABVString@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void SetTitle(IntPtr pThis, IntPtr pTitle);

        public static unsafe IntPtr Create(Rect rect, string string1, string string2, WindowStyle style, WindowFlags flags)
        {
            IntPtr pNew = MSVCR100.New(0xAC);
            IntPtr pString1 = StdString.Create(string1);
            IntPtr pString2 = StdString.Create(string2);

            IntPtr pWindow = Constructor(pNew, &rect, pString1, pString2, style, flags);

            StdString.Dispose(pString1);
            StdString.Dispose(pString2);

            return pWindow;
        }
    }
}
