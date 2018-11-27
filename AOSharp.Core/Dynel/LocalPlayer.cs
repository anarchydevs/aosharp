using System;
using System.Linq;
using System.Collections.Generic;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core
{
    public unsafe class LocalPlayer : SimpleChar
    {
        public List<GameData.SpecialAction> SpecialActions => GetSpecialActionList();

        public LocalPlayer(IntPtr pointer) : base(pointer)
        {
        }

        public void Attack(Dynel target)
        {
            if (target.GetStat(Stat.Health) == 0)
                return;

            Attack(target.Identity);
        }

        //TODO: Silence this function to prevent flood of "Please wait until previous action has finished."
        public void Attack(Identity target)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.DefaultAttack(pEngine, &target, true);
        }

        public void CastNano(Identity nano, Dynel target)
        {
            CastNano(nano, target.Identity);
        }

        public void CastNano(Identity nano, Identity target)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.CastNanoSpell(pEngine, &nano, &target);
        }

        internal List<GameData.SpecialAction> GetSpecialActionList()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return new List<GameData.SpecialAction>();

            return N3EngineClientAnarchy_t.GetSpecialActionList(pEngine)->ToList().Select(x => *(SpecialAction*)x).ToList();
        }
    }
}
