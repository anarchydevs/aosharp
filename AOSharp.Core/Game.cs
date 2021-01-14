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
            GamecodeUnk.FollowTarget = Marshal.GetDelegateForFunctionPointer<GamecodeUnk.FollowTargetDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 83 EC 24 53 56 8B D9 8B 83 ? ? ? ? 57 8B 7D 08 3B C7 0F 84 ? ? ? ? 83 A3 ? ? ? ? ? 85 C0 74 32 E8 ? ? ? ? 8B B3 ? ? ? ? 83 C0 14 50 8D 4E 14 E8 ? ? ? ? 84 C0 75 17 8D 83 ? ? ? ? 50 8D 4E 04 FF 15 ? ? ? ? 83 A3 ? ? ? ? ? 89 BB ? ? ? ? 85 FF 74 66 8B CB E8 ? ? ? ? 8B B3 ? ? ? ? 83 C0 14 50 8D 4E 14 E8 ? ? ? ? 84 C0 75 10 8D 83 ? ? ? ? 50 8D 4E 04"));
            if(Kernel32.GetModuleHandle("Cheetah.dll") == IntPtr.Zero)
                GamecodeUnk.AppendSystemText = Marshal.GetDelegateForFunctionPointer<GamecodeUnk.AppendSystemTextDelegate>(Utils.FindPattern("Gamecode.dll", "B8 ? ? ? ? E8 ? ? ? ? 83 EC 28 53 56 FF 15 ? ? ? ? FF 75 0C 33 DB 8D 4D CC 8B F0 C7 45 ? ? ? ? ? 89 5D DC 88 5D CC E8 ? ? ? ? 8D 86 ? ? ? ? 8B 08 89 5D FC 3B CB 74 75 89 45 EC A1 ? ? ? ? 89 4D E8 8B 08 89 4D F0 8D 4D E8 89 08 C6 45 FC 01 8B 4D E8 8B 41 14 89 45 E8 8B 41 0C 2B C3 74 32 48 74 25 48 74 14 48 75 2E FF 75 10 8B 01 8D 55 CC 52 FF 75 08 FF 50 04 EB 1D 8B 01 8D 55 CC 52 FF 75 08 FF 50 04 EB 0F FF 75 08"));
            else
                GamecodeUnk.AppendSystemText = Marshal.GetDelegateForFunctionPointer<GamecodeUnk.AppendSystemTextDelegate>(Utils.FindPattern("Gamecode.dll", "B8 ? ? ? ? E8 ? ? ? ? 83 EC 28 53 56 FF 15 ? ? ? ? FF 75 0C 33 DB 8D 4D CC 8B F0 C7 45 ? ? ? ? ? 89 5D DC 88 5D CC E8 ? ? ? ? 8D 86 ? ? ? ? 8B 08 89 5D FC 3B CB 0F 84 ? ? ? ? 89 45 EC A1 ? ? ? ? 89 4D E8 8B 08 89 4D F0 8D 4D E8 89 08 C6 45 FC 01 8B 4D E8 8B F1 FF 15 ? ? ? ? 8B CE 89 45 E8 FF 15 ? ? ? ? 2B C3 74 38 48 74 29 48 74 16 48 75 36 FF 75 10 8B 06 8D 4D CC 51 FF 75 08 8B CE FF 50 04 EB 23 8B 06 8D 4D CC 51 FF 75 08 8B CE FF 50 04 EB 13"));
            GamecodeUnk.IsInLineOfSight = Marshal.GetDelegateForFunctionPointer<GamecodeUnk.IsInLineOfSightDelegate>(Utils.FindPattern("Gamecode.dll", "55 8B EC 83 EC 30 56 8B 75 08 57 8B F9 85 F6 75 07 32 C0 E9 ? ? ? ? 3B F7 75 07 B0 01 E9 ? ? ? ? 8B 46 60 3B 47 60 75 E6 FF 15 ? ? ? ? 8B C8 89 4D 08 85 C9 74 D7 80 BF ? ? ? ? ? 74 13 80 BE ? ? ? ? ? 74 0A FF 15 ? ? ? ? 84 C0 74 C6 D9 EE 8B 4E 50 8B 35 ? ? ? ? D9 55 F4 D9 05 ? ? ? ? 53 D9 5D F8 D9 5D FC FF D6 D9 EE 8B 4F 50 D9 55 E8 8B D8 D9 05"));
            ChatWindowNode_t.AppendText = Marshal.GetDelegateForFunctionPointer<ChatWindowNode_t.AppendTextDelegate>(Utils.FindPattern("GUI.dll", "B8 ? ? ? ? E8 ? ? ? ? 83 EC 70 53 56 57 8B F1 68 ? ? ? ? 8D 4D D8 FF 15 ? ? ? ? 33 DB 89 5D FC 39 5D 0C 74 6B FF 75 0C 8D 45 84 50 E8 ? ? ? ? 50 8D 45 A0 68 ? ? ? ? 50 C6 45 FC 01 E8 ? ? ? ? 83 C4 14 68 ? ? ? ? 8D 4D BC 51 8B C8 C6 45 FC 02 FF 15 ? ? ? ? 50 8D 4D D8 C6 45 FC 03 FF 15 ? ? ? ? 8D 4D BC C6 45 FC 02 FF 15 ? ? ? ? 8D 4D A0 C6 45 FC 01 FF 15 ? ? ? ? 8D 4D 84 88 5D FC FF 15 ? ? ? ? FF 75 08 8D 45 84 50 E8 ? ? ? ? 59 59 50 8D 4D D8 C6 45 FC 04 FF 15 ? ? ? ? 53 6A 01 8D 4D 84 88 5D FC E8 ? ? ? ? 8B 3D ? ? ? ? 39 5D 0C 74 0A 68 ? ? ? ? 8D 4D D8"));
            GUIUnk.LoadViewFromXml = Marshal.GetDelegateForFunctionPointer<GUIUnk.LoadViewFromXmlDelegate>(Utils.FindPattern("GUI.dll", "55 8B EC 56 FF 75 10 FF 75 0C FF 15 ? ? ? ? 6A 00 68 ? ? ? ? 68 ? ? ? ? 8B F0 6A 00 56 E8 ? ? ? ? 8B 4D 08 83 C4 1C 89 01 85 C0 75 10 85 F6 74 08 8B 06 6A 01 8B CE FF 10 32 C0 EB 02 B0 01 5E 5D C3"));

            int* pGetOptionWindowOffset = (int*)(Kernel32.GetProcAddress(Kernel32.GetModuleHandle("GUI.dll"), "?ModuleActivated@OptionPanelModule_c@@UAEX_N@Z") + 0x14);
            OptionPanelModule_c.GetOptionWindow = Marshal.GetDelegateForFunctionPointer<OptionPanelModule_c.GetOptionWindowDelegate>(new IntPtr((int)pGetOptionWindowOffset + sizeof(int) + *pGetOptionWindowOffset));

            MovementController.Instance = new MovementController();
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
            if (DynelManager.LocalPlayer == null)
                return;

            OnEarlyUpdate?.Invoke(null, deltaTime);
        }

        private static void OnUpdateInternal(float deltaTime)
        {
            if (DynelManager.LocalPlayer == null)
                return;

            DynelManager.Update();

            Network.Update();
            IPCChannel.Update();

            UIController.UpdateViews();

            Item.Update();
            PerkAction.Update();
            Spell.Update();

            MovementController.Instance?.Update();
            CombatHandler.Instance?.Update(deltaTime);

            try
            { 
                OnUpdate?.Invoke(null, deltaTime);
            }
            catch(Exception e)
            {
                Chat.WriteLine(e.Message);
            }

            Chat.Update();
        }

        private static void OnTeleportStarted()
        {
            IsZoning = true;
            MovementController.Instance?.Halt();
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
