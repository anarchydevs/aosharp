using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public class Dynel
    {
        public readonly IntPtr Pointer;

        public unsafe Identity Identity => (*(Dynel_MemStruct*)Pointer).Identity;

        public unsafe Vector3 Position
        {
            get { return (*(Dynel_MemStruct*)Pointer).Vehicle->Position; }
            set { (*(Dynel_MemStruct*)Pointer).Vehicle->Position = value; }
        }

        public unsafe Quaternion Rotation
        {
            get { return (*(Dynel_MemStruct*)Pointer).Vehicle->Rotation; }
            set { (*(Dynel_MemStruct*)Pointer).Vehicle->Rotation = value; }
        }

        public unsafe MovementState MovementState
        {
            get { return (*(Dynel_MemStruct*)Pointer).Vehicle->CharMovementStatus->State; }
            set { (*(Dynel_MemStruct*)Pointer).Vehicle->CharMovementStatus->State = value; }
        }

        public unsafe float Runspeed
        {
            get { return (*(Dynel_MemStruct*)Pointer).Vehicle->Runspeed; }
            set { (*(Dynel_MemStruct*)Pointer).Vehicle->Runspeed = value; }
        }

        public unsafe virtual bool IsMoving => (*(Dynel_MemStruct*)Pointer).Vehicle->Velocity > 0f;

        public Dynel(IntPtr pointer)
        {
            Pointer = pointer;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private unsafe struct Dynel_MemStruct
        {
            [FieldOffset(0x14)]
            public Identity Identity;

            [FieldOffset(0x50)]
            public Vehicle* Vehicle;
        }
    }

    public static class DynelExtensions
    {
        public static T Cast<T>(this Dynel dynel) where T : Dynel
        {
            return (T)Activator.CreateInstance(typeof(T), dynel.Pointer);
        }
    }
}
