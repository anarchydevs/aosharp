using System;
using System.Collections.Generic;
using System.Linq;

namespace AOSharp.Core
{
    public static class Extensions
    {
        public static bool Find(this List<Buff> buffs, int id, out Buff buff)
        {
            return (buff = buffs.FirstOrDefault(x => x.Identity.Instance == id)) != null;
        }
    }
}
