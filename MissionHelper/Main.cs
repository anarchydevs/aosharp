using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Core;
using AOSharp.Common.GameData;

namespace MissionHelper
{
    public class Main : IAOPluginEntry
    {
        public void Run(string pluginDir)
        {
            Chat.WriteLine("MissionHelper loaded");
            Game.OnUpdate += Game_OnUpdate;
        }

        private void Game_OnUpdate(object s, float deltaTime)
        {
            LocalPlayer localPlayer = DynelManager.LocalPlayer;

            if (localPlayer == null)
                return;

            if (!Playfield.IsDungeon)
                return;

            Mission? mission = Mission.List.Cast<Mission?>().FirstOrDefault(x => x.Value.Playfield == Playfield.ModelIdentity);

            if (!mission.HasValue)
                return;

            foreach(MissionAction action in mission.Value.Actions)
            {
                switch (action.Type)
                {
                    case MissionActionType.FindItem:
                        Dynel findItemTarget = DynelManager.AllDynels.FirstOrDefault(x => x.Identity == ((FindItemAction)action).Target);

                        if (findItemTarget == null)
                            continue;

                        Debug.DrawSphere(findItemTarget.Position, 1, DebuggingColor.Red);
                        Debug.DrawLine(localPlayer.Position, findItemTarget.Position, DebuggingColor.Red);
                        break;
                    case MissionActionType.FindPerson:
                        Dynel findPersonTarget = DynelManager.AllDynels.FirstOrDefault(x => x.Identity == ((FindPersonAction)action).Target);

                        if (findPersonTarget == null)
                            continue;

                        Debug.DrawSphere(findPersonTarget.Position, 1, DebuggingColor.Red);
                        Debug.DrawLine(localPlayer.Position, findPersonTarget.Position, DebuggingColor.Red);
                        break;
                    case MissionActionType.KillPerson:
                        Dynel killPersonTarget = DynelManager.AllDynels.FirstOrDefault(x => x.Identity == ((KillPersonAction)action).Target);

                        if (killPersonTarget == null)
                            continue;

                        Debug.DrawSphere(killPersonTarget.Position, 1, DebuggingColor.Red);
                        Debug.DrawLine(localPlayer.Position, killPersonTarget.Position, DebuggingColor.Red);
                        break;
                    case MissionActionType.UseItemOnItem:
                        Dynel item1 = DynelManager.AllDynels.FirstOrDefault(x => x.Identity == ((UseItemOnItemAction)action).Source);

                        if (item1 != null)
                        {
                            Debug.DrawSphere(item1.Position, 1, DebuggingColor.Red);
                            Debug.DrawLine(localPlayer.Position, item1.Position, DebuggingColor.Red);
                        }

                        Dynel item2 = DynelManager.AllDynels.FirstOrDefault(x => x.Identity == ((UseItemOnItemAction)action).Destination);

                        if (item2 != null)
                        {
                            Debug.DrawSphere(item2.Position, 1, DebuggingColor.Red);
                            Debug.DrawLine(localPlayer.Position, item2.Position, DebuggingColor.Red);
                        }

                        break;
                }
            }
        }
    }
}
