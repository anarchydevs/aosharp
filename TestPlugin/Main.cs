using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Core;
using AOSharp.Core.UI;
using AOSharp.Core.Movement;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Core.UI.Options;

namespace TestPlugin
{
    public class Main : IAOPluginEntry
    {
        private Menu _menu;

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
                /*
                Chat.WriteLine("Playfield");
                Chat.WriteLine($"   Identity: {Playfield.Identity}");
                Chat.WriteLine($"   Name: {Playfield.Name}");
                Chat.WriteLine($"   AllowsVehicles: {Playfield.AllowsVehicles}");
                Chat.WriteLine($"   IsDungeon: {Playfield.IsDungeon}");
                Chat.WriteLine($"   IsShadowlands: {Playfield.IsShadowlands}");
                Chat.WriteLine($"   NumDynels: {DynelManager.AllDynels.Count}");
                */

                MovementController movementController = new MovementController(true);

                List<Vector3> testPath = new List<Vector3> {
                    new Vector3(438.6, 8.0f, 524.4f),
                    new Vector3(446.8f, 8.0f, 503.7f),
                    new Vector3(460.8, 15.1f, 414.0f) 
                };

                movementController.RunPath(testPath);

                /*
                Chat.WriteLine("Missions");
                foreach (Mission mission in Mission.List)
                {
                    Chat.WriteLine($"   {mission.Identity.ToString()}");
                    Chat.WriteLine($"       Source: {mission.Source.ToString()}");
                    Chat.WriteLine($"       Playfield: {mission.Playfield.ToString()}");
                    Chat.WriteLine($"       Action: {mission.Actions[0].Type}");

                    switch (mission.Actions[0].Type)
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
                */

                //DynelManager.LocalPlayer.CastNano(new Identity(IdentityType.NanoProgram, 223372), DynelManager.LocalPlayer);

                _menu = new Menu("TestPlugin", "TestPlugin");
                _menu.AddItem(new MenuBool("DrawingTest", "Drawing Test", false));
                OptionsPanel.AddMenu(_menu);

                //Chat.WriteLine($"Self Identity: {DynelManager.LocalPlayer.Health}");
                //Inventory.Test(new Identity((IdentityType)0xDEAD, DynelManager.LocalPlayer.Identity.Instance));
                //Inventory.Test(DynelManager.LocalPlayer.Identity);
                //Inventory.Test(new Identity((IdentityType)0xC749, 0x001A618D));

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

        double lastTrigger = Time.NormalTime;

        private void OnUpdate(float deltaTime)
        {
            if (DynelManager.LocalPlayer.IsAttacking)
               return;

            if (_menu.GetBool("DrawingTest"))
            {
                foreach (Dynel player in DynelManager.Players)
                {
                    Debug.DrawSphere(player.Position, 1, DebuggingColor.LightBlue);
                    Debug.DrawLine(DynelManager.LocalPlayer.Position, player.Position, DebuggingColor.LightBlue);
                }
            }

            if(Time.NormalTime > lastTrigger + 3)
            {
                //Chat.WriteLine($"IsChecked: {((Checkbox)window.Views[0]).IsChecked}");
                //IntPtr tooltip = AOSharp.Core.Imports.ToolTip_c.Create("LOLITA", "COMPLEX");
                lastTrigger = Time.NormalTime;
            }

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
