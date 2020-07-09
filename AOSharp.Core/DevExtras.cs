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
            /*
            Constructor = Marshal.GetDelegateForFunctionPointer<TiXmlDocumentConstructorDelegate>(Utils.FindPattern("GUI.dll", "55 8B EC 6A FF 68 ? ? ? ? 64 A1 ? ? ? ? 50 51 53 56 57 A1 ? ? ? ? 33 C5 50 8D 45 F4 64 A3 ? ? ? ? 8B F1 89 75 F0 33 DB 53 E8 ? ? ? ? C7 06 ? ? ? ? C7 46 ? ? ? ? ? 89 5E 5C 89 5D FC 88 5E 4C 8B 55 08 83 C8 FF 89 46 70 89 46 6C 8B C2 8D 48 01 C6 45 FC 01 C7 46 ? ? ? ? ? 88 5E 74 89 4D 08 EB 03 8D 49 00 8A 08"));
            LoadFile = Marshal.GetDelegateForFunctionPointer<LoadXmlFileDelegate>(Utils.FindPattern("GUI.dll", "55 8B EC 6A FF 68 ? ? ? ? 64 A1 ? ? ? ? 50 83 EC 20 A1 ? ? ? ? 33 C5 89 45 F0 53 56 57 50 8D 45 F4 64 A3 ? ? ? ? 8B F9 8B 4D 08 33 DB 8B C1 C7 45 ? ? ? ? ? 89 5D E4 88 5D D4 8D 70 01 8A 10 40 3A D3 75 F9 2B C6 50 51 8D 4D D4 E8 ? ? ? ? 6A FF 53 8D 45 D4 8D 77 20 50 8B CE 89 5D FC E8 ? ? ? ? 83 7E 14 10 72 02 8B 36"));

            IntPtr pNew = MSVCR100.New(0x150);
            IntPtr pDoc = Constructor(pNew, "test");
            LoadFile(pNew, @"C:\Users\tagyo\Desktop\Test.xml", 0);
            */

            //IntPtr pNew = LoadXMLObject(StdString.Create(@"C:\Users\tagyo\Desktop\Test.xml"), StdString.Create(@""));

            //return pNew;
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
