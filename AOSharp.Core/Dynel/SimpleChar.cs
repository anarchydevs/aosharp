using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Core.Imports;
using AOSharp.Common.GameData;
using System.Net.NetworkInformation;
using System.Net;
using System.Configuration.Assemblies;

namespace AOSharp.Core
{
    public unsafe class SimpleChar : Dynel
    {
        public string Name => (*(MemStruct*)Pointer).Name.ToString();

        public int Health => GetStat(Stat.Health);

        public int MaxHealth => GetStat(Stat.MaxHealth);

        public float HealthPercent => (float)Health / MaxHealth * 100;

        public int MissingHealth => MaxHealth - Health;

        public int Nano => GetStat(Stat.CurrentNano);

        public int MaxNano => GetStat(Stat.MaxNanoEnergy);

        public float NanoPercent => (float)Nano / MaxNano * 100;

        public int MissingNano => MaxNano - Nano;

        public bool IsPlayer => !(*(MemStruct*)Pointer).IsNPC;

        public bool IsNpc => (*(MemStruct*)Pointer).IsNPC && !IsPet;

        public bool IsPet => Flags.HasFlag(DynelFlags.Pet);

        public Profession Profession => (Profession)DynelManager.LocalPlayer.GetStat(Stat.Profession);

        public Breed Breed => (Breed) DynelManager.LocalPlayer.GetStat(Stat.Breed);

        public Side Side => (Side)DynelManager.LocalPlayer.GetStat(Stat.Side);

        public bool IsAttacking => (*(MemStruct*)Pointer).WeaponHolder->AttackingState == 0x02;

        public bool IsAlive => Health > 0;

        public SimpleChar FightingTarget => GetFightingTarget();

        public Buff[] Buffs => GetBuffs();

        public Dictionary<EquipSlot, WeaponItem> Weapons => GetWeapons();

        public HashSet<SpecialAttack> SpecialAttacks => GetSpecialAttacks();

        internal IntPtr pWeaponHolder => (IntPtr)(*(MemStruct*)Pointer).WeaponHolder;

        public SimpleChar(IntPtr pointer) : base(pointer)
        {
        }

        public SimpleChar(Dynel dynel) : base(dynel.Pointer)
        {
        }

        private SimpleChar GetFightingTarget()
        {
            IntPtr pFightingTarget = (*(MemStruct*)Pointer).pFightingTarget;

            if (pFightingTarget == IntPtr.Zero)
                return null;

            return new SimpleChar(pFightingTarget);
        }
        
        public bool IsFacing(SimpleChar target)
        {
            return Vector3.Angle(Rotation.Forward, target.Position - Position) <= 90f;
        }

        public unsafe bool IsInRange(Dynel target)
        {
            const EquipSlot mainHand = EquipSlot.Weap_RightHand;
            const EquipSlot offHand = EquipSlot.Weap_LeftHand;

            Dictionary<EquipSlot, WeaponItem> weapons = DynelManager.LocalPlayer.Weapons;

            if (weapons.Count > 0)
            {
                if (weapons.ContainsKey(mainHand))
                    return weapons[mainHand].IsDynelInRange(target);

                if (weapons.ContainsKey(offHand))
                    return weapons[offHand].IsDynelInRange(target);

                return false;
            }
            else
            {
                IntPtr pWeaponHolder = DynelManager.LocalPlayer.pWeaponHolder;
                IntPtr dummyWeapon = WeaponHolder_t.GetDummyWeapon(pWeaponHolder, Stat.MartialArts);

                if (dummyWeapon == IntPtr.Zero)
                    return false;

                IntPtr pdummyWeaponUnk = *(IntPtr*)(dummyWeapon + 0xE4);

                return WeaponHolder_t.IsDynelInWeaponRange(pWeaponHolder, pdummyWeaponUnk, target.Pointer) == 0x01;
            }
        }

        private Dictionary<EquipSlot, WeaponItem> GetWeapons()
        {
            Dictionary<EquipSlot, WeaponItem> weapons = new Dictionary<EquipSlot, WeaponItem>();

            IntPtr pWeaponHolder = (IntPtr)(*(MemStruct*)Pointer).WeaponHolder;

            if (pWeaponHolder == IntPtr.Zero)
                return weapons;

            IntPtr right = WeaponHolder_t.GetWeapon(pWeaponHolder, EquipSlot.Weap_RightHand, 0);

            if (right != IntPtr.Zero)
                weapons.Add(EquipSlot.Weap_RightHand, new WeaponItem(*(IntPtr*)(right + 0x14) + Offsets.RTTIDynamicCast.SimpleItem_t.n3Dynel_t, pWeaponHolder, right));

            IntPtr left = WeaponHolder_t.GetWeapon(pWeaponHolder, EquipSlot.Weap_LeftHand, 0);

            if (left != IntPtr.Zero)
                weapons.Add(EquipSlot.Weap_LeftHand, new WeaponItem(*(IntPtr*)(left + 0x14) + Offsets.RTTIDynamicCast.SimpleItem_t.n3Dynel_t, pWeaponHolder, left));

            return weapons;
        }

        private HashSet<SpecialAttack> GetSpecialAttacks()
        {
            HashSet<SpecialAttack> specials = new HashSet<SpecialAttack>();
            Dictionary<EquipSlot, WeaponItem> weapons = Weapons;

            if(weapons.Count > 0)
            {
                foreach (WeaponItem weapon in weapons.Values)
                {
                    foreach (SpecialAttack special in weapon.SpecialAttacks)
                    {
                        if (special == SpecialAttack.SneakAttack)
                        {
                            if (Profession == Profession.Adventurer || Profession == Profession.Shade)
                            {
                                if (DynelManager.LocalPlayer.GetStat(Stat.SneakAttack) >= 100)
                                {
                                    specials.Add(SpecialAttack.Backstab);
                                }
                            }
                        }
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

        private Buff[] GetBuffs()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return new Buff[0];

            Identity identity = Identity;
            return N3EngineClientAnarchy_t.GetNanoTemplateInfoList(pEngine, &identity)->ToList().Select(x => new Buff(Identity, (*(NanoTemplateInfoMemStruct*)x).Identity)).ToArray();
        }

        public bool IsInTeam()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return false;

            Identity identity = Identity;
            return N3EngineClientAnarchy_t.IsInTeam(pEngine, &identity);
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private new struct MemStruct
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

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct NanoTemplateInfoMemStruct
        {
            [FieldOffset(0x08)]
            public Identity Identity;

            //[FieldOffset(0x10)]
            //public int StartTime;

            //[FieldOffset(0x14)]
            //public int TotalDuration;
        }
    }
}
