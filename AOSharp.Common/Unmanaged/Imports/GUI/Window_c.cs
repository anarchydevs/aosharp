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

        [DllImport("GUI.dll", EntryPoint = "?Close@Window@@QAEXXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern void Close(IntPtr pThis);

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
        
        [DllImport("GUI.dll", EntryPoint = "?FindWindowName@Window@@SAPAV1@PBD@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr FindWindowName([MarshalAs(UnmanagedType.LPStr)] string windowName);

        [return: MarshalAs(UnmanagedType.U1)]

        [DllImport("GUI.dll", EntryPoint = "?IsVisible@Window@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool IsVisible(IntPtr pThis);

        public static unsafe IntPtr Create(Rect rect, string string1, string string2, WindowStyle style, WindowFlags flags)
        {
            StdString str1 = StdString.Create(string1);
            StdString str2 = StdString.Create(string2);

            IntPtr pWindow = Constructor(MSVCR100.New(0xAC), &rect, str1.Pointer, str2.Pointer, style, flags);

            return pWindow;
        }
    }
}
