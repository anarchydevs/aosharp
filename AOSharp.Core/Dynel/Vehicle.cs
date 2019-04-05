using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public unsafe struct Vehicle
    {
        [FieldOffset(0x38)]
        public float Runspeed;

        [FieldOffset(0x4C)]
        public float Radius;

        [FieldOffset(0x58)]
        public Vector3 Position;

        [FieldOffset(0x80)]
        public Quaternion Rotation;

        [FieldOffset(0xCC)]
        public float Velocity;

        [FieldOffset(0x178)]
        public CharMovementStatus* CharMovementStatus;
    }
}
