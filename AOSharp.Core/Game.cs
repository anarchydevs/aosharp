using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Core.UI;
using AOSharp.Common.Helpers;
using AOSharp.Core.Combat;
using AOSharp.Core.Inventory;
using System.Reflection;
using AOSharp.Core.IPC;
using SmokeLounge.AOtomation.Messaging.Exceptions;
using AOSharp.Core.Movement;

namespace AOSharp.Core
{
    public static class Game
    {
        public static bool IsZoning { get; private set; }
        public static int ClientInst => N3InterfaceModule_t.GetClientInst();

        public static EventHandler<float> OnEarlyUpdate;
        public static EventHandler<float> OnUpdate;
        public static EventHandler TeleportStarted;
        public static EventHandler TeleportEnded;
        public static EventHandler TeleportFailed; 
        public static EventHandler<uint> PlayfieldInit;

        private static unsafe void Init()
        {
            DummyItem_t.GetStat = Marshal.GetDelegateForFunctionPointer<DummyItem_t.GetStatDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 83 C1 34 8B 01 5D FF 60 04"));
            N3EngineClientAnarchy_t.GetItemActionInfo = Marshal.GetDelegateForFunctionPointer<N3EngineClientAnarchy_t.GetItemActionInfoDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 8B 49 6C 8D 45 08 50 E8 ? ? ? ? 8B 00 5D C2 04 00"));
            N3EngineClientAnarchy_t.GetMissionList = Marshal.GetDelegateForFunctionPointer<N3EngineClientAnarchy_t.GetMissionListDelegate>(Utils.FindPattern("Gamecode.dll", "B8 ? ? ? ? E8 ? ? ? ? 51 56 8B F1 83 BE ? ? ? ? ? 75 25 6A 18 E8 ? ? ? ? 59 8B C8 89 4D F0 83 65 FC 00 85 C9 74 08 56 E8 ? ? ? ? EB 02"));
            WeaponHolder_t.GetWeapon = Marshal.GetDelegateForFunctionPointer<WeaponHolder_t.GetWeaponDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 8B 45 08 56 8B F1 85 C0 78 05"));
            WeaponHolder_t.IsDynelInWeaponRange = Marshal.GetDelegateForFunctionPointer<WeaponHolder_t.IsDynelInWeaponRangeDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 83 EC 18 33 C0 56 8B F1 39 45 08 75 07"));
            WeaponHolder_t.IsInRange = Marshal.GetDelegateForFunctionPointer<WeaponHolder_t.IsInRangeDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 83 EC 18 56 8B F1 8B 4E 08 E8 ? ? ? ? 8B 48 5C 89 4D E8 8B 40 60 89 45 EC 8D 45 E8 50 FF 15 ? ? ? ? 59 89 45 F8 85 C0 0F 84"));
            WeaponHolder_t.GetDummyWeapon = Marshal.GetDelegateForFunctionPointer<WeaponHolder_t.GetDummyWeaponDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 51 56 8D 45 08 50 8B F1 8D 45 FC 50 8D 4E 0C E8 ? ? ? ? 8B 45 FC 3B 46 10 5E 74 05"));
            GamecodeUnk.AppendSystemText = Marshal.GetDelegateForFunctionPointer<GamecodeUnk.AppendSystemTextDelegate>(Utils.FindPattern("Gamecode.dll", "B8 ? ? ? ? E8 ? ? ? ? 83 EC 28 53 56 FF 15 ? ? ? ? FF 75 0C 33 DB 8D 4D CC 8B F0 C7 45 ? ? ? ? ? 89 5D DC 88 5D CC E8 ? ? ? ? 8D 86 ? ? ? ? 8B 08 89 5D FC 3B CB 74 75 89 45 EC A1 ? ? ? ? 89 4D E8 8B 08 89 4D F0 8D 4D E8 89 08 C6 45 FC 01 8B 4D E8 8B 41 14 89 45 E8 8B 41 0C 2B C3 74 32 48 74 25 48 74 14 48 75 2E FF 75 10 8B 01 8D 55 CC 52 FF 75 08 FF 50 04 EB 1D 8B 01 8D 55 CC 52 FF 75 08 FF 50 04 EB 0F FF 75 08"));
            ChatWindowNode_t.AppendText = Marshal.GetDelegateForFunctionPointer<ChatWindowNode_t.AppendTextDelegate>(Utils.FindPattern("GUI.dll", "B8 ? ? ? ? E8 ? ? ? ? 83 EC 70 53 56 57 8B F1 68 ? ? ? ? 8D 4D D8 FF 15 ? ? ? ? 33 DB 89 5D FC 39 5D 0C 74 6B FF 75 0C 8D 45 84 50 E8 ? ? ? ? 50 8D 45 A0 68 ? ? ? ? 50 C6 45 FC 01 E8 ? ? ? ? 83 C4 14 68 ? ? ? ? 8D 4D BC 51 8B C8 C6 45 FC 02 FF 15 ? ? ? ? 50 8D 4D D8 C6 45 FC 03 FF 15 ? ? ? ? 8D 4D BC C6 45 FC 02 FF 15 ? ? ? ? 8D 4D A0 C6 45 FC 01 FF 15 ? ? ? ? 8D 4D 84 88 5D FC FF 15 ? ? ? ? FF 75 08 8D 45 84 50 E8 ? ? ? ? 59 59 50 8D 4D D8 C6 45 FC 04 FF 15 ? ? ? ? 53 6A 01 8D 4D 84 88 5D FC E8 ? ? ? ? 8B 3D ? ? ? ? 39 5D 0C 74 0A 68 ? ? ? ? 8D 4D D8"));
            GUIUnk.LoadViewFromXml = Marshal.GetDelegateForFunctionPointer<GUIUnk.LoadViewFromXmlDelegate>(Utils.FindPattern("GUI.dll", "55 8B EC 56 FF 75 10 FF 75 0C FF 15 ? ? ? ? 6A 00 68 ? ? ? ? 68 ? ? ? ? 8B F0 6A 00 56 E8 ? ? ? ? 8B 4D 08 83 C4 1C 89 01 85 C0 75 10 85 F6 74 08 8B 06 6A 01 8B CE FF 10 32 C0 EB 02 B0 01 5E 5D C3"));

            int* pGetOptionWindowOffset = (int*)(Kernel32.GetProcAddress(Kernel32.GetModuleHandle("GUI.dll"), "?ModuleActivated@OptionPanelModule_c@@UAEX_N@Z") + 0x14);
            OptionPanelModule_c.GetOptionWindow = Marshal.GetDelegateForFunctionPointer<OptionPanelModule_c.GetOptionWindowDelegate>(new IntPtr((int)pGetOptionWindowOffset + sizeof(int) + *pGetOptionWindowOffset));
        }

