using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Core.Imports;
using AOSharp.Core.Inventory;
using SmokeLounge.AOtomation.Messaging.Messages;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core
{
    public static class Network
    {
        public static EventHandler<N3Message> N3MessageReceived;

        private static Queue<Message> _messageQueue = new Queue<Message>();

        private static Dictionary<N3MessageType, Action<N3Message>> n3MsgCallbacks = new Dictionary<N3MessageType, Action<N3Message>>
        {
            { N3MessageType.KnubotAnswerList, NpcDialog.OnKnubotAnswerList },
            { N3MessageType.CharacterAction, OnCharacterAction },
            { N3MessageType.TemplateAction, OnTemplateAction }
        };

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

        internal static void Update()
        {
            while (_messageQueue.Count > 0)
            {
                Message msg = _messageQueue.Dequeue();

                if (msg.Header.PacketType == PacketType.N3Message)
                    OnN3Message((N3Message)msg.Body);
            }
        }

        private static void OnMessage(byte[] datablock)
        {
            Message msg = PacketFactory.Disassemble(datablock);

            if (msg == null)
                return;

            _messageQueue.Enqueue(msg);
        }

        private static void OnN3Message(N3Message n3Msg)
        {
            if (n3MsgCallbacks.ContainsKey(n3Msg.N3MessageType))
                n3MsgCallbacks[n3Msg.N3MessageType].Invoke(n3Msg);

            N3MessageReceived?.Invoke(null, n3Msg);
        }

        private static void OnCharacterAction(N3Message n3Msg)
        {
            CharacterActionMessage charActionMessage = (CharacterActionMessage)n3Msg;

            //Chat.WriteLine($"OnCharacterAction {charActionMessage.Action}\t{charActionMessage.Identity}\t{charActionMessage.Target}\t{charActionMessage.Parameter1}\t{charActionMessage.Parameter2}\t{charActionMessage.Unknown1}\t{charActionMessage.Unknown2}");

            switch (charActionMessage.Action)
            {
                case CharacterActionType.LeaveTeam:
                    Team.OnMemberLeft(charActionMessage.Target);
                    break;
                case CharacterActionType.QueuePerk:
                    Perk.OnPerkQueued();
                    break;
                //case CharacterActionType.TeamKick:
                //    Team.OnMemberLeft(charActionMessage.Target);
                //    break;
            }
        }

        private static void OnTemplateAction(N3Message n3Msg)
        {
            TemplateActionMessage templateActionMessage = (TemplateActionMessage)n3Msg;

            switch (templateActionMessage.Unknown2)
            {
                case 3:
                    Item.OnItemUsed(templateActionMessage.ItemLowId, templateActionMessage.ItemHighId, templateActionMessage.Quality, templateActionMessage.Identity);
                    break;
                case 32:
                    Perk.OnPerkFinished(templateActionMessage.ItemLowId, templateActionMessage.ItemHighId, templateActionMessage.Identity);
                    break;
            }
        }
    }
}
