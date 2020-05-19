using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Core.Imports;

namespace AOSharp.Core
{
    public unsafe class LocalPlayer : SimpleChar
    {
        public Dictionary<Stat, Cooldown> Cooldowns => GetCooldowns();

        internal List<Mission> MissionList => GetMissionList();

        public float AttackRange => GetAttackRange();

        public LocalPlayer(IntPtr pointer) : base(pointer)
        {
        }

        public void Attack(Dynel target)
        {
            if (target.GetStat(Stat.Health) == 0)
                return;

            Attack(target.Identity);
        }

        //TODO: Silence this function to prevent flood of "Please wait until previous action has finished."
        public void Attack(Identity target)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.DefaultAttack(pEngine, &target, true);
        }

        public void StopAttack()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.StopAttack(pEngine);
        }

        public float GetAttackRange()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return 0f;

            return N3EngineClientAnarchy_t.GetAttackRange(pEngine);
        }

        public void CastNano(Identity nano, Dynel target)
        {
            //Targeting.SetTarget(target.Identity);
            CastNano(nano, target.Identity);
        }

        public void CastNano(Identity nano, Identity target)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.CastNanoSpell(pEngine, &nano, &target);
        }

        private Dictionary<Stat, Cooldown> GetCooldowns()
        {
            Dictionary<Stat, Cooldown> cooldowns = new Dictionary<Stat, Cooldown>();

            IntPtr pUnk = *(*(LocalPlayer_MemStruct*)Pointer).CooldownUnk;

            if (pUnk == IntPtr.Zero)
                return cooldowns;

            StdStructVector cooldownVector = *(StdStructVector*)pUnk;

            foreach(IntPtr pCooldown in cooldownVector.ToList(sizeof(Cooldown)))
            {
                Cooldown cooldown = *(Cooldown*)pCooldown;
                cooldowns.Add(cooldown.Stat, cooldown);
            }

            return cooldowns;
        }

        internal List<Mission> GetMissionList()
        {
            List<Mission> missions = new List<Mission>();

            IntPtr pUnk = (*(LocalPlayer_MemStruct*)Pointer).MissionUnk;

            if (pUnk == IntPtr.Zero)
                return missions;

            StdObjVector missionVector = *(StdObjVector*)(pUnk + 0x04);


            foreach (IntPtr pMission in missionVector.ToList())
            {
                Mission mission = new Mission(pMission);
                missions.Add(mission);
            }

            return missions;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private unsafe struct LocalPlayer_MemStruct
        {
            [FieldOffset(0x1BC)]
            public IntPtr* CooldownUnk;

            [FieldOffset(0x1C4)]
            public IntPtr MissionUnk;
        }
    }
}
