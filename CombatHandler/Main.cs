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
        public void Run()
        {
            try
            {
                Chat.WriteLine("CombatHandler loaded");

                HashSet<SpecialAttack> actions = DynelManager.LocalPlayer.SpecialAttacks;

                Chat.WriteLine($"Specials: {DynelManager.LocalPlayer.SpecialAttacks.Count}");
                foreach(SpecialAttack action in actions)
                {
                    Chat.WriteLine($"   {action}");
                }

                Dictionary<Stat, Cooldown> cooldowns = DynelManager.LocalPlayer.Cooldowns;

                Chat.WriteLine($"Cooldowns: {cooldowns.Count}");
                foreach (Cooldown cooldown in cooldowns.Values)
                {
                    Chat.WriteLine($"   {cooldown.Stat} - {cooldown.Remaining} / {cooldown.Total}");
                };

                Chat.WriteLine($"Weapons: {DynelManager.LocalPlayer.GetWeapons().Count}");

                Chat.WriteLine($"FA: {SpecialAttack.FastAttack.IsAvailable()}");

                Game.OnUpdate += OnUpdate;
            }
            catch (Exception e)
            {
                Chat.WriteLine(e.Message);
            }
        }

        private void OnUpdate()
        {
            if (!DynelManager.LocalPlayer.IsAttacking)
                return;

            SpecialAttacks();
        }

        private void SpecialAttacks()
        {
            SimpleChar fightingTarget = DynelManager.LocalPlayer.FightingTarget;

            if (fightingTarget == null)
                return;

            foreach(SpecialAttack special in DynelManager.LocalPlayer.SpecialAttacks)
            {
                if (special.IsAvailable())
                    special.UseOn(fightingTarget);
            }
        }
    }
}
