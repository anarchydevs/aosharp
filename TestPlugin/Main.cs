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
        public int i = 0;

        public void Run()
        {
            try
            {
                Chat.WriteLine("Plugin loaded");

                Chat.WriteLine($"LocalPlayer: {DynelManager.LocalPlayer.Identity}");
                Chat.WriteLine($"   Name: {DynelManager.LocalPlayer.Name}");
                Chat.WriteLine($"   Pos: {DynelManager.LocalPlayer.Position}");
                Chat.WriteLine($"   MoveState: {DynelManager.LocalPlayer.MovementState}");

                Chat.WriteLine("Playfield");
                Chat.WriteLine($"   AllowsVehicles: {Playfield.AllowsVehicles}");
                Chat.WriteLine($"   NumDynels: {DynelManager.AllDynels.Count}");

                foreach(Dynel dynel in DynelManager.AllDynels)
                {
                    Chat.WriteLine($"Dynel {dynel.Identity}: ");
                }

                foreach (SimpleChar c in DynelManager.Characters)
                {
                    Chat.WriteLine($"SimpleChar {c.Identity}: {c.Name}");
                }

                foreach (SimpleChar c in DynelManager.Players)
                {
                    Chat.WriteLine($"Player {c.Identity}: {c.Name}");
                }

                Game.OnUpdate += OnUpdate;
                DynelManager.DynelSpawned += DynelSpawned;
            }
            catch (Exception e)
            {
                Chat.WriteLine(e.Message);
            }
        }

        private void OnUpdate()
        {
        }

        private void DynelSpawned(Dynel dynel)
        {
            try
            {
                if (dynel.Identity.Type == IdentityType.SimpleChar)
                {
                    SimpleChar c = dynel.Cast<SimpleChar>();
                    Chat.WriteLine($"SimpleChar Spawned: {c.Identity} -- {c.Name} -- {c.Position} -- {c.MovementState}");
                    //Chat.WriteLine($"SimpleChar Spawned: {c.Identity}");
                }
            } catch (Exception e)
            {
                Chat.WriteLine(e.Message);
            }
        }
    }
}
