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
        private float ACTION_TIMEOUT = 1f;
        private int MAX_CONCURRENT_PERKS = 4;
        protected Queue<CombatActionQueueItem> _actionQueue = new Queue<CombatActionQueueItem>();
        //protected Dictionary<(int lowId, int highId), ItemConditionProcessor> _itemRules = new Dictionary<(int lowId, int highId), ItemConditionProcessor>();
        private List<(PerkHash PerkHash, PerkConditionProcessor ConditionProcessor)> _perkRules = new List<(PerkHash, PerkConditionProcessor)>();
        private List<(int[] SpellGroup, SpellConditionProcessor ConditionProcessor)> _spellRules = new List<(int[], SpellConditionProcessor)>();

        protected delegate bool ItemConditionProcessor(Item item, SimpleChar fightingTarget,  out SimpleChar target);
        protected delegate bool PerkConditionProcessor(Perk perk, SimpleChar fightingTarget, out SimpleChar target);
        protected delegate bool SpellConditionProcessor(Spell spell, SimpleChar fightingTarget, out SimpleChar target);

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
            SimpleChar fightingTarget = DynelManager.LocalPlayer.FightingTarget;

            if (fightingTarget != null)
                SpecialAttacks(fightingTarget);

            //Only queue perks if we have no items awaiting usage and aren't over max concurrent perks
            if(!_actionQueue.Any(x => x.CombatAction is Item))
            {
                foreach(var perkRule in _perkRules)
                {
                    if (_actionQueue.Count(x => x.CombatAction is Perk) >= MAX_CONCURRENT_PERKS)
                        break;

                    if (!Perk.Find(perkRule.PerkHash, out Perk perk))
                        continue;

                    if (perk.IsPending || perk.IsExecuting || !perk.IsAvailable)
                        continue;

                    if (_actionQueue.Any(x => x.CombatAction is Perk action && action == perk))
                        continue;

                    if (perkRule.ConditionProcessor != null && perkRule.ConditionProcessor.Invoke(perk, fightingTarget, out SimpleChar target))
                    {
                        if (!perk.MeetsUseReqs(target))
                            continue;

                        //Chat.WriteLine($"Queueing perk {perk.Name} -- actionQ.Count = {_actionQueue.Count(x => x.CombatAction is Perk)}");
                        _actionQueue.Enqueue(new CombatActionQueueItem(perk, target));
                    }
                }
            }

            if (!Spell.HasPendingCast)
            {
                foreach (var spellRule in _spellRules)
                {
                    Spell spell = null;

                    foreach (int spellId in spellRule.SpellGroup)
                    {
                        if (!Spell.Find(spellId, out Spell curSpell))
                            continue;

                        if (!curSpell.MeetsSelfUseReqs())
                            continue;

                        spell = curSpell;
                    }

                    if (spell == null)
                        continue;

                    if (!spell.IsReady)
                        continue;

                    if (spellRule.ConditionProcessor != null && spellRule.ConditionProcessor.Invoke(spell, fightingTarget, out SimpleChar target))
                    {
                        if (!spell.MeetsUseReqs(target))
                            continue;

                        spell.Cast(target);
                        break;
                    }
                }
            }

            if (_actionQueue.Count > 0)
            {
                //Drop any expired items
                while (_actionQueue.Peek().Timeout <= Time.NormalTime)
                {
                    Chat.WriteLine("Action Timed out");
                    _actionQueue.Dequeue();
                }

                List<CombatActionQueueItem> dequeueList = new List<CombatActionQueueItem>();

                foreach (CombatActionQueueItem actionItem in _actionQueue)
                {
                    if (actionItem.Used)
                        continue;

                    if (actionItem.CombatAction is Item item)
                    {
                        //I have no real way of checking if a use action is valid so we'll just send it off and pray
                        item.Use(actionItem.Target);
                        actionItem.Used = true;
                        actionItem.Timeout = Time.NormalTime + ACTION_TIMEOUT;
                    }
                    else if (actionItem.CombatAction is Perk perk)
                    {
                        if (!perk.Use(actionItem.Target))
                        {
                            dequeueList.Add(actionItem);
                            continue;
                        }
                        
                        actionItem.Used = true;
                        actionItem.Timeout = Time.NormalTime + ACTION_TIMEOUT;          
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
                if (special == SpecialAttack.AimedShot ||
                    special == SpecialAttack.SneakAttack)
                    continue;

                if (!special.IsAvailable())
                    continue;

                if (!special.IsInRange(target))
                    continue;

                if (special == SpecialAttack.Backstab && (target.FightingTarget == DynelManager.LocalPlayer || target.IsFacing(DynelManager.LocalPlayer)))
                    continue;

                special.UseOn(target);
            }
        }

        protected void RegisterPerkProcessor(PerkHash perkHash, PerkConditionProcessor conditionProcessor)
        {
            _perkRules.Add((perkHash, conditionProcessor));
        }

        protected void RegisterSpellProcessor(Spell spell, SpellConditionProcessor conditionProcessor)
        {
            RegisterSpellProcessor(new[] { spell.Identity.Instance }, conditionProcessor);
        }

        protected void RegisterSpellProcessor(IEnumerable<Spell> spellGroup, SpellConditionProcessor conditionProcessor)
        {
            RegisterSpellProcessor(spellGroup.GetIds(), conditionProcessor);
        }

        protected void RegisterSpellProcessor(int spellId, SpellConditionProcessor conditionProcessor)
        {
            RegisterSpellProcessor(new[] { spellId }, conditionProcessor);
        }

        protected void RegisterSpellProcessor(int[] spellGroup, SpellConditionProcessor conditionProcessor)
        {
            if (spellGroup.Length == 0)
                return;

            _spellRules.Add((spellGroup, conditionProcessor));
        }

        internal void OnPerkExecuted(Perk perk)
        {

            Chat.WriteLine($"Perk Executed {perk.Name}");
            //Drop the queued action
            _actionQueue = new Queue<CombatActionQueueItem>(_actionQueue.Where(x => (Perk)x.CombatAction != perk));
        }

        internal void OnPerkLanded(Perk perk, double timeout)
        {
            //Update the queued perk's timeout to match the internal perk queue's
            foreach(CombatActionQueueItem queueItem in _actionQueue)
            {
                if (!(queueItem.CombatAction is Perk))
                    return;

                if ((Perk)queueItem.CombatAction == perk)
                {
                    //Chat.WriteLine($"Perk {perk.Name} landed. Time: {Time.NormalTime}\tOldTimeout: {queueItem.Timeout}\tNewTimeout: {timeout}");
                    queueItem.Timeout = timeout;
                }
            }
        }

        protected class CombatActionQueueItem : IEquatable<CombatActionQueueItem>
        {
            public ICombatAction CombatAction;
            public SimpleChar Target;
            public bool Used = false;
            public double Timeout = 0;

            public CombatActionQueueItem(ICombatAction action, SimpleChar target = null)
            {
                CombatAction = action;
                Target = target;
                Timeout = Time.NormalTime + 1;
            }

            public bool Equals(CombatActionQueueItem other)
            {
                if (other == null)
                    return false;

                if (CombatAction.GetType() != other.CombatAction.GetType())
                    return false;

                switch (CombatAction)
                {
                    case Perk perk:
                        return perk == ((Perk)other.CombatAction);
                    case Item item:
                        return item.LowId == ((Item)other.CombatAction).LowId || item.HighId == ((Item)other.CombatAction).HighId;
                    case Spell spell:
                        return spell == ((Spell)other.CombatAction);
                    default:
                        return false;
                }
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
