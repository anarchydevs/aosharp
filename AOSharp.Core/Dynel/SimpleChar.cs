using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Core.Imports;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public unsafe class SimpleChar : Dynel
    {
        public string Name => (*(SimpleChar_MemStruct*)Pointer).Name.ToString();

        public int Health => GetStat(Stat.Health);

        public int MaxHealth => GetStat(Stat.MaxHealth);

        public bool IsPlayer => !(*(SimpleChar_MemStruct*)Pointer).IsNPC;

        public bool IsNPC => (*(SimpleChar_MemStruct*)Pointer).IsNPC && !IsPet;

        public bool IsPet => Flags.HasFlag(DynelFlags.Pet);

        public bool IsAttacking => (*(SimpleChar_MemStruct*)Pointer).WeaponHolder->AttackingState == 0x02;

        public bool IsAlive => Health > 0;

        public SimpleChar FightingTarget => GetFightingTarget();

        public Dictionary<int, WeaponItem> Weapons => GetWeapons();

        public HashSet<SpecialAttack> SpecialAttacks => GetSpecialAttacks();

        internal IntPtr pWeaponHolder => (IntPtr)(*(SimpleChar_MemStruct*)Pointer).WeaponHolder;

        public SimpleChar(IntPtr pointer) : base(pointer)
        {
        }

        public SimpleChar(Dynel dynel) : base(dynel.Pointer)
        {
        }

        private SimpleChar GetFightingTarget()
        {
            IntPtr pFightingTarget = (*(SimpleChar_MemStruct*)Pointer).pFightingTarget;

            if (pFightingTarget == IntPtr.Zero)
                return null;

            return new SimpleChar(pFightingTarget);
        }

        public unsafe bool IsInAttackRange(Dynel target, bool requireAllWeapons = false)
        {
            Dictionary<int, WeaponItem> weapons = Weapons;

            if(weapons.Count > 0)
            {
                bool inRange = true;

                foreach(WeaponItem weapon in weapons.Values)
                {
                    if (!weapon.IsDynelInRange(target))
                    {
                        if (requireAllWeapons)
                            return false;

                        inRange = false;
                    }
                }

                return inRange;
            }
            else
            {
                IntPtr pWeaponHolder = DynelManager.LocalPlayer.pWeaponHolder;
                IntPtr dummyWeapon = WeaponHolder_t.GetDummyWeapon(pWeaponHolder, Stat.MartialArts);

                if (dummyWeapon == null)
                    return false;

                IntPtr pdummyWeaponUnk = *(IntPtr*)(dummyWeapon + 0xE4);

                return WeaponHolder_t.IsDynelInWeaponRange(pWeaponHolder, pdummyWeaponUnk, target.Pointer) == 0x01;
            }
        }

        private unsafe Dictionary<int, WeaponItem> GetWeapons()
        {
            Dictionary<int, WeaponItem> weapons = new Dictionary<int, WeaponItem>();

            IntPtr pWeaponHolder = (IntPtr)(*(SimpleChar_MemStruct*)Pointer).WeaponHolder;

            if (pWeaponHolder == IntPtr.Zero)
                return weapons;

            IntPtr right = WeaponHolder_t.GetWeapon(pWeaponHolder, 0x6, 0);

            if (right != IntPtr.Zero)
                weapons.Add(0x6, new WeaponItem(*(IntPtr*)(right + 0x14) + Offsets.RTTIDynamicCast.SimpleItem_t.n3Dynel_t, pWeaponHolder, right));

            IntPtr left = WeaponHolder_t.GetWeapon(pWeaponHolder, 0x8, 0);

            if (left != IntPtr.Zero)
                weapons.Add(0x8, new WeaponItem(*(IntPtr*)(left + 0x14) + Offsets.RTTIDynamicCast.SimpleItem_t.n3Dynel_t, pWeaponHolder, left));

            return weapons;
        }

        private HashSet<SpecialAttack> GetSpecialAttacks()
        {
            HashSet<SpecialAttack> specials = new HashSet<SpecialAttack>();
            Dictionary<int, WeaponItem> weapons = Weapons;

            if(weapons.Count > 0)
            {
                foreach (WeaponItem weapon in weapons.Values)
                {
                    foreach (SpecialAttack special in weapon.SpecialAttacks)
                    {
                        specials.Add(special);
                    }
                }
            }
            else
            {
                specials.Add(SpecialAttack.Brawl);
                specials.Add(SpecialAttack.Dimach);
            }

            return specials;
        }

        public unsafe bool IsInTeam()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return false;

            Identity identity = Identity;
            return N3EngineClientAnarchy_t.IsInTeam(pEngine, &identity);
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private unsafe struct SimpleChar_MemStruct
        {
            [FieldOffset(0x154)]
            public StdString Name;

            [FieldOffset(0x1D4)]
            public WeaponHolder* WeaponHolder;

            [FieldOffset(0x21C)]
            public bool IsNPC;

            [FieldOffset(0x250)]
            public IntPtr pFightingTarget;
        }
    }
}
