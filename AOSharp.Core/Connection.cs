using System;
using System.Runtime.InteropServices;
using AOSharp.Core.Imports;
using SmokeLounge.AOtomation.Messaging.Messages;

namespace AOSharp.Core
{
    public static class Connection
    {
        public static void Send(N3Message message)
        {
            byte[] packet = PacketFactory.Create(message);

            if (packet == null)
                return;

            Send(packet);
        }

        public unsafe static void Send(byte[] payload)
        {
            IntPtr pClient = Client_t.GetInstanceIfAny();

            if (pClient == IntPtr.Zero)
                return;

            IntPtr pConnection = *(IntPtr*)(pClient + 0x84);

            if (pConnection == IntPtr.Zero)
                return;

            Connection_t.Send(pConnection, 0, payload.Length, Marshal.UnsafeAddrOfPinnedArrayElement(payload, 0));
        }
    }
}
