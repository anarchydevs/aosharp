using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOSharp.Common.Helpers;
using AOSharp.Core.Inventory;
using AOSharp.Core.UI;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Core
{
    public static class DevExtras
    {
        [DllImport("Utils.dll", EntryPoint = "?AddVariable@DistributedValue_c@@SAXABVString@@ABVVariant@@_N2@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddVariable(IntPtr pPathStr, IntPtr pVariant, bool unk1, bool unk2);

        //Mali likes cute anime traps (Git Test Pls Ignore)

        //Packed with random tests. Don't invoke unless you want weird stuff to execute.
        public static void Test()
        {
            /*
            IntPtr pName = StdString.Create("Well_this_is_op");
            IntPtr pVariant = Variant.Create(1).Pointer;
            AddVariable(pName, pVariant, true, true);
            */
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
