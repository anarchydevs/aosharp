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
                Chat.WriteLine("TestPlugin loaded");

                Chat.WriteLine($"LocalPlayer: {DynelManager.LocalPlayer.Identity}");
                Chat.WriteLine($"   Name: {DynelManager.LocalPlayer.Name}");
                Chat.WriteLine($"   Pos: {DynelManager.LocalPlayer.Position}");
                Chat.WriteLine($"   MoveState: {DynelManager.LocalPlayer.MovementState}");
                Chat.WriteLine($"   Health: {DynelManager.LocalPlayer.GetStat(Stat.Health)}");

                Chat.WriteLine("Playfield");
                Chat.WriteLine($"   AllowsVehicles: {Playfield.AllowsVehicles}");
                Chat.WriteLine($"   NumDynels: {DynelManager.AllDynels.Count}");

                DynelManager.LocalPlayer.CastNano(new Identity(IdentityType.NanoProgram, 0x46146), DynelManager.LocalPlayer.Identity);

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
            if (DynelManager.LocalPlayer.IsAttacking)
                return;

            //SimpleChar leet = DynelManager.Characters.FirstOrDefault(x => x.Name == "34-I Helper" && x.IsAlive);

            //if (leet == null)
            //    return;

            //DynelManager.LocalPlayer.Attack(leet);
        }

        private void DynelSpawned(Dynel dynel)
        {
            if (dynel.Identity.Type == IdentityType.SimpleChar)
            {
                SimpleChar c = dynel.Cast<SimpleChar>();

                Chat.WriteLine($"SimpleChar Spawned(TestPlugin): {c.Identity} -- {c.Name} -- {c.Position} -- {c.Health}");
            }
        }
    }
}
