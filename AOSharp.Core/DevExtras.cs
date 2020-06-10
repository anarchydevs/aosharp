using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AOSharp.Core
{
    public static class DevExtras
    {
        //Packed with random tests. Don't invoke unless you want weird stuff to execute.
        public unsafe static void ItemTemplateTest()
        {
            Identity none = Identity.None;
            IntPtr pEngine = N3Engine_t.GetInstance();
            IntPtr pItem = N3EngineClientAnarchy_t.GetItemByTemplate(pEngine, new Identity(IdentityType.ArmorPage, 0x13), &none);
            Chat.WriteLine(pItem.ToString("X4"));

            if (pItem == IntPtr.Zero)
                return;

            IntPtr pCriteria = N3EngineClientAnarchy_t.GetItemActionInfo(pItem, ItemActionInfo.UseCriteria);

            Chat.WriteLine(pCriteria.ToString("X4"));
        }

        public unsafe static void PerkTest()
        {
            Identity none = Identity.None;
            IntPtr pEngine = N3Engine_t.GetInstance();

            List<IntPtr> actions = N3EngineClientAnarchy_t.GetSpecialActionList(pEngine)->ToList();

            Chat.WriteLine($"NumActions {actions.Count}");
            foreach (IntPtr pAction in actions)
            {
                Chat.WriteLine($"\t {pAction.ToString("X4")}");
            }
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 0x1C)]
        private struct Perk
        {
            [FieldOffset(0x0)]
            public int Id;

            [FieldOffset(0x4)]
            public int Id2;
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
