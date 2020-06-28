using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOSharp.Common.Helpers;
using System.IO;
using AOSharp.Core.Inventory;

namespace AOSharp.Core
{
    public static class DevExtras
    {
        //Packed with random tests. Don't invoke unless you want weird stuff to execute.
        public static void Test()
        {
            DummyItem flurryDummyItem = new Item(85907, 85908, 104);
            Chat.WriteLine($"{flurryDummyItem.Name} pActivate: {flurryDummyItem.GetItemActionInfo(ItemActionInfo.Activate)}");
        }

        //Loads all surfaces (Collision) for the current playfield. Used by me to generate navmeshes.
        public static void LoadAllSurfaces()
        {
            Playfield.Zones.ForEach(x => x.LoadSurface());
        }

        //Maps message keys to string. Not very reliable as it seems most aren't mapped by the func.
        public static unsafe string KeyToString(int key)
        {
            return (*N3InfoItemRemote_t.KeyToString(key)).ToString();
        }
    }
}
