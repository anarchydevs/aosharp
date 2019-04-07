using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.Serialization;
using SmokeLounge.AOtomation.Messaging.Messages;

namespace AOSharp.Core
{
    public class PacketFactory
    {
        private static MessageSerializer _serializer = new MessageSerializer();

        public unsafe static byte[] Create(MessageBody messageBody)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return null;

            int localDynelInstance = N3EngineClient_t.GetClientInst(pEngine);

            var message = new Message
            {
                Body = messageBody,
                Header = new Header
                        {
                            MessageId = BitConverter.ToUInt16(new byte[] { 0xFF, 0xFF }, 0),
                            PacketType = messageBody.PacketType,
                            Unknown = 0x0001,
                            Sender = localDynelInstance,
                            Receiver = 0x02
                        }
            };

            using (MemoryStream stream = new MemoryStream())
            {
                _serializer.Serialize(stream, message);
                return stream.ToArray();
            }
        }
    }
}
