using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;
using AOSharp.Core.UI;
using AOSharp.Core.GameData;
using AOSharp.Core.Combat;

namespace AOSharp.Core
{
    public static class Game
    {
        public static EventHandler<float> OnEarlyUpdate;
        public static EventHandler<float> OnUpdate;
        public static EventHandler TeleportStarted;
        public static EventHandler TeleportEnded;
        public static EventHandler TeleportFailed; 
        public static EventHandler<uint> PlayfieldInit;

        private unsafe static void Init()
        {
            DummyItem_t.GetStat = Marshal.GetDelegateForFunctionPointer<DummyItem_t.GetStatDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 83 C1 34 8B 01 5D FF 60 04"));
            N3EngineClientAnarchy_t.GetItemActionInfo = Marshal.GetDelegateForFunctionPointer<N3EngineClientAnarchy_t.GetItemActionInfoDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 8B 49 6C 8D 45 08 50 E8 ? ? ? ? 8B 00 5D C2 04 00"));
            N3EngineClientAnarchy_t.GetMissionList = Marshal.GetDelegateForFunctionPointer<N3EngineClientAnarchy_t.GetMissionListDelegate>(Utils.FindPattern("Gamecode.dll", "B8 ? ? ? ? E8 ? ? ? ? 51 56 8B F1 83 BE ? ? ? ? ? 75 25 6A 18 E8 ? ? ? ? 59 8B C8 89 4D F0 83 65 FC 00 85 C9 74 08 56 E8 ? ? ? ? EB 02"));
            WeaponHolder_t.GetWeapon = Marshal.GetDelegateForFunctionPointer<WeaponHolder_t.GetWeaponDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 8B 45 08 56 8B F1 85 C0 78 05"));
            WeaponHolder_t.IsDynelInWeaponRange = Marshal.GetDelegateForFunctionPointer<WeaponHolder_t.IsDynelInWeaponRangeDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 83 EC 18 33 C0 56 8B F1 39 45 08 75 07"));
            WeaponHolder_t.IsInRange = Marshal.GetDelegateForFunctionPointer<WeaponHolder_t.IsInRangeDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 83 EC 18 56 8B F1 8B 4E 08 E8 ? ? ? ? 8B 48 5C 89 4D E8 8B 40 60 89 45 EC 8D 45 E8 50 FF 15 ? ? ? ? 59 89 45 F8 85 C0 0F 84"));
            WeaponHolder_t.GetDummyWeapon = Marshal.GetDelegateForFunctionPointer<WeaponHolder_t.GetDummyWeaponDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 51 56 8D 45 08 50 8B F1 8D 45 FC 50 8D 4E 0C E8 ? ? ? ? 8B 45 FC 3B 46 10 5E 74 05"));
            GamecodeUnk.AppendSystemText = Marshal.GetDelegateForFunctionPointer<GamecodeUnk.AppendSystemTextDelegate>(Utils.FindPattern("Gamecode.dll", "B8 ? ? ? ? E8 ? ? ? ? 83 EC 28 53 56 FF 15 ? ? ? ? FF 75 0C 33 DB 8D 4D CC 8B F0 C7 45 ? ? ? ? ? 89 5D DC 88 5D CC E8 ? ? ? ? 8D 86 ? ? ? ? 8B 08 89 5D FC 3B CB 74 75 89 45 EC A1 ? ? ? ? 89 4D E8 8B 08 89 4D F0 8D 4D E8 89 08 C6 45 FC 01 8B 4D E8 8B 41 14 89 45 E8 8B 41 0C 2B C3 74 32 48 74 25 48 74 14 48 75 2E FF 75 10 8B 01 8D 55 CC 52 FF 75 08 FF 50 04 EB 1D 8B 01 8D 55 CC 52 FF 75 08 FF 50 04 EB 0F FF 75 08"));

            int* pGetOptionWindowOffset = (int*)(Kernel32.GetProcAddress(Kernel32.GetModuleHandle("GUI.dll"), "?ModuleActivated@OptionPanelModule_c@@UAEX_N@Z") + 0x14);
            OptionPanelModule_c.GetOptionWindow = Marshal.GetDelegateForFunctionPointer<OptionPanelModule_c.GetOptionWindowDelegate>(new IntPtr((int)pGetOptionWindowOffset + sizeof(int) + *pGetOptionWindowOffset));
        }

        private static void OnEarlyUpdateInternal (float deltaTime)
        {
            OnEarlyUpdate?.Invoke(null, deltaTime);
        }

        private static void OnUpdateInternal(float deltaTime)
        {
            Network.Update();

            UIController.UpdateViews();

            if (CombatHandler.Instance != null)
                CombatHandler.Instance.Update(deltaTime);

            Perk.Update();
            Spell.Update();

            OnUpdate?.Invoke(null, deltaTime);

            Chat.Update();
        }

        private static void OnTeleportStarted()
        {
            TeleportStarted?.Invoke(null, EventArgs.Empty);
        }

        private static void OnTeleportEnded()
        {
            TeleportEnded?.Invoke(null, EventArgs.Empty);
        }

        private static void OnTeleportFailed()
        {
            TeleportFailed?.Invoke(null, EventArgs.Empty);
        }

        private static void OnPlayfieldInit(uint id)
        {
            PlayfieldInit?.Invoke(null, id);
        }

        //TODO: Find a home for this.
        public static void SetMovement(MovementAction action)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return;

            N3EngineClientAnarchy_t.MovementChanged(pEngine, action, 0, 0, true);
        }
    }
}
