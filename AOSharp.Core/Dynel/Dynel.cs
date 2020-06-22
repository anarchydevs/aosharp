using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Core
{
    public class Dynel
    {
        public readonly IntPtr Pointer;

        public unsafe Identity Identity => (*(MemStruct*)Pointer).Identity;

        public DynelFlags Flags => (DynelFlags)GetStat(Stat.Flags);

        public unsafe Vector3 Position
        {
            get => (*(MemStruct*)Pointer).Vehicle->Position;
            set => (*(MemStruct*)Pointer).Vehicle->Position = value;
        }

        public unsafe Quaternion Rotation
        {
            get => (*(MemStruct*)Pointer).Vehicle->Rotation;
            set => N3Dynel_t.SetRelRot(Pointer, &value);
        }

        public unsafe MovementState MovementState
        {
            get => (*(MemStruct*)Pointer).Vehicle->CharMovementStatus->State;
            set => (*(MemStruct*)Pointer).Vehicle->CharMovementStatus->State = value;
        }

        public unsafe float Runspeed
        {
            get => (*(MemStruct*)Pointer).Vehicle->Runspeed;
            set => (*(MemStruct*)Pointer).Vehicle->Runspeed = value;
        }

        public virtual unsafe bool IsMoving => (*(MemStruct*)Pointer).Vehicle->Velocity > 0f;

        public unsafe float Radius => (*(MemStruct*)Pointer).Vehicle->Radius;

        public Dynel(IntPtr pointer)
        {
            Pointer = pointer;
        }
        
        public void Target()
        {
            Targeting.SetTarget(Identity);
        }

        public unsafe int GetStat(Stat stat, int detail = 2)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return 0;

            //Copy identity
            Identity identity = Identity;
            Identity unk = new Identity();

            return N3EngineClientAnarchy_t.GetSkill(pEngine, &identity, stat, detail, &unk);
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        protected unsafe struct MemStruct
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
