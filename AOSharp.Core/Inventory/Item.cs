using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Core.Inventory
{
    public unsafe class Item
    {
        public int Unk1 => (*(ItemMemStruct*)Pointer).Unk1;
        public int Unk2 => (*(ItemMemStruct*)Pointer).Unk2;
        public int LowId => (*(ItemMemStruct*)Pointer).LowId;
        public int HighId => (*(ItemMemStruct*)Pointer).HighId;
        public int QualityLevel => (*(ItemMemStruct*)Pointer).QualityLevel;
        public Identity ContainerIdentity => (*(ItemMemStruct*)Pointer).ContainerIdentity;
        public readonly Identity Slot;
        public readonly IntPtr Pointer;

        internal Item(IntPtr pItem, Identity slot)
        {
            Pointer = pItem;
            Slot = slot;
        }

        public void Equip(EquipSlot equipSlot)
        {
            MoveToInventory((int)equipSlot);
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

        //Direct access to the MoveItemToInventory packet for those who need it.
        public static void MoveItemToInventory(Identity source, int slot)
        {
            Connection.Send(new ClientMoveItemToInventory()
            {
                SourceContainer = source,
                Slot = slot
            });
        }

        //Direct access to the ContainerAddItem packet for those who need it.
        public static void ContainerAddItem(Identity source, Identity target)
        {
            Connection.Send(new ClientContainerAddItem()
            {
                Source = source,
                Target = target
            });
        }


        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct ItemMemStruct
        {
            [FieldOffset(0x0)]
            public int Unk1; //Flags?

            [FieldOffset(0x04)]
            public Identity ContainerIdentity;

            [FieldOffset(0x0C)]
            public int LowId;

            [FieldOffset(0x10)]
            public int HighId;

            [FieldOffset(0x14)]
            public int QualityLevel;

            [FieldOffset(0x24)]
            public int Unk2; //Some other flags?
        }
    }
}
