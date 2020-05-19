using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Core;
using AOSharp.Core.UI;
using AOSharp.Core.Inventory;
using AOSharp.Core.Movement;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Core.UI.Options;

namespace TestPlugin
{
    public class Main : IAOPluginEntry
    {
        private Menu _menu;

        public unsafe void Run(string pluginDir)
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

                /*
                MovementController movementController = new MovementController(true);

                List<Vector3> testPath = new List<Vector3> {
                    new Vector3(438.6, 8.0f, 524.4f),
                    new Vector3(446.8f, 8.0f, 503.7f),
                    new Vector3(460.8, 15.1f, 414.0f) 
                };

                movementController.RunPath(testPath);
                */

                /*
                Chat.WriteLine("Missions");
                foreach (Mission mission in Mission.List)
                {
                    Chat.WriteLine($"   {mission.Identity.ToString()}");
                    Chat.WriteLine($"       Source: {mission.Source.ToString()}");
                    Chat.WriteLine($"       Playfield: {mission.Playfield.ToString()}");
                    Chat.WriteLine($"       DisplayName: {mission.DisplayName}");
                }
                */
                //DynelManager.LocalPlayer.CastNano(new Identity(IdentityType.NanoProgram, 223372), DynelManager.LocalPlayer);

                _menu = new Menu("TestPlugin", "TestPlugin");
                _menu.AddItem(new MenuBool("DrawingTest", "Drawing Test", false));
                OptionsPanel.AddMenu(_menu);

                //Chat.WriteLine($"Self Identity: {DynelManager.LocalPlayer.Health}");
                //Inventory.Test(new Identity((IdentityType)0xDEAD, DynelManager.LocalPlayer.Identity.Instance));
                List<Item> characterItems = Inventory.Items;

                foreach(Item item in characterItems)
                {
                    Chat.WriteLine($"{item.Slot} - {item.Unk1} - {item.Unk2} - {item.LowId} - {item.QualityLevel} - {item.ContainerIdentity}");
                }

                Chat.WriteLine("Backpacks:");

                List<Container> backpacks = Inventory.Backpacks;
                foreach(Container backpack in backpacks)
                {
                    Chat.WriteLine($"{backpack.Identity} - IsOpen:{backpack.IsOpen}{((backpack.IsOpen) ? $" - Items:{backpack.Items.Count}" : "")}");
                }

                Item noviRing;
                if (Inventory.Find(226307, out noviRing))
                {
                    //noviRing.Equip(EquipSlot.Cloth_RightFinger);

                    Container openBag = Inventory.Backpacks.FirstOrDefault(x => x.IsOpen);
                    if(openBag != null)
                    {
                        noviRing.MoveToContainer(openBag);
                    }
                }

                Game.OnUpdate += OnUpdate;
                Game.OnTeleportStarted += Game_OnTeleportStarted;
                Game.OnTeleportEnded += Game_OnTeleportEnded;
                Game.OnTeleportFailed += Game_OnTeleportFailed;
                Game.PlayfieldInit += Game_PlayfieldInit;
                Game.N3MessageReceived += Game_N3MessageReceived;
                NpcDialog.AnswerListChanged += NpcDialog_AnswerListChanged;
                DynelManager.DynelSpawned += DynelSpawned;
            }
            catch (Exception e)
            {
                Chat.WriteLine(e.Message);
            }
        }

        private void Game_N3MessageReceived(object s, SmokeLounge.AOtomation.Messaging.Messages.N3Message n3Msg)
        {
            //Chat.WriteLine($"{n3Msg.N3MessageType}");
        }

        private void NpcDialog_AnswerListChanged(object s, Dictionary<int, string> options)
        {
            /*
            Identity target = (Identity)s;

            foreach(KeyValuePair<int, string> option in options)
            {
                if (option.Value == "Is there anything I can help you with?" ||
                    option.Value == "I will defend against the creatures of the brink!" ||
                    option.Value == "I will deal with only the weakest aversaries")
                    NpcDialog.SelectAnswer(target, option.Key);
            }
            */
        }

        private void Game_PlayfieldInit(object s, uint id)
        {
            Chat.WriteLine($"PlayfieldInit: {id}");
        }

        private void Game_OnTeleportFailed(object s, EventArgs e)
        {
            Chat.WriteLine("Teleport Failed!");
        }

        private void Game_OnTeleportEnded(object s, EventArgs e)
        {
            Chat.WriteLine($"Teleport Ended!");

            InfBuddy.InfBuddy.NavMeshMovementController.MoveTo(new Vector3(2807, 25.4f, 3390.7));
        }

        private void Game_OnTeleportStarted(object s, EventArgs e)
        {
            Chat.WriteLine("Teleport Started!");
        }

        double lastTrigger = Time.NormalTime;

        private void OnUpdate(object s, float deltaTime)
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

        private void DynelSpawned(object s, Dynel dynel)
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
