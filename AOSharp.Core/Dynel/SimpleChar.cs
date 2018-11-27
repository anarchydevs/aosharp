using System;
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

        public SimpleChar(IntPtr pointer) : base(pointer)
        {
        }

        public SimpleChar(Dynel dynel) : base(dynel.Pointer)
        {
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
        }
    }
}
