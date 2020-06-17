using AOSharp.Common.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AOSharp.Core
{
    public static class Extensions
    {
        public static bool Find(this Buff[] buffs, int id, out Buff buff)
        {
            return (buff = buffs.FirstOrDefault(x => x.Identity.Instance == id)) != null;
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
