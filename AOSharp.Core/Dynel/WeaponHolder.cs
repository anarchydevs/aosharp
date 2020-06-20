using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WeaponHolder
    {
        //2 if attacking otherwise 1
        [FieldOffset(0x44)]
        public byte AttackingState;
    }
}
