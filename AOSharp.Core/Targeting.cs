using System;
using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core
{
    public static class Targeting
    {
        public unsafe static void SetTarget(Identity target, bool packetOnly = false)
        {
            if (!packetOnly)
            {
                TargetingModule_t.SetTarget(&target, true);

                IntPtr pEngine = N3Engine_t.GetInstance();

                if (pEngine == IntPtr.Zero)
                    return;

                N3EngineClientAnarchy_t.SelectedTarget(pEngine, &target);
            }
            else
            {
                Connection.Send(new LookAtMessage()
                {
                    Target = target
                });
            }
        }

        public unsafe static void SelectSelf(bool packetOnly = false)
        {
            Identity self = DynelManager.LocalPlayer.Identity;

            if (!packetOnly)
            {
                IntPtr pTargetingModule = TargetingModule_t.GetInstanceIfAny();

                if (pTargetingModule == IntPtr.Zero)
                    return;

                TargetingModule_t.SelectSelf(pTargetingModule);

                IntPtr pEngine = N3Engine_t.GetInstance();

                if (pEngine == IntPtr.Zero)
                    return;

                N3EngineClientAnarchy_t.SelectedTarget(pEngine, &self);
            }
            else
            {
                Connection.Send(new LookAtMessage()
                {
                    Target = self
                });
            }
        }
    }
}
