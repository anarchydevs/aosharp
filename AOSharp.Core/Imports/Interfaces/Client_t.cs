using AOSharp.Common.GameData;
using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core.Imports
{
    public class Client_t
    {
        [DllImport("Interfaces.dll", EntryPoint = "?GetInstanceIfAny@Client_t@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstanceIfAny();

        [DllImport("Interfaces.dll", EntryPoint = "?SendVicinityMessage@Client_t@@QAEXPADIABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe void SendVicinityMessage(IntPtr pThis, [MarshalAs(UnmanagedType.LPStr)] string message, int length, Identity* unk);
    }
}
