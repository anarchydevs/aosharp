using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core
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
        public unsafe static extern void StopAttack(IntPtr pThis);

        //GetSkill
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetSkill@n3EngineClientAnarchy_t@@QBEHABVIdentity_t@@W4Stat_e@GameData@@H0@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern int GetSkill(IntPtr pThis, Identity* dynel, Stat stat, int detail, Identity* unk);

        //IsSecondarySpecialAttackAvailable
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsSecondarySpecialAttackAvailable@n3EngineClientAnarchy_t@@QBE_NW4Stat_e@GameData@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern byte IsSecondarySpecialAttackAvailable(IntPtr pThis, Stat stat);

        //GetAttackRange
        [DllImport("Gamecode.dll", EntryPoint = "?GetAttackRange@n3EngineClientAnarchy_t@@QBEMXZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern float GetAttackRange(IntPtr pThis);

        //CastNanoSpell
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_CastNanoSpell@n3EngineClientAnarchy_t@@QAEXABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern void CastNanoSpell(IntPtr pThis, Identity* nano, Identity* target);

        //IsAttacking
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsAttacking@n3EngineClientAnarchy_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern byte IsAttacking(IntPtr pThis);

        //GetSpecialActionList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetSpecialActionList@n3EngineClientAnarchy_t@@QAEPAV?$list@VSpecialAction_t@@V?$allocator@VSpecialAction_t@@@std@@@std@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern StdObjList* GetSpecialActionList(IntPtr pThis);

        //IsMoving
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_IsMoving@n3EngineClientAnarchy_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern void IsMoving(IntPtr pThis);

        //MovementChanged
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_MovementChanged@n3EngineClientAnarchy_t@@QAEXW4MovementAction_e@Movement_n@@MM_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void MovementChanged(IntPtr pThis, MovementAction action, float unk1, float unk2, bool unk3);

        //GetContainerInventoryList
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_GetContainerInventoryList@n3EngineClientAnarchy_t@@QBEPBV?$list@VInventoryEntry_t@@V?$allocator@VInventoryEntry_t@@@std@@@std@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetContainerInventoryList(IntPtr pThis, IntPtr identity);

        //TradeskillCombine
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_TradeskillCombine@n3EngineClientAnarchy_t@@QBEXABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr TradeskillCombine(IntPtr pThis, IntPtr source, IntPtr destination);

        //GetClientDynelId
        [DllImport("Gamecode.dll", EntryPoint = "?GetClientDynelId@n3EngineClientAnarchy_t@@UBE?AVIdentity_t@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern Identity* GetClientDynelId(IntPtr pThis);

        //SelectedTarget
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_SelectedTarget@n3EngineClientAnarchy_t@@QAEXABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern IntPtr SelectedTarget(IntPtr pThis, Identity* target);

        //TextCommand
        [DllImport("Gamecode.dll", EntryPoint = "?N3Msg_TextCommand@n3EngineClientAnarchy_t@@QAE_NHPBDABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr TextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate IntPtr DTextCommand(IntPtr pThis, IntPtr unk, IntPtr text, IntPtr identity);

        //B8 ? ? ? ? E8 ? ? ? ? 51 56 8B F1 83 BE ? ? ? ? ? 75 25 6A 18 E8 ? ? ? ? 59 8B C8 89 4D F0 83 65 FC 00 85 C9 74 08 56 E8 ? ? ? ? EB 02 
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal unsafe delegate StdObjList* GetMissionListDelegate(IntPtr pThis, IntPtr unk);
        internal static GetMissionListDelegate GetMissionList = Marshal.GetDelegateForFunctionPointer<GetMissionListDelegate>(Kernel32.GetModuleHandle("Gamecode.dll") + 0x58778);
    }
}
