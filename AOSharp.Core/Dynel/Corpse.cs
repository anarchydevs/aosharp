using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.Unmanaged.Interfaces;
using AOSharp.Core.Inventory;

namespace AOSharp.Core
{
    public class Corpse : Dynel
    {
        public bool IsOpen => GetIsOpen();
        public IEnumerable<Item> Items => Inventory.Inventory.GetContainerItems(Identity);

        public Corpse(IntPtr pointer) : base(pointer)
        {
        }
        
        public Corpse(Dynel dynel) : base(dynel.Pointer)
        {
        }

        public void Open()
        {
            N3EngineClientAnarchy.UseItem(Identity);
        }
        private bool GetIsOpen()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return false;

            Identity identity = Identity;
            IntPtr pItems = N3EngineClientAnarchy_t.GetInventoryVec(pEngine, ref identity);

            return pItems != IntPtr.Zero;
        }
    }
}
