using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public unsafe class SimpleItem : Dynel
    {

        public SimpleItem(IntPtr pointer) : base(pointer)
        {
        }

        public SimpleItem(Dynel dynel) : base(dynel.Pointer)
        {
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private unsafe struct SimpleItem_MemStruct
        {

        }
    }
}
