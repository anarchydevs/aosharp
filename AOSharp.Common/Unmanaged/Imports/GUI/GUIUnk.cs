using SmokeLounge.AOtomation.Messaging.Messages;
using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class GUIUnk
    {
        [return: MarshalAs(UnmanagedType.U1)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate bool LoadViewFromXmlDelegate(out IntPtr pView, IntPtr pPathStr, IntPtr pUnkStr);
        public static LoadViewFromXmlDelegate LoadViewFromXml;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public unsafe delegate int ProcessChatMessageDelegate(IntPtr pThis, IntPtr unk1, IntPtr unk2, IntPtr unk3);
        public static ProcessChatMessageDelegate ProcessChatMessage;

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        public struct ChatMessage
        {
            [FieldOffset(0x0)]
            public ChatPacketType Type;

            [FieldOffset(0x4)]
            public short Length;

            [FieldOffset(0x8)]
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            public byte[] Payload;
        }
    }
}
