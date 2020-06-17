using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using AOSharp.Core.UI;
using AOSharp.Core.GameData;
using AOSharp.Core.Combat;

namespace AOSharp.Core
{
    public static class Game
    {
        public static EventHandler<float> OnEarlyUpdate;
        public static EventHandler<float> OnUpdate;
        public static EventHandler TeleportStarted;
        public static EventHandler TeleportEnded;
        public static EventHandler TeleportFailed; 
        public static EventHandler<uint> PlayfieldInit;

        public static void SetMovement(MovementAction action)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.MovementChanged(pEngine, action, 0, 0, true);
        }

        private static void OnEarlyUpdateInternal (float deltaTime)
        {
            OnEarlyUpdate?.Invoke(null, deltaTime);
        }

        private static void OnUpdateInternal(float deltaTime)
        {
            Network.Update();

            UIController.UpdateViews();

            if (CombatHandler.Instance != null)
                CombatHandler.Instance.Update(deltaTime);

            Perk.Update();
            Spell.Update();

            OnUpdate?.Invoke(null, deltaTime);

            Chat.Update();
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
