using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Core.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Core.Inventory
{
    public unsafe class Container
    {
        public List<Item> Items => GetItems();
        public bool IsOpen => GetIsOpen();
        public readonly Identity Identity;
        public readonly Identity Slot;
        public IntPtr Pointer;

        internal Container(IntPtr pItem, Identity identity, Identity slot)
        {
            Pointer = pItem;
            Identity = identity;
            Slot = slot;
        }

        private bool GetIsOpen()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return false;

            Identity identity = Identity;
            IntPtr pItems = N3EngineClientAnarchy_t.GetInventoryVec(pEngine, &identity);

            return pItems != IntPtr.Zero;
        }

        public unsafe List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
 
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return items;

            Identity identity = Identity;

            IntPtr pInvList = N3EngineClientAnarchy_t.GetContainerInventoryList(pEngine, &identity);

            if (pInvList == IntPtr.Zero)
                return items;

            IntPtr pItems = N3EngineClientAnarchy_t.GetInventoryVec(pEngine, &identity);

            if (pItems == IntPtr.Zero)
                return items;

            List<IntPtr> containerInvList = (*(StdObjList*)pInvList).ToList();

            int i = 0;
            foreach (IntPtr pItem in (*(StdStructVector*)pItems).ToList(sizeof(IntPtr)))
            {
                IntPtr pActualItem = *(IntPtr*)pItem;

                if (pActualItem != IntPtr.Zero)
                {
                    Identity slot = *((Identity*)(containerInvList[i] + 0x8));
                    items.Add(new Item(pActualItem, slot));
                    i++;
                }
            }
            return items;
        }
    }
}
