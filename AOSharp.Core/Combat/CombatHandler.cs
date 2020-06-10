using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core;
using AOSharp.Core.Inventory;

namespace AOSharp.Core.Combat
{
    public class CombatHandler
    {
        private float ACTION_TIMEOUT = 2f;
        private int MAX_CONCURRENT_PERKS = 2;
        private Queue<CombatActionQueueItem> _actionQueue = new Queue<CombatActionQueueItem>();
        protected Dictionary<(int lowId, int highId), ItemConditionProcessor> _itemRules = new Dictionary<(int lowId, int highId), ItemConditionProcessor>();
        protected Dictionary<int, PerkConditionProcessor> _perkRules = new Dictionary<int, PerkConditionProcessor>();
        protected Dictionary<int, SpellConditionProcessor> _spellRules = new Dictionary<int, SpellConditionProcessor>();

        protected delegate bool ItemConditionProcessor(Item item, SimpleChar fightingTarget,  out (CombatActionType actionType, SimpleChar target) actionUsageInfo);
        protected delegate bool PerkConditionProcessor(Perk perk, SimpleChar fightingTarget, out (CombatActionType actionType, SimpleChar target) actionUsageInfo);
        protected delegate bool SpellConditionProcessor(Spell spell, SimpleChar fightingTarget, out (CombatActionType actionType, SimpleChar target) actionUsageInfo);

        public static CombatHandler Instance { get; private set; }

        public static void Set(CombatHandler combatHandler)
        {
            Instance = combatHandler;
        }

        internal void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected virtual void OnUpdate(float deltaTime)
        {
            SimpleChar target = DynelManager.LocalPlayer.FightingTarget;

            if (target != null)
                SpecialAttacks(target);

            foreach(Item item in Inventory.Inventory.Items)
            {
                //if()
            }

            //Only queue perks if we have no items awaiting usage and aren't over max concurrent perks
            if(!_actionQueue.Any(x => x.CombatAction is Item) && _actionQueue.Count(x => x.CombatAction is Perk) < MAX_CONCURRENT_PERKS)
            {
                foreach(var perkRule in _perkRules)
                {
                    Perk perk;
                    if (!Perk.Find(perkRule.Key, out perk))
                        continue;

                    if (perk.IsQueued || !perk.IsAvailable)
                        continue;

                    if (_actionQueue.Any(x => x.CombatAction == perk))
                        continue;

                    (CombatActionType type, SimpleChar target) actionUsageInfo;
                    if (perkRule.Value != null && perkRule.Value.Invoke(perk, target, out actionUsageInfo))
                        _actionQueue.Enqueue(new CombatActionQueueItem(perk, actionUsageInfo.type, target));
                }
            }


            foreach (Spell spell in Spell.List)
            {
                //if()
            }

            if (_actionQueue.Count > 0)
            {
                //Drop any expired items
                while (_actionQueue.Peek().Timeout <= Time.NormalTime)
                    _actionQueue.Dequeue();

                List<CombatActionQueueItem> dequeueList = new List<CombatActionQueueItem>();

                foreach (CombatActionQueueItem actionItem in _actionQueue)
                {
                    if (actionItem.Used)
                        continue;

                    if (actionItem.CombatAction is Item)
                    {
                        Item item = actionItem.CombatAction as Item;

                        //I have no real way of checking if a use action is valid so we'll just send it off and pray
                        item.Use(actionItem.Target);
                        actionItem.Used = true;
                        actionItem.Timeout = Time.NormalTime + item.AttackTime + ACTION_TIMEOUT;
                    }
                    else if (actionItem.CombatAction is Perk)
                    {
                        Perk perk = actionItem.CombatAction as Perk;

                        if (!perk.Use(actionItem.Target))
                        {
                            dequeueList.Add(actionItem);
                            continue;
                        }
                        
                        actionItem.Used = true;
                        actionItem.Timeout = Time.NormalTime + perk.AttackTime + ACTION_TIMEOUT;          
                    }
                }

                //Drop any failed actions
                _actionQueue = new Queue<CombatActionQueueItem>(_actionQueue.Where(x => !dequeueList.Contains(x)));
            }
        }

        private void SpecialAttacks(SimpleChar target)
        {
            foreach (SpecialAttack special in DynelManager.LocalPlayer.SpecialAttacks)
            {
                if (!special.IsAvailable())
                    continue;

                if (!special.IsInRange(target))
                    continue;

                special.UseOn(target);
            }
        }

        internal void OnPerkExecuted(Perk perk)
        {
            //Drop the queued action
            _actionQueue = new Queue<CombatActionQueueItem>(_actionQueue.Where(x => x.CombatAction != perk));
        }

        protected class CombatActionQueueItem : IEquatable<CombatActionQueueItem>
        {
            public ICombatAction CombatAction;
            public CombatActionType Type;
            public SimpleChar Target;
            public bool Used = false;
            public double Timeout = 0;

            public CombatActionQueueItem(ICombatAction action, CombatActionType type, SimpleChar target = null)
            {
                CombatAction = action;
                Type = type;
                Target = target ?? DynelManager.LocalPlayer;
                Timeout = Time.NormalTime + 10;
            }

            public bool Equals(CombatActionQueueItem other)
            {
                if (CombatAction.GetType() != other.GetType())
                    return false;

                if (CombatAction is Perk)
                {
                    return ((Perk)CombatAction).Identity == ((Perk)other.CombatAction).Identity;
                } 
                else if (CombatAction is Item)
                {
                    return ((Item)CombatAction).LowId == ((Item)other.CombatAction).LowId || ((Item)CombatAction).HighId == ((Item)other.CombatAction).HighId;
                }
                else if(CombatAction is Spell)
                {
                    return ((Spell)CombatAction).Identity == ((Spell)other.CombatAction).Identity;
                }

                return false;
            }
        }

        protected enum CombatActionType
        {
            Damage,
            Heal,
            Buff
        }
    }
}
