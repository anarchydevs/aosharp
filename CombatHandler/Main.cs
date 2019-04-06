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

                Dictionary<Stat, Cooldown> cooldowns = DynelManager.LocalPlayer.Cooldowns;

                Chat.WriteLine($"Cooldowns: {cooldowns.Count}");
                foreach (Cooldown cooldown in cooldowns.Values)
                {
                    Chat.WriteLine($"   {cooldown.Stat} - {cooldown.Remaining} / {cooldown.Total}");
                };


                Chat.WriteLine($"Weapons: {DynelManager.LocalPlayer.Weapons.Count}");

                foreach (WeaponItem weapon in DynelManager.LocalPlayer.Weapons.Values)
                {
                    Chat.WriteLine($"   Pointer: {weapon.Pointer.ToString("X4")}");
                    Chat.WriteLine($"       Specials: {weapon.SpecialAttacks.Count}");
                    foreach (SpecialAttack special in weapon.SpecialAttacks)
                    {
                        Chat.WriteLine($"          {special.ToString()}");
                    }
                }

                Chat.WriteLine($"Specials: {DynelManager.LocalPlayer.SpecialAttacks.Count}");
                foreach (SpecialAttack special in DynelManager.LocalPlayer.SpecialAttacks)
                {
                    Chat.WriteLine($"          {special.ToString()}");
                }

                Game.OnUpdate += OnUpdate;
            }
            catch (Exception e)
            {
                Chat.WriteLine(e.ToString());
            }
        }

        private void OnUpdate(float deltaTime)
        {
            if (!DynelManager.LocalPlayer.IsAttacking)
                return;

            SimpleChar fightingTarget = DynelManager.LocalPlayer.FightingTarget;

            if (fightingTarget != null)
            {
                Debug.DrawSphere(fightingTarget.Position, 1, DebuggingColor.LightBlue);
                Debug.DrawLine(DynelManager.LocalPlayer.Position, fightingTarget.Position, DebuggingColor.LightBlue);
            }

            SpecialAttacks();
        }

        private void SpecialAttacks()
        {
            SimpleChar fightingTarget = DynelManager.LocalPlayer.FightingTarget;

            if (fightingTarget == null)
                return;

            foreach(SpecialAttack special in DynelManager.LocalPlayer.SpecialAttacks)
            {
                if (!special.IsAvailable())
                    continue;

                if (!special.IsInRange(fightingTarget))
                    continue;

                special.UseOn(fightingTarget);
            }
        }
    }
}
