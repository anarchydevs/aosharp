using AOSharp.Bootstrap.IPC;
using EasyHook;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using AOSharp.Common.Unmanaged.Imports;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Common.Helpers;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Bootstrap
{
    [Serializable]
    public class Main : IEntryPoint
    {
        private IPCServer _ipcPipe;
        private AppDomain _pluginAppDomain;
        private ManualResetEvent _connectEvent;
        private ManualResetEvent _unloadEvent;
        private static List<LocalHook> _hooks = new List<LocalHook>();
        private PluginProxy _pluginProxy;

        private string _lastChatInput;
        private IntPtr _lastChatInputWindowPtr;

        public Main(RemoteHooking.IContext inContext, String inChannelName)
        {
            _connectEvent = new ManualResetEvent(false);
            _unloadEvent = new ManualResetEvent(false);

            //Setup IPC server that will be used for handling API requests and events.
            _ipcPipe = new IPCServer(inChannelName);
            _ipcPipe.OnConnected = OnIPCClientConnected;
            _ipcPipe.OnDisconnected = OnIPCClientDisconnected;
            _ipcPipe.RegisterCallback((byte)HookOpCode.LoadAssembly, typeof(LoadAssemblyMessage), OnAssembliesChanged);
            _ipcPipe.Start();
        }

        public void Run(RemoteHooking.IContext inContext, String inChannelName)
        {
            //If the GameController doesn't connect within 10 seconds we will unload the dll.
            if (!_connectEvent.WaitOne(10000))
                return;

            //Wait for the signal to unload the dll.
            _unloadEvent.WaitOne();
        }

        private void OnIPCClientConnected(IPCServer pipe)
        {
            //Notify the main thread we recieved a connection from the GameController.
            _connectEvent.Set();

            SetupHooks();
            //ProcessChatInputPatcher.Patch(this, Test);
        }

        private void OnIPCClientDisconnected(IPCServer pipe)
        {
            UnhookAll();
            //ProcessChatInputPatcher.Unpatch();

            _ipcPipe.Close();

            if (_pluginAppDomain != null)
            {
                AppDomain.Unload(_pluginAppDomain);
                _pluginAppDomain = null;
            }

            //Notify the main thread that it is time to unload the dll.
            _unloadEvent.Set();
        }

        private void OnAssembliesChanged(object pipe, IPCMessage message)
        {
            try
            {
                LoadAssemblyMessage msg = message as LoadAssemblyMessage;

                if (_pluginAppDomain != null)
                {
                    //Release existing AppDomain
                    AppDomain.Unload(_pluginAppDomain);
                    _pluginAppDomain = null;
                }

                if (!msg.Assemblies.Any())
                    return;

                AppDomainSetup setup = new AppDomainSetup()
                {
                    ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
                };

                _pluginAppDomain = AppDomain.CreateDomain("plugins", null, setup);

                Type type = typeof(PluginProxy);
                _pluginProxy = (PluginProxy)_pluginAppDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);

                _pluginProxy.LoadCore(_pluginAppDomain.BaseDirectory + "\\AOSharp.Core.dll");

                foreach (string assembly in msg.Assemblies)
                {
                    _pluginProxy.LoadPlugin(assembly);
                }
            }
            catch (Exception e)
            {
                //TODO: Send IPC message back to loader on error
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"AOSharp.Bootstrap_Exception.txt", true))
                {
                    file.WriteLine($"{DateTime.Now}: {e.Message}");
                }
            }
        }

        private unsafe void SetupHooks()
        {
            CreateHook("N3.dll",
                        "?AddChildDynel@n3Playfield_t@@QAEXPAVn3Dynel_t@@ABVVector3_t@@ABVQuaternion_t@@@Z",
                        new N3Playfield_t.DAddChildDynel(N3Playfield_t__AddChildDynel_Hook));

            CreateHook("Gamecode.dll",
                        "?RunEngine@n3EngineClientAnarchy_t@@UAEXM@Z",
                        new N3EngineClientAnarchy_t.DRunEngine(N3EngineClientAnarchy_RunEngine_Hook));

            CreateHook("Gamecode.dll",
                        "?N3Msg_SendInPlayMessage@n3EngineClientAnarchy_t@@QBE_NXZ",
                        new N3EngineClientAnarchy_t.DSendInPlayMessage(N3EngineClientAnarchy_SendInPlayMessage_Hook));

            CreateHook("GUI.dll",
                        "?TeleportStartedMessage@FlowControlModule_t@@CAXXZ",
                        new FlowControlModule_t.DTeleportStartedMessage(FlowControlModule_t_TeleportStarted_Hook));

            CreateHook("Gamecode.dll",
                        "?TeleportFailed@TeleportTrier_t@@QAEXXZ",
                        new TeleportTrier_t.DTeleportFailed(TeleportTrier_t_TeleportFailed_Hook));

            CreateHook("GUI.dll",
                        "?ModuleActivated@OptionPanelModule_c@@UAEX_N@Z",
                        new OptionPanelModule_c.DModuleActivated(OptionPanelModule_ModuleActivated_Hook));

            CreateHook("GUI.dll",
                        "?ViewDeleted@WindowController_c@@QAEXPAVView@@@Z",
                        new WindowController_c.DViewDeleted(WindowController_ViewDeleted_Hook));

            CreateHook("MessageProtocol.dll",
                        "?DataBlockToMessage@@YAPAVMessage_t@@IPAX@Z",
                        new MessageProtocol.DDataBlockToMessage(DataBlockToMessage_Hook));

            CreateHook("Gamecode.dll",
                        "?PlayfieldInit@n3EngineClientAnarchy_t@@UAEXI@Z",
                        new N3EngineClientAnarchy_t.DPlayfieldInit(N3EngineClientAnarchy_PlayfieldInit_Hook));

            CreateHook("GUI.dll",
                        "?SlotJoinTeamRequest@TeamViewModule_c@@AAEXABVIdentity_t@@ABV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@Z",
                        new TeamViewModule_c.DSlotJoinTeamRequest(TeamViewModule_SlotJoinTeamRequest_Hook));

            CreateHook("Gamecode.dll",
                        "?N3Msg_PerformSpecialAction@n3EngineClientAnarchy_t@@QAE_NABVIdentity_t@@@Z",
                        new N3EngineClientAnarchy_t.DPerformSpecialAction(N3EngineClientAnarchy_PerformSpecialAction_Hook));

            if (ProcessChatInputPatcher.Patch(out IntPtr pProcessCommand, out IntPtr pGetCommand))
            {
                CommandInterpreter_c.ProcessChatInput = Marshal.GetDelegateForFunctionPointer<CommandInterpreter_c.DProcessChatInput>(pProcessCommand);
                CommandInterpreter_c.GetCommand = Marshal.GetDelegateForFunctionPointer<CommandInterpreter_c.DGetCommand>(pGetCommand);
                CreateHook(pProcessCommand, new CommandInterpreter_c.DProcessChatInput(ProcessChatInput_Hook));
                CreateHook(pGetCommand, new CommandInterpreter_c.DGetCommand(GetCommand_Hook));
            }
        }

        private void CreateHook(string module, string funcName, Delegate newFunc)
        {
            CreateHook(LocalHook.GetProcAddress(module, funcName), newFunc);
        }

        public void CreateHook(IntPtr origFunc, Delegate newFunc)
        {
            LocalHook hook = LocalHook.Create(origFunc, newFunc, this);
            hook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            _hooks.Add(hook);
        }

        private void UnhookAll()
        {
            foreach (LocalHook hook in _hooks)
                hook.Dispose();
        }

        public unsafe byte ProcessChatInput_Hook(IntPtr pThis, IntPtr pWindow, StdString* commandText)
        {        
            IntPtr tokenized = StdString.Create();
            ChatGUIModule_t.ExpandChatTextArgs(tokenized, StdString.Create(commandText->ToString()));
            _lastChatInput = ((StdString*)tokenized)->ToString();
            _lastChatInputWindowPtr = pWindow;
            StdString.Dispose(tokenized);

            return CommandInterpreter_c.ProcessChatInput(pThis, pWindow, commandText);
        }

        public unsafe IntPtr GetCommand_Hook(IntPtr pThis, StdString* commandText, bool unk)
        {
            
            IntPtr result;
            if ((result = CommandInterpreter_c.GetCommand(pThis, commandText, unk)) == IntPtr.Zero && unk)
                _pluginProxy.UnknownChatCommand(_lastChatInputWindowPtr, _lastChatInput);

            return result;
        }

        private int DataBlockToMessage_Hook(int size, IntPtr pDataBlock)
        {
            //Let the client process the packet before we inspect it.
            int ret = MessageProtocol.DataBlockToMessage(size, pDataBlock);

            try
            {
                byte[] buffer = new byte[size];
                Marshal.Copy(pDataBlock, buffer, 0, size);
                _pluginProxy.DataBlockToMessage(buffer);
            }
            catch (Exception) { }

            return ret;
        }

        private void WindowController_ViewDeleted_Hook(IntPtr pThis, IntPtr pView)
        {
            try
            {
                _pluginProxy.ViewDeleted(pView);
            }
            catch (Exception) { }

            WindowController_c.ViewDeleted(pThis, pView);
        }

        private unsafe void TeamViewModule_SlotJoinTeamRequest_Hook(IntPtr pThis, IntPtr identity, IntPtr pName)
        {
            try
            {
                _pluginProxy.JoinTeamRequest(identity, pName);
            }
            catch (Exception) { }
        }

        private unsafe bool N3EngineClientAnarchy_PerformSpecialAction_Hook(IntPtr pThis, IntPtr identity)
        {
            bool specialActionResult = N3EngineClientAnarchy_t.PerformSpecialAction(pThis, ref *(Identity*)identity);

            try
            {
                if (specialActionResult)
                    _pluginProxy.ClientPerformedSpecialAction(identity);
            }
            catch (Exception) { }

            return specialActionResult;
        }

        public void OptionPanelModule_ModuleActivated_Hook(IntPtr pThis, bool unk)
        {
            OptionPanelModule_c.ModuleActivated(pThis, unk);

            try
            {
                if (unk)
                    _pluginProxy.OptionPanelActivated(pThis, unk);
            }
            catch (Exception) { }
        }

        public unsafe void FlowControlModule_t_TeleportStarted_Hook()
        {
            try
            {
                if (!*FlowControlModule_t.pIsTeleporting)
                    _pluginProxy.TeleportStarted();
            }
            catch (Exception) { }

            FlowControlModule_t.TeleportStartedMessage();
        }

        public void TeleportTrier_t_TeleportFailed_Hook(IntPtr pThis)
        {
            try
            {
                _pluginProxy.TeleportFailed();
            }
            catch (Exception) { }

            TeleportTrier_t.TeleportFailed(pThis);
        }

        public bool N3EngineClientAnarchy_SendInPlayMessage_Hook(IntPtr pThis)
        {
            try
            {
                _pluginProxy.TeleportEnded();
            }
            catch (Exception) { }

            return N3EngineClientAnarchy_t.SendInPlayMessage(pThis);
        }

        public void N3EngineClientAnarchy_PlayfieldInit_Hook(IntPtr pThis, uint id)
        {
            N3EngineClientAnarchy_t.PlayfieldInit(pThis, id);

            try
            {
                _pluginProxy.PlayfieldInit(id);
            }
            catch (Exception) { }
        }

        public void N3EngineClientAnarchy_RunEngine_Hook(IntPtr pThis, float deltaTime)
        {
            try
            {
                _pluginProxy.EarlyUpdate(deltaTime);

                N3EngineClientAnarchy_t.RunEngine(pThis, deltaTime);

                _pluginProxy.Update(deltaTime);
            }
            catch (Exception) { }
        }

        public void N3Playfield_t__AddChildDynel_Hook(IntPtr pThis, IntPtr pDynel, IntPtr pos, IntPtr rot)
        {
            //Let the client load the dynel before we notify the GameController of it's spawn.
            N3Playfield_t.AddChildDynel(pThis, pDynel, pos, rot);

            try
            {
                _pluginProxy.DynelSpawned(pDynel);
            }
            catch (Exception) { }
        }
    }
}