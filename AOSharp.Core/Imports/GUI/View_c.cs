using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core.Imports
{
    public class View_c
    {
        [DllImport("GUI.dll", EntryPoint = "??0View@@QAE@ABVRect@@ABVString@@II@Z", CallingConvention = CallingConvention.ThisCall)]
        internal unsafe static extern IntPtr Constructor(IntPtr pThis, Rect* rect, IntPtr pName, int unk1, int unk2);

        [DllImport("GUI.dll", EntryPoint = "??1View@@UAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        internal static extern int Deconstructor(IntPtr pThis);

        [DllImport("GUI.dll", EntryPoint = "?AddChild@View@@UAEXPAV1@_N@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern void AddChild(IntPtr pThis, IntPtr pView, bool unk);

        [DllImport("GUI.dll", EntryPoint = "?SetBorders@View@@QAEXMMMM@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern void SetBorders(IntPtr pThis, float minX, float minY, float maxX, float maxY);

        [DllImport("GUI.dll", EntryPoint = "?LimitMaxSize@View@@QAEXABVPoint@@@Z", CallingConvention = CallingConvention.ThisCall)]
        internal unsafe static extern void LimitMaxSize(IntPtr pThis, Vector2* maxSize);

        [DllImport("GUI.dll", EntryPoint = "?SetFrame@View@@UAEXABVRect@@_N@Z", CallingConvention = CallingConvention.ThisCall)]
        internal unsafe static extern void SetFrame(IntPtr pThis, Rect* rect, bool unk);

        [DllImport("GUI.dll", EntryPoint = "?Show@View@@QAEX_N0@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern void Show(IntPtr pThis, bool visible, bool unk);

        [DllImport("GUI.dll", EntryPoint = "?SetLayoutNode@View@@QAEXPAVLayoutNode@@@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern void SetLayoutNode(IntPtr pThis, IntPtr pLayoutNode);


        public unsafe static IntPtr Create(Rect rect, string name, int unk1, int unk2)
        {
            IntPtr pNew = MSVCR100.New(0x128);
            IntPtr pName = StdString.Create(name);

            IntPtr pView = Constructor(pNew, &rect, pName, unk1, unk2);

            StdString.Dispose(pName);

            return pView;
        }
    }
}
