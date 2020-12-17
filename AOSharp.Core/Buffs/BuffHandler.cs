using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core.Combat;

namespace AOSharp.Core.Buffs
{
    class BuffHandler
    {
        public IEnumerable<Buff2> MissingBuffs
        {
            get
            {
                foreach (Buff2 buff in Buffs)
                {
                    foreach (SimpleChar buffTarget in GetBuffTargets(buff))
                    {
                        if (!buffTarget.Buffs.Contains(buff.ChildSpell.Identity.Instance))
                            yield return buff;
                    }
                }
            }
        }

        private IEnumerable<SimpleChar> GetBuffTargets(Buff2 buff2)
        {
            switch (buff2.Target)
            {
                case BuffTarget.Self:
                    yield return DynelManager.LocalPlayer;
                    break;
                case BuffTarget.Team:
                    if (Team.IsInTeam)
                    {
                        foreach (TeamMember teamMember in Team.Members)
                        {
                            if (DynelManager.Find(teamMember.Identity, out SimpleChar teamMemberSimpleChar))
                                yield return teamMemberSimpleChar;
                        }
                    }
                    else
                    {
                        yield return DynelManager.LocalPlayer;
                    }
                    break;
                case BuffTarget.HealingConstruct:
                case BuffTarget.MesmerizingConstruct:
                case BuffTarget.AttackConstruct:
                case BuffTarget.RoboticPet:
                    foreach (SimpleChar pet in DynelManager.LocalPlayer.GetPetDynels())
                    {
                        if (buff2.Target == BuffTarget.RoboticPet && pet.GetStat(Stat.NPCFamily) == 95)
                            yield return pet;

                        if (buff2.Target == BuffTarget.HealingConstruct && pet.GetStat(Stat.NPCFamily) == 96)
                            yield return pet;

                        if (buff2.Target == BuffTarget.AttackConstruct && pet.GetStat(Stat.NPCFamily) == 97)
                            yield return pet;

                        if (buff2.Target == BuffTarget.MesmerizingConstruct && pet.GetStat(Stat.NPCFamily) == 98)
                            yield return pet;
                    }
                    break;

            }
        }

        private List<Buff2> Buffs { get; set; }
        public static BuffHandler Instance { get; private set; }

        public BuffHandler()
        {
            Buffs = new List<Buff2>();
        }

        public static void Set(BuffHandler buffHandler)
        {
            Instance = buffHandler;
        }

        public TimeSpan TimeUntilRebuffRequired
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        
        public void RegisterBuff(Spell spell, BuffTarget buffTarget = BuffTarget.Self)
        {
            if (spell == null)
                return;

            RegisterBuff(spell, spell, buffTarget);
        }

        public void RegisterBuff(Spell spell, Spell effect, BuffTarget buffTarget = BuffTarget.Self)
        {
            if (spell == null)
                return;

            Buff2 buff2 = new Buff2(spell, effect, buffTarget);

            if (!Buffs.Contains(buff2))
                Buffs.Add(buff2);
        }

        public enum BuffTarget
        {
            Self,
            Team,
            HealingConstruct,
            MesmerizingConstruct,
            AttackConstruct,
            RoboticPet
        }

        public class Buff2 : IEqualityComparer<Buff2>
        {
            public Spell ParentSpell;
            public Spell ChildSpell;
            public BuffTarget Target;

            public Buff2(Spell parentSpell, BuffTarget buffTarget)
            {
                ParentSpell = parentSpell;
                ChildSpell = parentSpell;
                Target = buffTarget;
            }

            public Buff2(Spell parentSpell, Spell childSpell, BuffTarget buffTarget)
            {
                ParentSpell = parentSpell;
                ChildSpell = childSpell;
                Target = buffTarget;
            }

            public bool Equals(Buff2 x, Buff2 y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                if (x.Target != y.Target)
                    return false;

                if (x.ParentSpell != y.ParentSpell)
                    return false;

                return true;
            }

            public int GetHashCode(Buff2 obj)
            {
                return obj.ParentSpell.Identity.Instance ^ obj.ChildSpell.Identity.Instance ^ (int)Target;
            }
        }
    }
}
