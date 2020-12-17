using AOSharp.Common.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AOSharp.Core
{
    public static class Extensions
    {
        public static T Cast<T>(this Dynel dynel) where T : Dynel
        {
            return (T)Activator.CreateInstance(typeof(T), dynel.Pointer);
        }

        public static bool Find(this Buff[] buffs, Spell spell, out Buff buff)
        {
            return (buff = buffs.FirstOrDefault(x => x.Identity.Instance == spell.Identity.Instance)) != null;
        }

        public static bool Find(this Buff[] buffs, int id, out Buff buff)
        {
            return (buff = buffs.FirstOrDefault(x => x.Identity.Instance == id)) != null;
        }

        public static bool Find(this Buff[] buffs, Nanoline nanoline, out Buff buff)
        {
            return (buff = buffs.FirstOrDefault(x => (Nanoline)x.GetStat(Stat.NanoStrain) == nanoline)) != null;
        }

        public static bool Contains(this Buff[] buffs, int id)
        {
            return Contains(buffs, new[] { id });
        }

        public static bool Contains(this Buff[] buffs, int[] ids)
        {
            return buffs.Any(b => ids.Contains(b.Identity.Instance));
        }

        public static bool Contains(this Buff[] buffs, Nanoline nanoline)
        {
            return buffs.Any(b => (Nanoline) b.GetStat(Stat.NanoStrain) == nanoline);
        }

        public static IEnumerable<Spell> OrderByStackingOrder(this IEnumerable<Spell> spells)
        {
            return spells.OrderByDescending(x => x.StackingOrder);
        }

        public static int[] GetIds(this IEnumerable<Spell> spells)
        {
            return spells.Select(x => x.Identity.Instance).ToArray();
        }
    }
}
