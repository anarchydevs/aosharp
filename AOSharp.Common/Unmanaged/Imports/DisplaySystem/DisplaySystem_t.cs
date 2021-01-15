using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class DisplaySystem_t
    {
        [DllImport("DisplaySystem.dll", EntryPoint = "?FrameProcess@VisualEnvFX_t@@QAEXMMIMAAVVector3_t@@AAVQuaternion_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int FrameProcess(IntPtr pThis, float unk1, float unk2, int unk3, float unk4, int unk5, int unk6);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate int DFrameProcess(IntPtr pThis, float unk1, float unk2, int unk3, float unk4, int unk5, int unk6);

        [DllImport("DisplaySystem.dll", EntryPoint = "?Commit@DisplaySystem_t@@QAEXXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern void Commit(IntPtr pThis);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DCommit(IntPtr pThis);

    }
}
