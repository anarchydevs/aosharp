using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core
{
    public class N3Engine_t
    {
        [DllImport("N3.dll", EntryPoint = "?GetInstance@n3Engine_t@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstance();
    }
}
