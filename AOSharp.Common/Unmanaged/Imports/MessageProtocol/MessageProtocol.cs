using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class MessageProtocol
    {
        [DllImport("MessageProtocol.dll", EntryPoint = "?DataBlockToMessage@@YAPAVMessage_t@@IPAX@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DataBlockToMessage(int size, IntPtr pDataBlock);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate int DDataBlockToMessage(int size, IntPtr pDataBlock);
    }
}
