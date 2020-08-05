using System;
using System.Collections.Concurrent;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core
{
    public static class Targeting
    {
        private static ConcurrentQueue<Identity> _targetQueue = new ConcurrentQueue<Identity>();

        internal static void Update()
        {
            while (_targetQueue.TryDequeue(out Identity target))
                SetTargetInternal(target);
        }

        private static unsafe void SetTargetInternal(Identity target)
        {
            TargetingModule_t.SetTarget(&target, false);

            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.SelectedTarget(pEngine, &target);
        }

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
                _targetQueue.Enqueue(target);
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
