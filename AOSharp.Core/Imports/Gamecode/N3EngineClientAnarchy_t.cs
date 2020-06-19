using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core.Imports
{
    public class N3EngineClientAnarchy_t
    {
        //SecondarySpecialAttack
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_SecondarySpecialAttack@n3EngineClientAnarchy_t@@QBE_NABVIdentity_t@@W4Stat_e@GameData@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern bool SecondarySpecialAttack(IntPtr pThis, Identity* target, Stat stat);

        //DefaultAttack
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_DefaultAttack@n3EngineClientAnarchy_t@@QBEXABVIdentity_t@@_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern void DefaultAttack(IntPtr pThis, Identity* target, bool unk);

        //StopAttack
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_StopAttack@n3EngineClientAnarchy_t@@QBEXXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern void StopAttack(IntPtr pThis);

        //GetSkill
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetSkill@n3EngineClientAnarchy_t@@QBEHABVIdentity_t@@W4Stat_e@GameData@@H0@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern int GetSkill(IntPtr pThis, Identity* dynel, Stat stat, int detail, Identity* unk);

        //IsSecondarySpecialAttackAvailable
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsSecondarySpecialAttackAvailable@n3EngineClientAnarchy_t@@QBE_NW4Stat_e@GameData@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern byte IsSecondarySpecialAttackAvailable(IntPtr pThis, Stat stat);

        //GetAttackRange
        [DllImport("Gamecode.dll", EntryPoint = "?GetAttackRange@n3EngineClientAnarchy_t@@QBEMXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern float GetAttackRange(IntPtr pThis);

        //CastNanoSpell
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_CastNanoSpell@n3EngineClientAnarchy_t@@QAEXABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern void CastNanoSpell(IntPtr pThis, Identity* nano, Identity* target);

        //PerformSpecialAction
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_PerformSpecialAction@n3EngineClientAnarchy_t@@QAE_NABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern byte PerformSpecialAction(IntPtr pThis, Identity* action);

        //GetName
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetName@n3EngineClientAnarchy_t@@QBEPBDABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern IntPtr GetName(IntPtr pThis, Identity* identity, Identity* identityUnk);

        //IsFormulaReady
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsFormulaReady@n3EngineClientAnarchy_t@@QBE_NABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern byte IsFormulaReady(IntPtr pThis, Identity* identity);

        //HasPerk
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_HasPerk@n3EngineClientAnarchy_t@@QAE_NI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern byte HasPerk(IntPtr pThis, int perkId);

        //IsAttacking
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsAttacking@n3EngineClientAnarchy_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern byte IsAttacking(IntPtr pThis);

        //GetSpecialActionList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetSpecialActionList@n3EngineClientAnarchy_t@@QAEPAV?$list@VSpecialAction_t@@V?$allocator@VSpecialAction_t@@@std@@@std@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern StdObjList* GetSpecialActionList(IntPtr pThis);

        //GetNanoSpellList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetNanoSpellList@n3EngineClientAnarchy_t@@QAEPBV?$list@HV?$allocator@H@std@@@std@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern StdObjList* GetNanoSpellList(IntPtr pThis);

        //GetNanoTemplateInfoList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetNanoTemplateInfoList@n3EngineClientAnarchy_t@@QBEPAV?$list@VNanoTemplateInfo_c@@V?$allocator@VNanoTemplateInfo_c@@@std@@@std@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern StdObjList* GetNanoTemplateInfoList(IntPtr pThis, Identity* identity);

        //IsMoving
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsMoving@n3EngineClientAnarchy_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern void IsMoving(IntPtr pThis);

        //MovementChanged
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_MovementChanged@n3EngineClientAnarchy_t@@QAEXW4MovementAction_e@Movement_n@@MM_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void MovementChanged(IntPtr pThis, MovementAction action, float unk1, float unk2, bool unk3);

        //GetContainerInventoryList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetContainerInventoryList@n3EngineClientAnarchy_t@@QBEPBV?$list@VInventoryEntry_t@@V?$allocator@VInventoryEntry_t@@@std@@@std@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern IntPtr GetContainerInventoryList(IntPtr pThis, Identity* identity);

        //GetInventoryVec
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetInventoryVec@n3EngineClientAnarchy_t@@QAEPBV?$vector@PAVNewInventoryEntry_t@@V?$allocator@PAVNewInventoryEntry_t@@@std@@@std@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern IntPtr GetInventoryVec(IntPtr pThis, Identity* identity);

        //IsInTeam
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsInTeam@n3EngineClientAnarchy_t@@QBE_NABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern bool IsInTeam(IntPtr pThis, Identity* identity);

        //TradeskillCombine
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_TradeskillCombine@n3EngineClientAnarchy_t@@QBEXABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr TradeskillCombine(IntPtr pThis, IntPtr source, IntPtr destination);

        //GetClientDynelId
        [DllImport("Gamecode.dll", EntryPoint = "?GetClientDynelId@n3EngineClientAnarchy_t@@UBE?AVIdentity_t@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern Identity* GetClientDynelId(IntPtr pThis);

        //SelectedTarget
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_SelectedTarget@n3EngineClientAnarchy_t@@QAEXABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern IntPtr SelectedTarget(IntPtr pThis, Identity* target);

        //IsInRaidTeam
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsInRaidTeam@n3EngineClientAnarchy_t@@QAE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern byte IsInRaidTeam(IntPtr pThis);

        //GetTeamMemberList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetTeamMemberList@n3EngineClientAnarchy_t@@QAEPAV?$vector@PAVTeamEntry_t@@V?$allocator@PAVTeamEntry_t@@@std@@@std@@H@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern StdObjVector* GetTeamMemberList(IntPtr pThis, int teamIndex);

        //GetFullPerkMap
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetFullPerkMap@n3EngineClientAnarchy_t@@QBEABV?$vector@VPerk_t@@V?$allocator@VPerk_t@@@std@@@std@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern IntPtr GetFullPerkMap(IntPtr pThis);

        //IsTeamLeader
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsTeamLeader@n3EngineClientAnarchy_t@@QBE_NABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern byte IsTeamLeader(IntPtr pThis, Identity* target);

        //GetItemByTemplate
        [DllImport("Gamecode.dll", EntryPoint = "?GetItemByTemplate@n3EngineClientAnarchy_t@@ABEPAVDummyItemBase_t@@VIdentity_t@@ABV3@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern IntPtr GetItemByTemplate(IntPtr pThis, Identity template, Identity* unk);

        //GetBuffCurrentTime
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetBuffCurrentTime@n3EngineClientAnarchy_t@@QAEHABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern int GetBuffCurrentTime(IntPtr pThis, Identity* identity, Identity* unk);

        //GetBuffTotalTime
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetBuffTotalTime@n3EngineClientAnarchy_t@@QAEHABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern int GetBuffTotalTime(IntPtr pThis, Identity* identity, Identity* unk);

        //CreateDummyItemID
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_CreateDummyItemID@n3EngineClientAnarchy_t@@QBE_NAAVIdentity_t@@ABVACGItem_t@GameData@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern byte CreateDummyItemID(IntPtr pThis, Identity* template, ACGItem* acgItem);

        //TextCommand
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_TextCommand@n3EngineClientAnarchy_t@@QAE_NHPBDABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr TextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate IntPtr DTextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate StdObjList* GetMissionListDelegate(IntPtr pThis, IntPtr unk);
        internal static GetMissionListDelegate GetMissionList;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate IntPtr GetItemActionInfoDelegate(IntPtr pThis, ItemActionInfo action);
        internal static GetItemActionInfoDelegate GetItemActionInfo;
    }
}
