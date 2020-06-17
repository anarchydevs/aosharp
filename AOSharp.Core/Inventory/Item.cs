using AOSharp.Common.GameData;
using AOSharp.Core.Combat;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Core.Inventory
{
    public unsafe class Item : DummyItem, ICombatAction
    {
        public readonly int LowId;
        public readonly int HighId;
        public readonly int QualityLevel;
        public readonly Identity UniqueIdentity;
        public readonly Identity Slot;

        public static EventHandler<ItemUsedEventArgs> ItemUsed;

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

        public void Use()
        {
            Use(DynelManager.LocalPlayer);
        }

        public void Use(SimpleChar target)
        {
            Network.Send(new GenericCmdMessage()
            {
                Action = GenericCmdAction.Use,
                User = DynelManager.LocalPlayer.Identity,
                Target = target.Identity
            });
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

        internal static void OnItemUsed(int lowId, int highId, int ql, Identity owner)
        {
            ItemUsed?.Invoke(null, new ItemUsedEventArgs
            {
                OwnerIdentity = owner,
                Owner = DynelManager.GetDynel(owner)?.Cast<SimpleChar>(),
                Item = new Item(lowId, highId, ql, Identity.None, Identity.None)
            });
        }

        //Direct access to the MoveItemToInventory packet for those who need it.
        public static void MoveItemToInventory(Identity source, int slot)
        {
            Network.Send(new ClientMoveItemToInventory()
            {
                SourceContainer = source,
                Slot = slot
            });
        }

        //Direct access to the ContainerAddItem packet for those who need it.
        public static void ContainerAddItem(Identity source, Identity target)
        {
            Network.Send(new ClientContainerAddItem()
            {
                Source = source,
                Target = target
            });
        }
    }

    public class ItemUsedEventArgs : EventArgs
    {
        public SimpleChar Owner { get; set; }
        public Identity OwnerIdentity { get; set; }
        public Item Item { get; set; }
    }
}