        private static void OnPluginLoaded(Assembly assembly)
        {
            try
            {
                IPCChannel.LoadMessages(assembly);
            }
            catch (ContractIdCollisionException e)
            {
                Chat.WriteLine(e.Message);
            }
        }

        private static void OnEarlyUpdateInternal (float deltaTime)
        {
            OnEarlyUpdate?.Invoke(null, deltaTime);
        }

        private static void OnUpdateInternal(float deltaTime)
        {
            Network.Update();
            IPCChannel.Update();

            UIController.UpdateViews();

            Item.Update();
            Perk.Update();
            Spell.Update();

            MovementController.UpdateInternal();

            if (CombatHandler.Instance != null)
                CombatHandler.Instance.Update(deltaTime);

            OnUpdate?.Invoke(null, deltaTime);

            Chat.Update();
        }

        private static void OnTeleportStarted()
        {
            IsZoning = true;
            TeleportStarted?.Invoke(null, EventArgs.Empty);
        }

        private static void OnTeleportEnded()
        {
            IsZoning = false;
            TeleportEnded?.Invoke(null, EventArgs.Empty);
        }

        private static void OnTeleportFailed()
        {
            IsZoning = false;
            TeleportFailed?.Invoke(null, EventArgs.Empty);
        }

        private static void OnPlayfieldInit(uint id)
        {
            PlayfieldInit?.Invoke(null, id);
        }
    }
}
