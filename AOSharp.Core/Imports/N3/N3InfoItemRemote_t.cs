using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    public class N3InfoItemRemote_t
    {
        [DllImport("N3.dll", EntryPoint = "?KeyToString@n3InfoItemRemote_t@@SAABV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@J@Z", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern StdString* KeyToString(int key);
    }
}
