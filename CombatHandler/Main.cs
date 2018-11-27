using System;
using System.Collections.Generic;
using System.Linq;
using AOSharp.Core;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace CombatHandler
{
    public class Main : IAOPluginEntry
    {
        public int i = 0;

        public void Run()
        {
            try
            {
                Chat.WriteLine("CombatHandler loaded");

                List<AOSharp.Core.GameData.SpecialAction> actions = DynelManager.LocalPlayer.SpecialActions;

                Chat.WriteLine($"Specials: {DynelManager.LocalPlayer.SpecialActions.Count}");
                foreach(AOSharp.Core.GameData.SpecialAction action in actions)
                {
                    Chat.WriteLine($"   {action.Identity} - {action.Owner} - {action.OpCode}");
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
            if (DynelManager.LocalPlayer.IsAttacking)
                return;

            
        }

        private void DynelSpawned(Dynel dynel)
        {
            if (dynel.Identity.Type == IdentityType.SimpleChar)
            {
                SimpleChar c = dynel.Cast<SimpleChar>();

                Chat.WriteLine($"SimpleChar Spawned (CombatHandler): {c.Identity} -- {c.Name} -- {c.Position} -- {c.Health}");
            }
        }
    }
}
