using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core.Imports
{
    public class ListViewBaseItem_c
    {
        [DllImport("GUI.dll", EntryPoint = "?AppendChild@ListViewBaseItem_c@@QAEXPAV1@@Z", CallingConvention = CallingConvention.ThisCall)]
        internal static extern void AppendChild(IntPtr pThis, IntPtr pItem);
    }
}
