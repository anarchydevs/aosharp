using System;
using System.Runtime.InteropServices;

namespace AOSharp.Bootstrap
{
    public class Client_t
    {
        [DllImport("Interfaces.dll", EntryPoint = "?GetInstanceIfAny@Client_t@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstanceIfAny();
    }
}
