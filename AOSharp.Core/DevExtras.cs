using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOSharp.Core.GameData;
using System.IO;

namespace AOSharp.Core
{
    public static class DevExtras
    {
        //Packed with random tests. Don't invoke unless you want weird stuff to execute.
        public unsafe static void Test()
        {
            using (StreamWriter fileWriter = File.AppendText("Nanolines.txt"))
            {
                for (int i = 0; i < 100000; i++)
                {
                    StdString result = LDBFace.GetText(2009, i);
                    if (!result.ToString().Contains("no LDBintern"))
                        fileWriter.WriteLine($"{result.ToString()} = {i},");
                }
            }
        }

        //Loads all surfaces (Collision) for the current playfield. Used by me to generate navmeshes.
        public static void LoadAllSurfaces()
        {
            Playfield.Zones.ForEach(x => x.LoadSurface());
        }

        //Maps message keys to string. Not very reliable as it seems most aren't mapped by the func.
        public unsafe static string KeyToString(int key)
        {
            return (*N3InfoItemRemote_t.KeyToString(key)).ToString();
        }
    }
}
