using System;
using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core
{
    public static class Tageting
    {
        public unsafe static void SetTarget(Identity target, bool packetOnly = false)
        {
            if (!packetOnly)
            {
                IntPtr pTargetingModule = TargetingModule_t.GetInstanceIfAny();

                if (pTargetingModule == IntPtr.Zero)
                    return;

                TargetingModule_t.SetTarget(pTargetingModule, &target, true);
            }
            else
            {
                Connection.Send(new LookAtMessage()
                {
                    Target = target
                });
            }
        }
    }
}
