using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class MultiListView_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0MultiListView_c@@QAE@ABVRect@@III@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern IntPtr Constructor(IntPtr pThis, ref Rect rect, int flags, int unk1, int unk2);

        [DllImport("GUI.dll", EntryPoint = "??1MultiListView_c@@UAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int Deconstructor(IntPtr pThis);


        [DllImport("GUI.dll", EntryPoint = "?AddColumn@MultiListView_c@@QAEXHABVString@@MI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void AddColumn(IntPtr pThis, int idx, IntPtr pStr, float width, int unk);

        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("GUI.dll", EntryPoint = "?AddItem@MultiListView_c@@QAE_NABVIPoint@@PAVMultiListViewItem_c@@_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool AddItem(IntPtr pThis, ref Vector2 slot, IntPtr listViewItem, bool unk);

        [DllImport("GUI.dll", EntryPoint = "?SetLayoutMode@MultiListView_c@@QAEXW4LayoutMode_e@1@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void SetLayoutMode(IntPtr pThis, int mode);

        [DllImport("GUI.dll", EntryPoint = "?SetBackgroundBitmap@MultiListView_c@@QAEXH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void SetBackgroundBitmap(IntPtr pThis, int gfxId);

        [DllImport("GUI.dll", EntryPoint = "?SetGridIconSpacing@MultiListView_c@@QAEXABVPoint@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void SetGridIconSpacing(IntPtr pThis, ref Vector2 spacing);

        [DllImport("GUI.dll", EntryPoint = "?SetGridLabelsOnTop@MultiListView_c@@QAEX_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void SetGridLabelsOnTop(IntPtr pThis, bool spacing);

        [DllImport("GUI.dll", EntryPoint = "?SetViewCellCounts@MultiListView_c@@QAEXABVIPoint@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void SetViewCellCounts(IntPtr pThis, ref Vector2 unk1, ref Vector2 unk2);

        [DllImport("GUI.dll", EntryPoint = "?GetFirstFreePos@MultiListView_c@@QBE?AVIPoint@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetFirstFreePos(IntPtr pThis, ref Vector2 pos);

        [DllImport("GUI.dll", EntryPoint = "?GetSelectedItem@MultiListView_c@@QBEPAVMultiListViewItem_c@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetSelectedItem(IntPtr pThis);

        public static IntPtr Create(Rect rect, int flags, int unk1, int unk2)
        {
            return Constructor(MSVCR100.New(0x2D8), ref rect, flags, unk1, unk2);
        }
    }
}
