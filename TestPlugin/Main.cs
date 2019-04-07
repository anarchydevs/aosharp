using System;
using System.Collections.Generic;
using System.IO;
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

        public unsafe void Run()
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
                Chat.WriteLine($"   Identity: {Playfield.Identity}");
                Chat.WriteLine($"   Name: {Playfield.Name}");
                Chat.WriteLine($"   AllowsVehicles: {Playfield.AllowsVehicles}");
                Chat.WriteLine($"   IsDungeon: {Playfield.IsDungeon}");
                Chat.WriteLine($"   IsShadowlands: {Playfield.IsShadowlands}");
                Chat.WriteLine($"   NumDynels: {DynelManager.AllDynels.Count}");

                Chat.WriteLine("Missions");
                foreach (Mission mission in Mission.List)
                {
                    Chat.WriteLine($"   {mission.Identity.ToString()}");
                    Chat.WriteLine($"       Source: {mission.Source.ToString()}");
                    Chat.WriteLine($"       Playfield: {mission.Playfield.ToString()}");
                    Chat.WriteLine($"       Action: {mission.Actions[0].Type}");

                    switch(mission.Actions[0].Type)
                    {
                        case MissionActionType.FindItem:
                            Chat.WriteLine($"           Target: {((FindItemAction)mission.Actions[0]).Target}");
                            break;
                        case MissionActionType.FindPerson:
                            Chat.WriteLine($"           Target: {((FindPersonAction)mission.Actions[0]).Target}");
                            break;
                        case MissionActionType.KillPerson:
                            Chat.WriteLine($"           Target: {((KillPersonAction)mission.Actions[0]).Target}");
                            break;
                        case MissionActionType.UseItemOnItem:
                            Chat.WriteLine($"           Source: {((UseItemOnItemAction)mission.Actions[0]).Source}");
                            Chat.WriteLine($"           Destination: {((UseItemOnItemAction)mission.Actions[0]).Destination}");
                            break;
                    }
                }

                var lookAtMsg = new SmokeLounge.AOtomation.Messaging.Messages.N3Messages.LookAtMessage()
                {
                    Identity = DynelManager.LocalPlayer.Identity
                };

                byte[] packet = PacketFactory.Create(lookAtMsg);

                Chat.WriteLine(BitConverter.ToString(packet).Replace("-", ""));

                Game.OnUpdate += OnUpdate;
                Game.OnTeleportStarted += Game_OnTeleportStarted;
                Game.OnTeleportEnded += Game_OnTeleportEnded;
                Game.OnTeleportFailed += Game_OnTeleportFailed;
                DynelManager.DynelSpawned += DynelSpawned;
            }
            catch (Exception e)
            {
                Chat.WriteLine(e.Message);
            }
        }

        private void Game_OnTeleportFailed()
        {
            Chat.WriteLine("Teleport Failed!");
        }

        private void Game_OnTeleportEnded()
        {
            Chat.WriteLine("Teleport Ended!");
        }

        private void Game_OnTeleportStarted()
        {
            Chat.WriteLine("Teleport Started!");
        }

        private void OnUpdate(float deltaTime)
        {
            if (DynelManager.LocalPlayer.IsAttacking)
               return;

            SimpleChar leet = DynelManager.Characters.FirstOrDefault(x => x.Name == "Leet" && x.IsAlive && DynelManager.LocalPlayer.IsInAttackRange(x));

            if (leet == null)
                return;

            DynelManager.LocalPlayer.Attack(leet);
        }

        private void DynelSpawned(Dynel dynel)
        {
            /*
            if (dynel.Identity.Type == IdentityType.SimpleChar)
            {
                SimpleChar c = dynel.Cast<SimpleChar>();

                Chat.WriteLine($"SimpleChar Spawned(TestPlugin): {c.Identity} -- {c.Name} -- {c.Position} -- {c.Health}");
            }
            */
        }
    }
}
