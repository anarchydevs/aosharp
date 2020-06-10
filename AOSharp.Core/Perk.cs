using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AOSharp.Common.GameData;
using AOSharp.Core.Combat;
using AOSharp.Core.Imports;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core
{
    public class Perk : DummyItem, ICombatAction
    {
        private const float PERK_QUEUE_TIMEOUT = 1;

        public readonly string Hash;
        public readonly float AttackTime = 1f; //TODO: Actually load this.
        public unsafe bool IsAvailable => !(*(SpecialActionMemStruct*)_pointer).IsOnCooldown;
        public bool IsQueued => _perkQueue.FirstOrDefault(x => x.Identity == Identity) != null;
        public readonly Identity Identity;
        private readonly int _hashInt;
        private IntPtr _pointer;

        public static List<Perk> List => GetPerks();
        private static Queue<QueueItem> _perkQueue = new Queue<QueueItem>();

        public static EventHandler<PerkExecutedEventArgs> PerkExecuted;

        private Perk(IntPtr pointer, Identity identity, int hashInt) : base(identity)
        {
            Identity = identity;
            _pointer = pointer;
            _hashInt = hashInt;
            Hash = Encoding.ASCII.GetString(BitConverter.GetBytes(hashInt).Reverse().ToArray());
        }

        public bool Use(bool packetOnly = false)
        {
            return Use(DynelManager.LocalPlayer, packetOnly);
        }

        public unsafe bool Use(SimpleChar target, bool packetOnly = false)
        {
            if (packetOnly)
            {
                Connection.Send(new CharacterActionMessage()
                {
                    Action = CharacterActionType.UsePerk,
                    Target = target.Identity,
                    Parameter1 = Identity.Instance,
                    Parameter2 = _hashInt
                });

                _perkQueue.Enqueue(new QueueItem {
                    Identity = Identity,
                    Timeout = Time.NormalTime + PERK_QUEUE_TIMEOUT
                });

                return true;
            } else
            {
                IntPtr pEngine = N3Engine_t.GetInstance();

                if (pEngine == IntPtr.Zero)
                    return false;

                Identity identity = Identity;
                return N3EngineClientAnarchy_t.PerformSpecialAction(pEngine, &identity) == 1;
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

        private static void EnqueuePerk(Identity identity)
        {
            _perkQueue.Enqueue(new QueueItem
            {
                Identity = identity,
                Timeout = Time.NormalTime + PERK_QUEUE_TIMEOUT
            });
        }

        private unsafe static List<Perk> GetPerks()
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

        internal static void OnUpdate(float deltaTime)
        {
            while(_perkQueue.Count > 0 && _perkQueue.Peek().Timeout <= Time.NormalTime)
                _perkQueue.Dequeue();
        }

        internal static void OnPerkFinished(int lowId, int highId, Identity owner)
        {
            Perk perk;
            if (!Find(highId, out perk))
                return;

            PerkExecuted?.Invoke(null, new PerkExecutedEventArgs
            {
                OwnerIdentity = owner,
                Owner = DynelManager.GetDynel(owner)?.Cast<SimpleChar>(),
                Perk = perk
            });

            if (owner != DynelManager.LocalPlayer.Identity)
                return;

            Identity dequeudPerk = _perkQueue.Dequeue().Identity;
            if (dequeudPerk.Instance != lowId && dequeudPerk.Instance != highId)
                Chat.WriteLine($"Perk queue desync {perk.Identity} != {dequeudPerk}");

            if (CombatHandler.Instance != null)
                CombatHandler.Instance.OnPerkExecuted(perk);
        }

        internal static void OnPerkQueued()
        {
            Perk perk;
            if (_perkQueue.Count == 0 || !Find(_perkQueue.Peek().Identity.Instance, out perk))
                return;

            _perkQueue.Peek().Timeout = Time.NormalTime + perk.AttackTime + PERK_QUEUE_TIMEOUT;
        }

        private static void OnClientPerformedPerk(Identity identity)
        {
            EnqueuePerk(identity);
        }

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
