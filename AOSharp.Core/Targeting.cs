using System;
using System.Collections.Concurrent;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core
{
    public static class Targeting
    {
        public static void SelectSelf(bool packetOnly = false)
        {
            SetTarget(DynelManager.LocalPlayer);
        }

        public static void SetTarget(SimpleChar target, bool packetOnly = false)
        {
            SetTarget(target.Identity, packetOnly);
        }

        public static unsafe void SetTarget(Identity target, bool packetOnly = false)
        {
            if (!packetOnly)
            {
                TargetingModule_t.SetTarget(ref target, false);

                IntPtr pEngine = N3Engine_t.GetInstance();

                if (pEngine == IntPtr.Zero)
                    return;

                N3EngineClientAnarchy_t.SelectedTarget(pEngine, ref target);
            }
            else
            {
                Network.Send(new LookAtMessage()
                {
                    Target = target
                });
            }
        }
    }
}
