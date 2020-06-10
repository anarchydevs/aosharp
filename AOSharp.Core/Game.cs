using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using AOSharp.Core.UI;
using AOSharp.Core.GameData;
using SmokeLounge.AOtomation.Messaging.Messages;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using AOSharp.Core.Combat;
using AOSharp.Core.Inventory;

namespace AOSharp.Core
{
    public static class Game
    {
        public static EventHandler<float> OnUpdate;
        public static EventHandler<float> OnLateUpdate;
        public static EventHandler<N3Message> N3MessageReceived;
        public static EventHandler TeleportStarted;
        public static EventHandler TeleportEnded;
        public static EventHandler TeleportFailed; 
        public static EventHandler<uint> PlayfieldInit;

        private static Dictionary<N3MessageType, Action<N3Message>> n3MsgCallbacks = new Dictionary<N3MessageType, Action<N3Message>>
        {
            { N3MessageType.KnubotAnswerList, NpcDialog.OnKnubotAnswerList },
            { N3MessageType.CharacterAction, OnCharacterAction },
            { N3MessageType.TemplateAction, OnTemplateAction }
        };

        public static void SetMovement(MovementAction action)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.MovementChanged(pEngine, action, 0, 0, true);
        }

        private static void OnUpdateInternal (float deltaTime)
        {
            UIController.UpdateViews();

            OnUpdate?.Invoke(null, deltaTime);
        }

        private static void OnLateUpdateInternal(float deltaTime)
        {
            if (CombatHandler.Instance != null)
                CombatHandler.Instance.Update(deltaTime);

            Perk.OnUpdate(deltaTime);

            OnLateUpdate?.Invoke(null, deltaTime);
        }

        private static void OnMessage(byte[] datablock)
        {
            Message msg = PacketFactory.Disassemble(datablock);

            if (msg == null)
                return;

            if (msg.Header.PacketType == PacketType.N3Message)
                OnN3Message((N3Message)msg.Body);
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

        private static void OnTeleportStarted()
        {
            TeleportStarted?.Invoke(null, EventArgs.Empty);
        }

        private static void OnTeleportEnded()
        {
            TeleportEnded?.Invoke(null, EventArgs.Empty);
        }

        private static void OnTeleportFailed()
        {
            TeleportFailed?.Invoke(null, EventArgs.Empty);
        }

        private static void OnPlayfieldInit(uint id)
        {
            PlayfieldInit?.Invoke(null, id);
        }
    }
}
