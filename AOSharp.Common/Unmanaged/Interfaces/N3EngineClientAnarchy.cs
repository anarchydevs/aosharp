﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Common.Helpers;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Common.Unmanaged.Interfaces
{
    public class N3EngineClientAnarchy
    {

        public static string GetPFName(int id)
        {
            return Marshal.PtrToStringAnsi(N3EngineClientAnarchy_t.GetPFName(id));
        }

        public static string GetPerkName(int perkId, bool unk = false)
        {
            StdString retStr = StdString.Create();

            IntPtr pStr = N3EngineClientAnarchy_t.GetPerkName(retStr.Pointer, perkId, unk);

            if (pStr == IntPtr.Zero)
                return string.Empty;

            return retStr.ToString();
        }

        public static float GetPerkProgress(uint perkId)
        {
            IntPtr pInterfaceModule = N3InterfaceModule_t.GetInstance();

            if (pInterfaceModule == IntPtr.Zero)
            {
                return 0.0f;
            }

            return N3InterfaceModule_t.GetPerkProgress(pInterfaceModule, perkId);
        }

        public static List<uint> GetCompletedPersonalResearchGoals()
        {
            List<uint> completedGoals = new List<uint>();
            IntPtr pInterfaceModule = N3InterfaceModule_t.GetInstance();

            if (pInterfaceModule != IntPtr.Zero)
            {
                StdStructVector vector = new StdStructVector();
                N3InterfaceModule_t.GetCompletedPersonalResearchGoals(pInterfaceModule, ref vector);
                completedGoals = vector.ToList<uint>();
            }

            return completedGoals;
        }

        public static List<ResearchGoal> GetPersonalResearchGoals()
        {
            List<ResearchGoal> goals = new List<ResearchGoal>();
            IntPtr pInterfaceModule = N3InterfaceModule_t.GetInstance();

            if (pInterfaceModule != IntPtr.Zero)
            {
                StdStructVector vector = new StdStructVector();
                N3InterfaceModule_t.GetPersonalResearchGoals(pInterfaceModule, ref vector);
                goals = vector.ToList<ResearchGoal>();
            }

            return goals;
        }

        public static void DebugSpellListToChat(Identity identity, int unk, int spellList)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.DebugSpellListToChat(pEngine, unk, ref identity, spellList);
        }

        public static Identity TemplateIDToDynelID(Identity templateId)
        {
            N3EngineClientAnarchy_t.TemplateIDToDynelID(N3Engine_t.GetInstance(), out Identity dynelId, ref templateId);

            return dynelId;
        }

        public static string GetName(Identity identity)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return null;
            
            Identity garbage = new Identity();
            
            return Utils.UnsafePointerToString(N3EngineClientAnarchy_t.GetName(pEngine, ref identity, ref garbage));
        }

        public static bool HasPerk(int perkId)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                throw new NullReferenceException("Could not get N3Engine instance");

            return N3EngineClientAnarchy_t.HasPerk(pEngine, perkId);
        }
        
        public static void UseItem(Identity identity, bool unknown = false)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine != IntPtr.Zero)
            {
                N3EngineClientAnarchy_t.UseItem(pEngine, ref identity, unknown);
            }
        }
        
        public static void UseItemOnItem(Identity source, Identity target)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine != IntPtr.Zero)
            {
                N3EngineClientAnarchy_t.UseItemOnItem(pEngine, ref source, ref target);
            }
        }
        
        public static void UseItemOnCharacter(Identity source, Identity target)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine != IntPtr.Zero)
            {
                N3EngineClientAnarchy_t.UseItemOnCharacter(pEngine, ref source, ref target);
            }
        }

        public static bool GetQuestWorldPos(Identity mission, out Identity playfield, out Vector3 universePos, out Vector3 zonePos)
        {
            playfield = Identity.None;
            universePos = Vector3.Zero;
            zonePos = Vector3.Zero;
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return false;

            return N3EngineClientAnarchy_t.GetQuestWorldPos(pEngine, ref mission, ref playfield, ref universePos, ref zonePos);
        }

        public static int GetNumberOfFreeInventorySlots()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return 0;

            return N3EngineClientAnarchy_t.GetNumberOfFreeInventorySlots(pEngine);
        }

        public static void SetStat(Stat stat, int value)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine != IntPtr.Zero)
                N3EngineClientAnarchy_t.SetStat(pEngine, value, stat);
        }

        public static void NPCChatStartTrade(Identity self, Identity npc)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine != IntPtr.Zero)
            {
                N3EngineClientAnarchy_t.NPCChatStartTrade(pEngine, ref self, ref npc);
            }
        }

        public static void NPCChatAddTradeItem(Identity self, Identity npc, Identity item)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine != IntPtr.Zero)
            {
                N3EngineClientAnarchy_t.NPCChatAddTradeItem(pEngine, ref self, ref npc, ref item);
            }
        }

        public static void NPCChatEndTrade(Identity self, Identity npc, int credits = 0, bool accept = true)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine != IntPtr.Zero)
            {
                N3EngineClientAnarchy_t.NPCChatEndTrade(pEngine, ref self, ref npc, credits, accept);
            }
        }
    }
}
