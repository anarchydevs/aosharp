using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Bootstrap;
using AOSharp.Core;
using AOSharp.Core.UI;
using AOSharp.Core.Inventory;
using AOSharp.Core.Movement;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Core.UI.Options;
using AOSharp.Core.Imports;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace TestPlugin
{
    public class Main : IAOPluginEntry
    {
        private Menu _menu;
        private int i = 0;
        public void Run(string pluginDir)
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

                Chat.WriteLine("Team:");
                Chat.WriteLine($"\tIsInTeam: {Team.IsInTeam}");
                Chat.WriteLine($"\tIsLeader: {Team.IsLeader}");
                Chat.WriteLine($"\tIsRaid: {Team.IsRaid}");

                foreach(TeamMember teamMember in Team.Members)
                {
                    Chat.WriteLine($"\t{teamMember.Name} - {teamMember.Identity} - {teamMember.Level} - {teamMember.Profession} - IsLeader:{teamMember.IsLeader} @ Team {teamMember.TeamIndex + 1}");
                }

                Chat.WriteLine("Tests:");



                /*
                foreach(Spell spell in Spell.List)
                {
                    Chat.WriteLine($"\t{spell.Identity}\t{spell.Name}\t{spell.MeetsUseReqs()}\t{spell.IsReady}");
                }
                */

                
                foreach(Perk perk in Perk.List)
                {
                    Chat.WriteLine($"\t{perk.Identity}\t{perk.Hash}\t{perk.Name}\t{perk.MeetsSelfUseReqs()}\t{perk.GetStat(Stat.AttackDelay)}");
                    //Chat.WriteLine($"{perk.Name} = 0x{((uint)perk.Hash).ToString("X4")},");
                }

                /*
                Chat.WriteLine("Buffs:");
                foreach(Buff buff in DynelManager.LocalPlayer.Buffs)
                {
                    Chat.WriteLine($"\tBuff: {buff.Name}\t{buff.RemainingTime}/{buff.TotalTime}");
                }
                */

                /*
                Perk perk;
                if(Perk.Find(PerkHash.Gore, out perk))
                {
                    if(DynelManager.LocalPlayer.FightingTarget != null)
                        Chat.WriteLine($"Can use perk? {perk.MeetsUseReqs(DynelManager.LocalPlayer.FightingTarget)}");
                }
                */

                /*
                Chat.WriteLine("Pet Identities:");
                foreach(Identity identity in DynelManager.LocalPlayer.Pets)
                {
                    Chat.WriteLine($"\t{identity}");
                }

                Chat.WriteLine("Pet Dynels:");
                foreach(SimpleChar pet in DynelManager.LocalPlayer.GetPetDynels())
                {
                    Chat.WriteLine($"\t{pet.Name}");
                }
                */

                /*
                Item item;
                if(Inventory.Find(244216, out item))
                {
                    DummyItem dummyItem = DummyItem.GetFromTemplate(item.Slot);
                    dummyItem.MeetsUseReqs();
                }
                */

                //DevExtras.Test();

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
                OptionPanel.AddMenu(_menu);


                /*
                List<Item> characterItems = Inventory.Items;
            
                foreach(Item item in characterItems)
                {
                    Chat.WriteLine($"{item.Slot} - {item.LowId} - {item.Name} - {item.QualityLevel} - {item.UniqueIdentity}");
                }

                Chat.WriteLine("Backpacks:");

                List<Container> backpacks = Inventory.Backpacks;
                foreach(Container backpack in backpacks)
                {
                    Chat.WriteLine($"{backpack.Identity} - IsOpen:{backpack.IsOpen}{((backpack.IsOpen) ? $" - Items:{backpack.Items.Count}" : "")}");
                }        
                */

                /*
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
                */

                Game.OnUpdate += OnUpdate;
                Game.TeleportStarted += Game_OnTeleportStarted;
                Game.TeleportEnded += Game_OnTeleportEnded;
                Game.TeleportFailed += Game_OnTeleportFailed;
                Game.PlayfieldInit += Game_PlayfieldInit;
                MiscClientEvents.AttemptingSpellCast += AttemptingSpellCast;
                Network.N3MessageReceived += Network_N3MessageReceived;
                Team.TeamRequest += Team_TeamRequest;
                Team.MemberLeft += Team_MemberLeft;
                Item.ItemUsed += Item_ItemUsed;
                NpcDialog.AnswerListChanged += NpcDialog_AnswerListChanged;
                DynelManager.DynelSpawned += DynelSpawned;
            }
            catch (Exception e)
            {
                Chat.WriteLine(e.Message);
            }
        }

        private void AttemptingSpellCast(object sender, AttemptingSpellCastEventArgs e)
        {
            Chat.WriteLine($"{e.Nano}, {e.Target}");
        }

        private void Network_N3MessageReceived(object s, SmokeLounge.AOtomation.Messaging.Messages.N3Message n3Msg)
        {
            //Chat.WriteLine($"{n3Msg.N3MessageType}");

            if(n3Msg.N3MessageType == SmokeLounge.AOtomation.Messaging.Messages.N3MessageType.TemplateAction)
            {
                TemplateActionMessage ayy = (TemplateActionMessage)n3Msg;
                Chat.WriteLine($"TemplateAction: {ayy.Unknown1.ToString()}\t{ayy.Unknown2.ToString()}\t{ayy.Unknown3.ToString()}\t{ayy.Unknown4.ToString()}\t{ayy.ItemLowId.ToString()}\t{ayy.Placement.ToString()}\t{ayy.Identity.ToString()}");
            }

            if (n3Msg.N3MessageType == SmokeLounge.AOtomation.Messaging.Messages.N3MessageType.Feedback)
            {
                FeedbackMessage ayy = (FeedbackMessage)n3Msg;
                Chat.WriteLine($"Feedback: {ayy.MessageId.ToString()}\t{ayy.CategoryId.ToString()}\t{ayy.Unknown1.ToString()}");
            }
        }

        private void Team_TeamRequest(object s, TeamRequestEventArgs e)
        {
            e.Accept();
        }

        private void Team_MemberLeft(object s, Identity leaver)
        {
            Chat.WriteLine($"Player {leaver} left the team.");
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

        private void Game_OnTeleportStarted(object s, EventArgs e)
        {
            Chat.WriteLine("Teleport Started!");
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
        }

        private void Item_ItemUsed(object s, ItemUsedEventArgs e)
        {
            Chat.WriteLine($"Item {e.Item.Name} used by {e.OwnerIdentity}");
        }

        double lastTrigger = Time.NormalTime;

        private void OnUpdate(object s, float deltaTime)
        {
            if (_menu.GetBool("DrawingTest"))
            {
                foreach (Dynel player in DynelManager.Players)
                {
                    Debug.DrawSphere(player.Position, 1, DebuggingColor.LightBlue);
                    Debug.DrawLine(DynelManager.LocalPlayer.Position, player.Position, DebuggingColor.LightBlue);
                }
            }

            if (Time.NormalTime > lastTrigger + 0.05)
            {
                //Chat.WriteLine($"IsChecked: {((Checkbox)window.Views[0]).IsChecked}");
                //IntPtr tooltip = AOSharp.Core.Imports.ToolTip_c.Create("LOLITA", "COMPLEX");

                /*
                Spell testSpell;
                if(Spell.Find("Matrix of Ka", out testSpell))
                {
                    if (testSpell.IsReady && testSpell.MeetsUseReqs())
                        testSpell.Cast();
                }
                */

                /*
                Perk testPerk;
                if(Perk.Find("Dance of Fools", out testPerk))
                {
                    if (testPerk.IsAvailable)
                        testPerk.Use();
                }
                */
                //SimpleChar randomTarget = DynelManager.Characters.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                //Targeting.SetTarget(randomTarget);

                lastTrigger = Time.NormalTime;
            }
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
