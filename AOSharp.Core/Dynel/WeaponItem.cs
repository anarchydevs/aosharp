using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public unsafe class WeaponItem : SimpleItem
    {

        public WeaponItem(IntPtr pointer) : base(pointer)
        {
        }

        public WeaponItem(Dynel dynel) : base(dynel.Pointer)
        {
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private unsafe struct WeaponItem_MemStruct
        {

        }
    }
}
