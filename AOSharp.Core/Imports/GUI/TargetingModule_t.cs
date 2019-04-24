using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Core.Imports
{
    public class TargetingModule_t
    {
        [DllImport("GUI.dll", EntryPoint = "?GetInstanceIfAny@TargetingModule_t@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstanceIfAny();

        [DllImport("GUI.dll", EntryPoint = "?SetTarget@TargetingModule_t@@CAXABVIdentity_t@@_N@Z", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void SetTarget(Identity* target, bool unk);

        [DllImport("GUI.dll", EntryPoint = "?SelectSelf@TargetingModule_t@@QAEXXZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern void SelectSelf(IntPtr pThis);
    }
}
