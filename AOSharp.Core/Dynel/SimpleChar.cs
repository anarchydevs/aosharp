using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;

namespace AOSharp.Core
{
    public unsafe class SimpleChar : Dynel
    {
        public string Name => (*(SimpleChar_MemStruct*)Pointer).Name.ToString();

        public bool IsPlayer => !(*(SimpleChar_MemStruct*)Pointer).IsNPC;

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

            [FieldOffset(0x21C)]
            public bool IsNPC;
        }
    }
}
