using AOSharp.Common.Unmanaged.DataTypes;
using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class ChatGUIModule_t
    {
        [DllImport("GUI.dll", EntryPoint = "?GetInstanceIfAny@ChatGUIModule_c@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstance();

        [DllImport("GUI.dll", EntryPoint = "?ExpandChatTextArgs@ChatGUIModule_c@@SA?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@ABV23@@Z", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern StdString* ExpandChatTextArgs(IntPtr pOut, IntPtr pMsg);

    }
}
