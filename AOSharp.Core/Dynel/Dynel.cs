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

        public unsafe IntPtr VehiclePointer => new IntPtr((*(MemStruct*)Pointer).Vehicle);

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

        public unsafe float Radius => (*(MemStruct*)Pointer).Vehicle->Radius;

        public virtual unsafe bool IsMoving => (*(MemStruct*)Pointer).Vehicle->Velocity > 0f;

        protected unsafe bool IsPathing => (*(MemStruct*)Pointer).Vehicle->PathingDestination != Vector3.Zero;
        protected unsafe Vector3 PathingDestination => (*(MemStruct*)Pointer).Vehicle->PathingDestination;

        public virtual string Name => GetName();

        public bool IsValid => DynelManager.IsValid(this);

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

        private string GetName()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return string.Empty;

            Identity identity = Identity;
            Identity unk = new Identity();

            return Marshal.PtrToStringAnsi(N3EngineClientAnarchy_t.GetName(pEngine, ref identity, ref unk));
        }

        public float DistanceFrom(Dynel dynel)
        {
            return Vector3.Distance(Position, dynel.Position);
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
}
