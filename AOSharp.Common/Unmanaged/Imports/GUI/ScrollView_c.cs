using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class ScrollView_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0ScrollView_c@@QAE@ABVRect@@ABVString@@W4ScrollBarMode_e@0@2HII@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern unsafe IntPtr Constructor(IntPtr pThis, Rect* rect, IntPtr pName, int scrollBarModeH, int scrollBarModeV, int unk1, int unk2, int unk3);

        [DllImport("GUI.dll", EntryPoint = "??1ScrollView_c@@UAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int Deconstructor(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?SetClient@ScrollView_c@@QAEXPAVView@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void SetClient(IntPtr pThis, IntPtr pView1, IntPtr pView2);

        public static unsafe IntPtr Create(Rect rect, string name, int scrollBarModeH, int scrollBarModeV, int unk1, int unk2, int unk3)
        {
            IntPtr pNew = MSVCR100.New(0x1B0);
            IntPtr pName = StdString.Create(name);

            IntPtr pView = Constructor(pNew, &rect, pName, scrollBarModeH, scrollBarModeV, unk1, unk2, unk3);

            StdString.Dispose(pName);

            return pView;
        }
    }
}
