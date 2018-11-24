using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Core;
using AOSharp.Common.GameData;

namespace TestPlugin
{
    public class Main : IAOPluginEntry
    {
        public void Run()
        {
            Game.OnUpdate += OnUpdate;
            Playfield.DynelSpawned += DynelSpawned;

            Chat.WriteLine("desu");
        }

        private void OnUpdate()
        {
            Debug.DrawSphere(new Vector3(160, 6, 173), 0.5f, DebuggingColor.LightBlue);
            Debug.DrawLine(new Vector3(165, 6, 165), new Vector3(165, 6, 175), DebuggingColor.Red);
        }

        private void DynelSpawned(Dynel dynel)
        {
            Game.SetMovement(MovementAction.TurnRightStart);
        }
    }
}
