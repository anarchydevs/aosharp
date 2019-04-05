using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public static class Game
    {
        public delegate void OnUpdateEventHandler(float deltaTime);
        public static event OnUpdateEventHandler OnUpdate;

        public static void SetMovement(MovementAction action)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.MovementChanged(pEngine, action, 0, 0, true);
        }

        private unsafe static void UpdateInternal(float deltaTime)
        {
            if (OnUpdate != null)
                OnUpdate(deltaTime);
        }
    }
}
