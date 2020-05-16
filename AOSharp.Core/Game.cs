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

namespace AOSharp.Core
{
    public static class Game
    {
        public static EventHandler<float> OnUpdate;
        public static EventHandler<N3Message> N3MessageReceived;
        public static EventHandler OnTeleportStarted;
        public static EventHandler OnTeleportEnded;
        public static EventHandler OnTeleportFailed; 
        public static EventHandler<uint> PlayfieldInit;

        private static Dictionary<N3MessageType, Action<N3Message>> n3MsgCallbacks = new Dictionary<N3MessageType, Action<N3Message>>
        {
            { N3MessageType.KnubotAnswerList, NpcDialog.OnKnubotAnswerList }
        };

        public static void SetMovement(MovementAction action)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.MovementChanged(pEngine, action, 0, 0, true);
        }

        private static void OnUpdateInternal(float deltaTime)
        {
            UIController.UpdateViews();

            OnUpdate?.Invoke(null, deltaTime);
        }

        private static void OnMessageInternal(byte[] datablock)
        {
            Message msg = PacketFactory.Disassemble(datablock);

            if (msg.Header.PacketType == PacketType.N3Message)
                OnN3MessageInternal((N3Message)msg.Body);
        }

        private static void OnN3MessageInternal(N3Message n3Msg)
        {
            if (n3MsgCallbacks.ContainsKey(n3Msg.N3MessageType))
                n3MsgCallbacks[n3Msg.N3MessageType].Invoke(n3Msg);

            N3MessageReceived?.Invoke(null, n3Msg);
        }

        private static void OnTeleportStartedInternal()
        {
            OnTeleportStarted?.Invoke(null, EventArgs.Empty);
        }

        private static void OnTeleportEndedInternal()
        {
            OnTeleportEnded?.Invoke(null, EventArgs.Empty);
        }

        private static void OnTeleportFailedInternal()
        {
            OnTeleportFailed?.Invoke(null, EventArgs.Empty);
        }

        private static void OnPlayfieldInit(uint id)
        {
            PlayfieldInit?.Invoke(null, id);
        }
    }
}
