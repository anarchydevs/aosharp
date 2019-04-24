using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Core.Imports;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public unsafe class WeaponItem : SimpleItem
    {
        public float AttackRange => GetStat(Stat.AttackRange);

        public readonly HashSet<SpecialAttack> SpecialAttacks;

        private readonly IntPtr _pWeaponHolder;
        private readonly IntPtr _pWeaponUnk;

        public WeaponItem(IntPtr pointer, IntPtr pWeaponHolder, IntPtr pWeaponUnk) : base(pointer)
        {
            _pWeaponHolder = pWeaponHolder;
            _pWeaponUnk = pWeaponUnk;
            SpecialAttacks = GetSpecialAttacks();
        }

        public WeaponItem(Dynel dynel) : base(dynel.Pointer)
        {
        }

        private HashSet<SpecialAttack> GetSpecialAttacks()
        {
            HashSet<SpecialAttack> specials = new HashSet<SpecialAttack>();
            SpecialAttackFlags canFlags = (SpecialAttackFlags)GetStat(Stat.Can);

            if (canFlags.HasFlag(SpecialAttackFlags.AimedShot))
                specials.Add(SpecialAttack.AimedShot);

            if (canFlags.HasFlag(SpecialAttackFlags.Brawl))
                specials.Add(SpecialAttack.Brawl);

            if (canFlags.HasFlag(SpecialAttackFlags.Burst))
                specials.Add(SpecialAttack.Burst);

            if (canFlags.HasFlag(SpecialAttackFlags.Dimach))
                specials.Add(SpecialAttack.Dimach);

            if (canFlags.HasFlag(SpecialAttackFlags.FastAttack))
                specials.Add(SpecialAttack.FastAttack);

            if (canFlags.HasFlag(SpecialAttackFlags.FlingShot))
                specials.Add(SpecialAttack.FlingShot);

            if (canFlags.HasFlag(SpecialAttackFlags.FullAuto))
                specials.Add(SpecialAttack.FullAuto);

            if (canFlags.HasFlag(SpecialAttackFlags.SneakAttack))
                specials.Add(SpecialAttack.SneakAttack);

            return specials;
        }

        public bool IsDynelInRange(Dynel target)
        {
            return WeaponHolder_t.IsDynelInWeaponRange(_pWeaponHolder, _pWeaponUnk, target.Pointer) == 0x01;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private unsafe struct WeaponItem_MemStruct
        {

        }
    }
}
