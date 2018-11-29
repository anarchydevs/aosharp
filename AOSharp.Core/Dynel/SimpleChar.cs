using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
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

        public HashSet<SpecialAttack> SpecialAttacks => GetSpecialAttacks();

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

        private HashSet<SpecialAttack> GetSpecialAttacks()
        {
            HashSet<SpecialAttack> specials = new HashSet<SpecialAttack>();
            List<WeaponItem> weapons = GetWeapons();

            if (weapons.Count == 0)
            {
                specials.Add(SpecialAttack.Brawl);
                specials.Add(SpecialAttack.Dimach);
                return specials;
            }

            foreach (WeaponItem weapon in weapons)
            {
                SpecialAttackFlags canFlags = (SpecialAttackFlags)weapon.GetStat(Stat.Can);

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
            }

            return specials;
        }

        public unsafe List<WeaponItem> GetWeapons()
        {
            List<WeaponItem> weapons = new List<WeaponItem>();

            IntPtr pWeaponHolder = (IntPtr)(*(SimpleChar_MemStruct*)Pointer).WeaponHolder;

            if (pWeaponHolder == IntPtr.Zero)
                return weapons;

            IntPtr pUnk1 = *(IntPtr*)(pWeaponHolder + 0x20);

            if (pUnk1 == IntPtr.Zero)
                return weapons;

            IntPtr pUnk2 = *(IntPtr*)(pUnk1 + 0x04);

            if (pUnk2 == IntPtr.Zero)
                return weapons;

            IntPtr pRightHandWeapon = *(IntPtr*)(pUnk2 + 0x10);

            if (pRightHandWeapon != IntPtr.Zero)
                weapons.Add(new WeaponItem(*(IntPtr*)(pRightHandWeapon + 0x14) + 0xB0));

            IntPtr pUnk3 = *(IntPtr*)(pUnk2 + 0x08);

            if (pUnk3 == IntPtr.Zero)
                return weapons;

            IntPtr pLeftHandWeapon = *(IntPtr*)(pUnk3 + 0x10);

            if (pLeftHandWeapon != IntPtr.Zero)
                weapons.Add(new WeaponItem(*(IntPtr*)(pLeftHandWeapon + 0x14) + 0xB0));

            return weapons;
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
