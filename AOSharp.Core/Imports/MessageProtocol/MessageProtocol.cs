using System;
using System.Runtime.InteropServices;

namespace AOSharp.Core.Imports
{
    public class MessageProtocol
    {
        [DllImport("MessageProtocol.dll", EntryPoint = "?DataBlockToMessage@@YAPAVMessage_t@@IPAX@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr DataBlockToMessage(int size, IntPtr pDataBlock);
    }
}
