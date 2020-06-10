using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using AOSharp.Core.Combat;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using AOSharp.Core.GameData;

namespace AOSharp.Core
{
    public class Buff : DummyItem
    {
        public readonly Identity Identity;
        public float RemainingTime => GetCurrentTime();
        public float TotalTime => GetTotalTime();

        internal unsafe Buff(Identity identity) : base(identity)
        {
            Identity = identity;
        }

        private unsafe float GetCurrentTime()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return 0;

            Identity none = Identity.None;
            fixed (Identity* pIdentity = &Identity)
            {
                return N3EngineClientAnarchy_t.GetBuffCurrentTime(pEngine, pIdentity, &none) / 100f;
            }
        }

        private unsafe float GetTotalTime()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return 0;

            Identity none = Identity.None;
            fixed (Identity* pIdentity = &Identity)
            {
                return N3EngineClientAnarchy_t.GetBuffTotalTime(pEngine, pIdentity, &none) / 100f;
            }
        }
    }
}
