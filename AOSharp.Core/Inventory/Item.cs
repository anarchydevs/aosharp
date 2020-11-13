using AOSharp.Common.GameData;
using AOSharp.Core.Combat;
using SmokeLounge.AOtomation.Messaging.GameData;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Core.Inventory
{
    public class Item : DummyItem, ICombatAction, IEquatable<Item>
    {
        private const float USE_TIMEOUT = 1;

        public readonly int LowId;
        public readonly int HighId;
        public readonly int QualityLevel;
        public readonly Identity UniqueIdentity;
        public readonly Identity Slot;

        public static EventHandler<ItemUsedEventArgs> ItemUsed;

        public static bool HasPendingUse => _pendingUse.Slot != Identity.None;
        private static (Identity Slot, double Timeout) _pendingUse = (Identity.None, 0);

        internal Item(int lowId, int highId, int ql) : base(lowId, highId, ql)
        {
            LowId = lowId;
            HighId = highId;
            QualityLevel = ql;
            UniqueIdentity = Identity.None;
            Slot = Identity.None;
        }

        internal Item(int lowId, int highId, int ql, Identity uniqueIdentity, Identity slot) : base(slot)
        {
            LowId = lowId;
            HighId = highId;
            QualityLevel = ql;
            UniqueIdentity = uniqueIdentity;
            Slot = slot;
        }

        public void Equip(EquipSlot equipSlot)
        {
            MoveToInventory((int)equipSlot);
        }

        public void Use(SimpleChar target = null, bool setTarget = false)
        {
            if (target == null)
                target = DynelManager.LocalPlayer;

            if (setTarget)
                target.Target();

            Network.Send(new GenericCmdMessage()
            {
                Action = GenericCmdAction.Use,
                User = DynelManager.LocalPlayer.Identity,
                Target = Slot
            });

            _pendingUse = (Slot, Time.NormalTime + USE_TIMEOUT);
        }

        public void MoveToInventory(int targetSlot = 0x6F)
        {
            MoveItemToInventory(Slot, targetSlot);
        }

        public void MoveToContainer(Container target)
        {
            ContainerAddItem(Slot, target.Identity);
        }

        public void MoveToContainer(Identity target)
        {
            ContainerAddItem(Slot, target);
        }

        public void Split(int count)
        {
            Network.Send(new CharacterActionMessage()
            {
                Action = CharacterActionType.SplitItem,
                Target = Slot,
                Parameter2 = count
            });
        }

        internal static void Update()
        {
            if (_pendingUse.Slot != Identity.None && _pendingUse.Timeout <= Time.NormalTime)
                _pendingUse.Slot = Identity.None;
        }

        internal static void OnUsingItem(Identity slot)
        {
            if (slot != _pendingUse.Slot || !Inventory.Find(slot, out Item item))
                return;

            double nextTimeout = Time.NormalTime + item.AttackDelay + USE_TIMEOUT;
            _pendingUse = (slot, nextTimeout);

            if (CombatHandler.Instance != null)
                CombatHandler.Instance.OnUsingItem(item, nextTimeout);
        }

        internal static void OnItemUsed(int lowId, int highId, int ql, Identity owner)
        {
            ItemUsed?.Invoke(null, new ItemUsedEventArgs
            {
                OwnerIdentity = owner,
                Owner = DynelManager.GetDynel(owner)?.Cast<SimpleChar>(),
                Item = new Item(lowId, highId, ql)
            });

            if (owner != DynelManager.LocalPlayer.Identity)
                return;

            _pendingUse = (Identity.None, 0);

            if (CombatHandler.Instance != null)
                CombatHandler.Instance.OnItemUsed(lowId, highId, ql);
        }

        public static void MoveItemToInventory(Identity source, int slot)
        {
            Network.Send(new ClientMoveItemToInventory()
            {
                SourceContainer = source,
                Slot = slot
            });
        }

        public static void ContainerAddItem(Identity source, Identity target)
        {
            Network.Send(new ClientContainerAddItem()
            {
                Source = source,
                Target = target
            });
        }

        public static void SplitItem(Identity source, int count)
        {
            Network.Send(new CharacterActionMessage()
            {
                Action = CharacterActionType.SplitItem,
                Target = source,
                Parameter2 = count
            });
        }

        public static void Use(Identity slot)
        {
            Network.Send(new GenericCmdMessage()
            {
                Action = GenericCmdAction.Use,
                User = DynelManager.LocalPlayer.Identity,
                Target = slot
            });
        }

        public bool Equals(Item other)
        {
            if (object.ReferenceEquals(other, null))
                return false;

            return LowId == other.LowId && HighId == other.HighId && QualityLevel == other.QualityLevel && Slot == other.Slot;
        }

        public static bool operator ==(Item a, Item b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);

            return a.Equals(b);
        }

        public static bool operator !=(Item a, Item b) => !(a == b);
    }

    public class ItemUsedEventArgs : EventArgs
    {
        public SimpleChar Owner { get; set; }
        public Identity OwnerIdentity { get; set; }
        public Item Item { get; set; }
    }
}
