using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core.Imports
{
    public class Client_t
    {
        [DllImport("Interfaces.dll", EntryPoint = "?GetInstanceIfAny@Client_t@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetInstanceIfAny();
    }
}
