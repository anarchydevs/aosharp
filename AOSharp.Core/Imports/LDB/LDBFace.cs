using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    public class LDBFace
    {
        [DllImport("ldb.dll", EntryPoint = "?GetText@LDBface@@SA?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@II@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern StdString GetText(int type, int instance);
    }
}
