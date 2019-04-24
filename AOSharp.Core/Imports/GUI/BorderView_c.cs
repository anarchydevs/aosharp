using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core.Imports
{
    public class BorderView_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0BorderView_c@@QAE@ABVRect@@ABV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@II@Z", CallingConvention = CallingConvention.ThisCall)]
        internal unsafe static extern IntPtr Constructor(IntPtr pThis, Rect* rect, IntPtr pName, int unk1, int unk2);

        [DllImport("GUI.dll", EntryPoint = "??1BorderView_c@@UAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern int Deconstructor(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?SetClient@BorderView_c@@QAEXPAVView@@MMMM@Z", CallingConvention = CallingConvention.ThisCall)]
        internal unsafe static extern void SetClient(IntPtr pThis, IntPtr pView, float x1, float y1, float x2, float y2);

        public unsafe static IntPtr Create(Rect rect, string name, int unk1, int unk2)
        {
            IntPtr pNew = MSVCR100.New(0x1A8);
            IntPtr pName = StdString.Create(name);

            IntPtr pView = Constructor(pNew, &rect, pName, unk1, unk2);

            StdString.Dispose(pName);

            return pView;
        }
    }
}
