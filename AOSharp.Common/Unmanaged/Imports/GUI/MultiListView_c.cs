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

        [DllImport("GUI.dll", EntryPoint = "?SetLayoutMode@MultiListView_c@@QAEXW4LayoutMode_e@1@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void SetLayoutMode(IntPtr pThis, int mode);

        public static IntPtr Create(Rect rect, int flags, int unk1, int unk2)
        {
            return Constructor(MSVCR100.New(0x2D8), ref rect, flags, unk1, unk2);
        }
    }
}
