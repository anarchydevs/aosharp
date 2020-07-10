using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class N3EngineClientAnarchy_t
    {
        //SecondarySpecialAttack
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_SecondarySpecialAttack@n3EngineClientAnarchy_t@@QBE_NABVIdentity_t@@W4Stat_e@GameData@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe bool SecondarySpecialAttack(IntPtr pThis, Identity* target, Stat stat);

        //DefaultAttack
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_DefaultAttack@n3EngineClientAnarchy_t@@QBEXABVIdentity_t@@_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe void DefaultAttack(IntPtr pThis, Identity* target, bool unk);

        //StopAttack
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_StopAttack@n3EngineClientAnarchy_t@@QBEXXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern void StopAttack(IntPtr pThis);

        //GetSkill
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetSkill@n3EngineClientAnarchy_t@@QBEHABVIdentity_t@@W4Stat_e@GameData@@H0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe int GetSkill(IntPtr pThis, Identity* dynel, Stat stat, int detail, Identity* unk);

        //IsSecondarySpecialAttackAvailable
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsSecondarySpecialAttackAvailable@n3EngineClientAnarchy_t@@QBE_NW4Stat_e@GameData@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool IsSecondarySpecialAttackAvailable(IntPtr pThis, Stat stat);

        //GetAttackRange
        [DllImport("Gamecode.dll", EntryPoint = "?GetAttackRange@n3EngineClientAnarchy_t@@QBEMXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern float GetAttackRange(IntPtr pThis);

        //CastNanoSpell
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_CastNanoSpell@n3EngineClientAnarchy_t@@QAEXABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe void CastNanoSpell(IntPtr pThis, Identity* nano, Identity* target);
        public unsafe delegate void DCastNanoSpell(IntPtr pThis, Identity* nanoIdentity, Identity targetIdentity);

        //GetCorrectActionId
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetCorrectActionID@n3EngineClientAnarchy_t@@QBEXAAVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe void GetCorrectActionId(IntPtr pThis, Identity* id);

        //PerformSpecialAction
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_PerformSpecialAction@n3EngineClientAnarchy_t@@QAE_NABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe bool PerformSpecialAction(IntPtr pThis, Identity* action);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate bool DPerformSpecialAction(IntPtr pThis, IntPtr identity);

        //GetName
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetName@n3EngineClientAnarchy_t@@QBEPBDABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe IntPtr GetName(IntPtr pThis, Identity* identity, Identity* identityUnk);

        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetName@n3EngineClientAnarchy_t@@QBEPBDABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe IntPtr GetName(IntPtr pThis, ref Identity identity, ref Identity identityUnk);

        //IsFormulaReady
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsFormulaReady@n3EngineClientAnarchy_t@@QBE_NABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe bool IsFormulaReady(IntPtr pThis, Identity* identity);

        //HasPerk
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_HasPerk@n3EngineClientAnarchy_t@@QAE_NI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool HasPerk(IntPtr pThis, int perkId);

        //IsAttacking
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsAttacking@n3EngineClientAnarchy_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool IsAttacking(IntPtr pThis);

        //GetSpecialActionList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetSpecialActionList@n3EngineClientAnarchy_t@@QAEPAV?$list@VSpecialAction_t@@V?$allocator@VSpecialAction_t@@@std@@@std@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe StdObjList* GetSpecialActionList(IntPtr pThis);

        //GetNanoSpellList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetNanoSpellList@n3EngineClientAnarchy_t@@QAEPBV?$list@HV?$allocator@H@std@@@std@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe StdObjList* GetNanoSpellList(IntPtr pThis);

        //GetNanoTemplateInfoList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetNanoTemplateInfoList@n3EngineClientAnarchy_t@@QBEPAV?$list@VNanoTemplateInfo_c@@V?$allocator@VNanoTemplateInfo_c@@@std@@@std@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe StdObjList* GetNanoTemplateInfoList(IntPtr pThis, Identity* identity);

        //IsMoving
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsMoving@n3EngineClientAnarchy_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern void IsMoving(IntPtr pThis);

        //MovementChanged
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_MovementChanged@n3EngineClientAnarchy_t@@QAEXW4MovementAction_e@Movement_n@@MM_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void MovementChanged(IntPtr pThis, MovementAction action, float unk1, float unk2, bool unk3);

        //GetContainerInventoryList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetContainerInventoryList@n3EngineClientAnarchy_t@@QBEPBV?$list@VInventoryEntry_t@@V?$allocator@VInventoryEntry_t@@@std@@@std@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe IntPtr GetContainerInventoryList(IntPtr pThis, Identity* identity);

        //GetInventoryVec
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetInventoryVec@n3EngineClientAnarchy_t@@QAEPBV?$vector@PAVNewInventoryEntry_t@@V?$allocator@PAVNewInventoryEntry_t@@@std@@@std@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe IntPtr GetInventoryVec(IntPtr pThis, Identity* identity);

        //IsInTeam
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsInTeam@n3EngineClientAnarchy_t@@QBE_NABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe bool IsInTeam(IntPtr pThis, Identity* identity);

        //TradeskillCombine
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_TradeskillCombine@n3EngineClientAnarchy_t@@QBEXABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr TradeskillCombine(IntPtr pThis, IntPtr source, IntPtr destination);

        //GetClientDynelId
        [DllImport("Gamecode.dll", EntryPoint = "?GetClientDynelId@n3EngineClientAnarchy_t@@UBE?AVIdentity_t@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe Identity* GetClientDynelId(IntPtr pThis);

        //SelectedTarget
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_SelectedTarget@n3EngineClientAnarchy_t@@QAEXABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe IntPtr SelectedTarget(IntPtr pThis, Identity* target);

        //IsInRaidTeam
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsInRaidTeam@n3EngineClientAnarchy_t@@QAE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool IsInRaidTeam(IntPtr pThis);

        //GetTeamMemberList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetTeamMemberList@n3EngineClientAnarchy_t@@QAEPAV?$vector@PAVTeamEntry_t@@V?$allocator@PAVTeamEntry_t@@@std@@@std@@H@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe StdObjVector* GetTeamMemberList(IntPtr pThis, int teamIndex);

        //GetFullPerkMap
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetFullPerkMap@n3EngineClientAnarchy_t@@QBEABV?$vector@VPerk_t@@V?$allocator@VPerk_t@@@std@@@std@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetFullPerkMap(IntPtr pThis);

        //IsTeamLeader
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsTeamLeader@n3EngineClientAnarchy_t@@QBE_NABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe bool IsTeamLeader(IntPtr pThis, Identity* target);

        //GetItemByTemplate
        [DllImport("Gamecode.dll", EntryPoint = "?GetItemByTemplate@n3EngineClientAnarchy_t@@ABEPAVDummyItemBase_t@@VIdentity_t@@ABV3@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe IntPtr GetItemByTemplate(IntPtr pThis, Identity template, Identity* unk);

        //GetBuffCurrentTime
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetBuffCurrentTime@n3EngineClientAnarchy_t@@QAEHABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe int GetBuffCurrentTime(IntPtr pThis, Identity* identity, Identity* unk);

        //GetBuffTotalTime
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetBuffTotalTime@n3EngineClientAnarchy_t@@QAEHABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe int GetBuffTotalTime(IntPtr pThis, Identity* identity, Identity* unk);

        //CreateDummyItemID
        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_CreateDummyItemID@n3EngineClientAnarchy_t@@QBE_NAAVIdentity_t@@ABVACGItem_t@GameData@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern unsafe bool CreateDummyItemID(IntPtr pThis, Identity* template, ACGItem* acgItem);

        //TextCommand
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_TextCommand@n3EngineClientAnarchy_t@@QAE_NHPBDABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr TextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate IntPtr DTextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public unsafe delegate StdObjList* GetMissionListDelegate(IntPtr pThis, IntPtr unk);
        public static GetMissionListDelegate GetMissionList;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate IntPtr GetItemActionInfoDelegate(IntPtr pThis, ItemActionInfo action);
        public static GetItemActionInfoDelegate GetItemActionInfo;

        [DllImport("N3.dll", EntryPoint = "?GetPlayfield@n3EngineClient_t@@SAPAVn3Playfield_t@@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetPlayfield();

        [DllImport("Gamecode.dll", EntryPoint = "?RunEngine@n3EngineClientAnarchy_t@@UAEXM@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void RunEngine(IntPtr pThis, float unk);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DRunEngine(IntPtr pThis, float unk);

        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_SendInPlayMessage@n3EngineClientAnarchy_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool SendInPlayMessage(IntPtr pThis);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate bool DSendInPlayMessage(IntPtr pThis);

        [DllImport("Gamecode.dll", EntryPoint = "?PlayfieldInit@n3EngineClientAnarchy_t@@UAEXI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void PlayfieldInit(IntPtr pThis, uint id);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DPlayfieldInit(IntPtr pThis, uint id);

        [return: MarshalAs(UnmanagedType.U1)]
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsPerk@n3EngineClientAnarchy_t@@QBE_NI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern bool IsPerk(IntPtr pThis, int id);
    }
}
