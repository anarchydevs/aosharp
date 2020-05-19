using System;
using System.Linq;
using System.Collections.Generic;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Inventory
{
    public static class Inventory
    {
        public static List<Item> Items => GetItems(DynelManager.LocalPlayer.Identity);

        public static List<Container> Backpacks => Items.Where(x => x.ContainerIdentity.Type == IdentityType.Container).Select(x => new Container(x.Pointer, x.ContainerIdentity, x.Slot)).ToList();

        public static bool Find(int id, out Item item)
        {
            return (item = Items.FirstOrDefault(x => x.LowId == id || x.HighId == id)) != null;
        }

        public static bool Find(int lowId, int highId, out Item item)
        {
            return (item = Items.FirstOrDefault(x => x.LowId == lowId && x.HighId == highId)) != null;
        }

        public static List<Item> FindAll(int id)
        {
            return Items.Where(x => x.LowId == id || x.HighId == id).ToList();
        }

        public static List<Item> FindAll(int lowId, int highId)
        {
            return Items.Where(x => x.LowId == lowId && x.HighId == highId).ToList();
        }

        //This will likely be made internal once I provide a way of accessing the inventory of all types of containers.
        //For now just utilize this if you REALLY need the contents of something random like contracts (and i guess bank too)
        public unsafe static List<Item> GetItems(Identity container)
        {
            List<Item> items = new List<Item>();
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return items;

            IntPtr pItems = N3EngineClientAnarchy_t.GetInventoryVec(pEngine, &container);

            if (pItems == IntPtr.Zero)
                return items;

            int i = 0;
            foreach (IntPtr pItem in (*(StdStructVector*)pItems).ToList(sizeof(IntPtr)))
            {
                //Resolve proper type for item slot
                IdentityType slotType = IdentityType.None;

                switch (container.Type)
                {
                    case IdentityType.SimpleChar:
                        slotType = IdentityType.Inventory;

                        //Correct the slot type to match the equipment pages
                        if (i <= (int)EquipSlot.Weap_Deck6)
                            slotType = IdentityType.WeaponPage;
                        else if (i <= (int)EquipSlot.Cloth_LeftFinger)
                            slotType = IdentityType.ArmorPage;
                        else if (i <= (int)EquipSlot.Imp_Feet)
                            slotType = IdentityType.ImplantPage;
                        else if (i <= (int)EquipSlot.Social_LeftWeap)
                            slotType = IdentityType.SocialPage;

                        break;
                    case IdentityType.Bank:
                        slotType = IdentityType.BankByRef;
                        break;
                }

                IntPtr pActualItem = *(IntPtr*)pItem;

                if (pActualItem != IntPtr.Zero)
                    items.Add(new Item(pActualItem, new Identity(slotType, i)));

                i++;
            }

            return items;
        }
    }
}
