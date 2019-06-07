using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using AOSharp.Core.GameData;

namespace AOSharp.Core
{
    public static class Inventory
    {
        public unsafe static void Test(Identity container)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            IntPtr pItems = N3EngineClientAnarchy_t.GetInventoryVec(pEngine, &container);

            Chat.WriteLine($"Inv: {pItems.ToString("X4")}");


            if (pItems == IntPtr.Zero)
                return;

            StdStructVector items = *(StdStructVector*)pItems;

            foreach (IntPtr pItem in items.ToList(sizeof(IntPtr)))
            {
                Chat.WriteLine($"   pItem: {pItem.ToString("X4")}");
            }
        }
    }
}
