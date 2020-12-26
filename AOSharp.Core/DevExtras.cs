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
        //Packed with random tests. Don't invoke unless you want weird stuff to execute.
        public static void Test()
        {
            DistributedValue.Create("Well_this_is_op", 1337);
            Window.CreateFromXml("Test", @"D:\2020Backup\Desktop\Test.xml").Show(true);
            //DistributedValue.LoadConfig(@"C:\Users\tagyo\AppData\Local\Funcom\Anarchy Online\cd94ae4f\Anarchy Online\Prefs\melatonin220\Char1108641443\Prefs.xml", 3, true);
            //DistributedValue.SaveConfig(@"C:\Users\tagyo\Desktop\test3.xml", 3);
            /*
            IntPtr pPrefs = IndependentPrefs__GetInstance();

            if (pPrefs == IntPtr.Zero)
                return;

            Chat.WriteLine(IndependentPrefs__Load(pPrefs, "Well_this_worked", 0));
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
            return StdString.FromPointer(N3InfoItemRemote_t.KeyToString(key)).ToString();
        }
    }
}
