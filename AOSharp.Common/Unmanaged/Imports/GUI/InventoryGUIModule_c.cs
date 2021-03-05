using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class InventoryGUIModule_c
    {
        [DllImport("GUI.dll", EntryPoint = "?GetInstanceIfAny@InventoryGUIModule_c@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstance();

        [DllImport("GUI.dll", EntryPoint = "?GetBackpackName@InventoryGUIModule_c@@QAE?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@ABVIdentity_t@@_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetBackpackName(IntPtr pThis, IntPtr pStr, ref Identity identity, bool unk);
    }
}
