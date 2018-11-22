using System;
using System.Runtime.InteropServices;

namespace AOSharp.Bootstrap
{
    public class MessageProtocol
    {
        [DllImport("MessageProtocol.dll", EntryPoint = "?DataBlockToMessage@@YAPAVMessage_t@@IPAX@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr DataBlockToMessage(int size, IntPtr pDataBlock);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate IntPtr DDataBlockToMessage(int size, IntPtr pDataBlock);
    }
}
