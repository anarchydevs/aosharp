using System.Runtime.InteropServices;

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
