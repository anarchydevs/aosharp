using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using AOSharp.Common.GameData;
using AOSharp.Core.Combat;
using AOSharp.Common.Unmanaged.Imports;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using AOSharp.Core.UI;

namespace AOSharp.Core
{
    public class Perk : DummyItem, ICombatAction, IEquatable<Perk>
    {
        private const float PERK_TIMEOUT = 1;

        public unsafe bool IsAvailable => !(*(SpecialActionMemStruct*)_pointer).IsOnCooldown;
        public bool IsPending => _pendingQueue.FirstOrDefault(x => x.Identity == Identity) != null;
        public bool IsExecuting => _executingQueue.FirstOrDefault(x => x.Identity == Identity) != null;
        public readonly Identity Identity;
        public readonly PerkHash Hash;
        private IntPtr _pointer;

        public static List<Perk> List => GetPerks();
        private static Queue<QueueItem> _pendingQueue = new Queue<QueueItem>();
        private static Queue<QueueItem> _executingQueue = new Queue<QueueItem>();

        private Perk(IntPtr pointer, Identity identity, int hashInt) : base(identity)
        {
            Identity = identity;
            _pointer = pointer;
            Hash = (PerkHash)hashInt;
        }

        public bool Use(bool packetOnly = false)
        {
            return Use(DynelManager.LocalPlayer, packetOnly);
        }

        public unsafe bool Use(SimpleChar target, bool packetOnly = false)
        {
            if (target != null)
                Targeting.SetTarget(target);

            if (packetOnly)
            {
                Network.Send(new CharacterActionMessage()
                {
                    Action = CharacterActionType.UsePerk,
                    Target = target.Identity,
                    Parameter1 = Identity.Instance,
                    Parameter2 = (int)Hash
                });

                EnqueuePendingPerk(this);

                return true;
            } else
            {
                IntPtr pEngine = N3Engine_t.GetInstance();

                if (pEngine == IntPtr.Zero)
                    return false;

                Identity identity = Identity;
                return N3EngineClientAnarchy_t.PerformSpecialAction(pEngine, &identity);
            }
        }

        public static bool Find(int id, out Perk perk)
        {
            return (perk = List.FirstOrDefault(x => x.Identity.Instance == id)) != null;
        }

        public static bool Find(string name, out Perk perk)
        {
            return (perk = List.FirstOrDefault(x => x.Name == name)) != null;
        }

        public static bool Find(PerkHash hash, out Perk perk)
        {
            return (perk = List.FirstOrDefault(x => x.Hash == hash)) != null;
        }

        private static void EnqueuePendingPerk(Perk perk)
        {
            _pendingQueue.Enqueue(new QueueItem
            {
                Identity = perk.Identity,
                AttackTime = perk.AttackDelay,
                Timeout = Time.NormalTime + PERK_TIMEOUT
            });
        }

        private static unsafe List<Perk> GetPerks()
        {
            List<Perk> perks = new List<Perk>();
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return perks;

            foreach (IntPtr pAction in N3EngineClientAnarchy_t.GetSpecialActionList(pEngine)->ToList())
            {
                SpecialActionMemStruct specialAction = *(SpecialActionMemStruct*)pAction;

                if (specialAction.Identity.Type != IdentityType.PerkHash)
                    continue;

                perks.Add(new Perk(pAction, specialAction.TemplateIdentity, specialAction.Identity.Instance));
            }

            return perks;
        }

        internal static void Update()
        {
            while(_pendingQueue.Count > 0 && _pendingQueue.Peek().Timeout <= Time.NormalTime)
                _pendingQueue.Dequeue();

            while (_executingQueue.Count > 0 && _executingQueue.Peek().Timeout <= Time.NormalTime)
                _executingQueue.Dequeue();
        }

        internal static void OnPerkFinished(int lowId, int highId, int ql, Identity owner)
        {
            //Will have to implement this some other way
            /*
            PerkExecuted?.Invoke(null, new PerkExecutedEventArgs
            {
                OwnerIdentity = owner,
                Owner = DynelManager.GetDynel(owner)?.Cast<SimpleChar>(),
                Perk = perk
            });
            */

            if (owner != DynelManager.LocalPlayer.Identity)
                return;

            DummyItem perkDummyItem = new DummyItem(lowId, highId, ql);

            _executingQueue.Dequeue();

            if (CombatHandler.Instance != null)
                CombatHandler.Instance.OnPerkExecuted(perkDummyItem);
        }

        internal static void OnPerkQueued()
        {
            Perk perk;
            if (_pendingQueue.Count == 0 || !Find(_pendingQueue.Dequeue().Identity.Instance, out perk))
                return;

            //Calc time offset of perks before this one in queue.
            float queueOffset = _executingQueue.Sum(x => x.AttackTime);
            double nextTimeout = Time.NormalTime + perk.AttackDelay + PERK_TIMEOUT + queueOffset;

            _executingQueue.Enqueue(new QueueItem
            {
                Identity = perk.Identity,
                AttackTime = perk.AttackDelay,
                Timeout = nextTimeout
            });

            if (CombatHandler.Instance != null)
                CombatHandler.Instance.OnPerkLanded(perk, nextTimeout);
        }

        private static void OnClientPerformedSpecialAction(Identity identity)
        {
            Perk perk;
            if (!Find(identity.Instance, out perk))
                return;

            EnqueuePendingPerk(perk);
        }

        public bool Equals(Perk other)
        {
            if (object.ReferenceEquals(other, null))
                return false;

            return Identity == other.Identity;
        }

        public static bool operator ==(Perk a, Perk b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);

            return a.Equals(b);
        }

        public static bool operator !=(Perk a, Perk b) => !(a == b);

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct SpecialActionMemStruct
        {
            [FieldOffset(0x8)]
            public Identity TemplateIdentity;

            [FieldOffset(0x10)]
            public Identity Identity;

            [FieldOffset(0x24)]
            public bool IsOnCooldown;
        }

        private class QueueItem
        {
            public Identity Identity;
            public float AttackTime;
            public double Timeout;
        }
    }

    public class PerkExecutedEventArgs : EventArgs
    {
        public SimpleChar Owner { get; set; }
        public Identity OwnerIdentity { get; set; }
        public Perk Perk { get; set; }
    }
}
