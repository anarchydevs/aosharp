﻿using AOSharp.Bootstrap.IPC;
using EasyHook;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using AOSharp.Bootstrap.Imports;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

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
        }

        private void OnIPCClientDisconnected(IPCServer pipe)
        {
            UnhookAll();

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
        }

        private void CreateHook(string module, string funcName, Delegate newFunc)
        {
            CreateHook(LocalHook.GetProcAddress(module, funcName), newFunc);
        }

        private void CreateHook(IntPtr origFunc, Delegate newFunc)
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

        public int DataBlockToMessage_Hook(int size, IntPtr pDataBlock)
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
            bool specialActionResult = N3EngineClientAnarchy_t.PerformSpecialAction(pThis, identity);

            try
            {
                if(specialActionResult && N3EngineClientAnarchy_t.IsPerk(pThis, (*(uint*)identity)))
                    _pluginProxy.ClientPerformedPerk(identity);
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
                if(!*FlowControlModule_t.pIsTeleporting)
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

        public class CoreDelegates
        {
            public delegate void CoreLoadedDelegate();
            public CoreLoadedDelegate CoreLoaded;
            public delegate void DynelSpawnedDelegate(IntPtr pDynel);
            public DynelSpawnedDelegate DynelSpawned;
            public delegate void DataBlockToMessageDelegate(byte[] datablock);
            public DataBlockToMessageDelegate DataBlockToMessage;
            public delegate void UpdateDelegate(float deltaTime);
            public UpdateDelegate Update;
            public delegate void EarlyUpdateDelegate(float deltaTime);
            public EarlyUpdateDelegate EarlyUpdate;
            public delegate void TeleportStartedDelegate();
            public TeleportStartedDelegate TeleportStarted;
            public delegate void TeleportEndedDelegate();
            public TeleportEndedDelegate TeleportEnded;
            public delegate void TeleportFailedDelegate();
            public TeleportFailedDelegate TeleportFailed;
            public delegate void JoinTeamRequestDelegate(Identity pIdentity, IntPtr pName);
            public JoinTeamRequestDelegate JoinTeamRequest;
            public delegate void ClientPerformedPerkDelegate(Identity pIdentity);
            public ClientPerformedPerkDelegate ClientPerformedPerk;
            public delegate void PlayfieldInitDelegate(uint id);
            public PlayfieldInitDelegate PlayfieldInit;
            public delegate void OptionPanelActivatedDelegate(IntPtr pOptionPanelModule, bool unk);
            public OptionPanelActivatedDelegate OptionPanelActivated;
            public delegate void ViewDeletedDelegate(IntPtr pView);
            public ViewDeletedDelegate ViewDeleted;
        }

        public class PluginProxy : MarshalByRefObject
        {
            private static CoreDelegates _coreDelegates;

            public void DataBlockToMessage(byte[] datablock)
            {
                if (_coreDelegates.DataBlockToMessage != null)
                    _coreDelegates.DataBlockToMessage(datablock);
            }

            public unsafe void JoinTeamRequest(IntPtr identity, IntPtr pName)
            {
                if (_coreDelegates.JoinTeamRequest != null)
                    _coreDelegates.JoinTeamRequest(*(Identity*)identity, pName);
            }

            public unsafe void ClientPerformedPerk(IntPtr identity)
            {
                if (_coreDelegates.ClientPerformedPerk != null)
                    _coreDelegates.ClientPerformedPerk(*(Identity*)identity);
            }

            public void DynelSpawned(IntPtr pDynel)
            {
                if(_coreDelegates.DynelSpawned != null)
                    _coreDelegates.DynelSpawned(pDynel);
            }

            public void Update(float deltaTime)
            {
                if (_coreDelegates.Update != null)
                    _coreDelegates.Update(deltaTime);
            }

            public void EarlyUpdate(float deltaTime)
            {
                if (_coreDelegates.EarlyUpdate != null)
                    _coreDelegates.EarlyUpdate(deltaTime);
            }

            public void TeleportStarted()
            {
                if (_coreDelegates.TeleportStarted != null)
                    _coreDelegates.TeleportStarted();
            }

            public unsafe void TeleportEnded()
            {
                if (_coreDelegates.TeleportEnded != null)
                    _coreDelegates.TeleportEnded();
            }

            public void TeleportFailed()
            {
                if (_coreDelegates.TeleportFailed != null)
                    _coreDelegates.TeleportFailed();
            }

            public void PlayfieldInit(uint id)
            {
                if (_coreDelegates.PlayfieldInit != null)
                    _coreDelegates.PlayfieldInit(id);
            }

            public void OptionPanelActivated(IntPtr pOptionPanelModule, bool unk)
            {
                if (_coreDelegates.OptionPanelActivated != null)
                    _coreDelegates.OptionPanelActivated(pOptionPanelModule, unk);
            }

            public void ViewDeleted(IntPtr pView)
            {
                if (_coreDelegates.ViewDeleted != null)
                    _coreDelegates.ViewDeleted(pView);
            }

            private T CreateDelegate<T>(Assembly assembly, string className, string methodName) where T: class
            {
                Type t = assembly.GetType(className);

                if (t == null)
                    return default(T);
                
                MethodInfo m = t.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);

                if (m == null)
                    return default(T);

                return Delegate.CreateDelegate(typeof(T), m) as T;
            }

            public void LoadCore(string assemblyPath)
            {
                //Load main assembly
                var assembly = Assembly.LoadFile(assemblyPath);

                //Load references
                foreach (var reference in assembly.GetReferencedAssemblies())
                {
                    Assembly.Load(reference);
                }

                _coreDelegates = new CoreDelegates()
                {
                    Update = CreateDelegate<CoreDelegates.UpdateDelegate>(assembly, "AOSharp.Core.Game", "OnUpdateInternal"),
                    EarlyUpdate = CreateDelegate<CoreDelegates.EarlyUpdateDelegate>(assembly, "AOSharp.Core.Game", "OnEarlyUpdateInternal"),
                    DynelSpawned = CreateDelegate<CoreDelegates.DynelSpawnedDelegate>(assembly, "AOSharp.Core.DynelManager", "OnDynelSpawned"),
                    TeleportStarted = CreateDelegate<CoreDelegates.TeleportStartedDelegate>(assembly, "AOSharp.Core.Game", "OnTeleportStarted"),
                    TeleportEnded = CreateDelegate<CoreDelegates.TeleportEndedDelegate>(assembly, "AOSharp.Core.Game", "OnTeleportEnded"),
                    TeleportFailed = CreateDelegate<CoreDelegates.TeleportFailedDelegate>(assembly, "AOSharp.Core.Game", "OnTeleportFailed"),
                    PlayfieldInit = CreateDelegate<CoreDelegates.PlayfieldInitDelegate>(assembly, "AOSharp.Core.Game", "OnPlayfieldInit"),
                    OptionPanelActivated = CreateDelegate<CoreDelegates.OptionPanelActivatedDelegate>(assembly, "AOSharp.Core.UI.Options.OptionsPanel", "OnOptionPanelActivated"),
                    ViewDeleted = CreateDelegate<CoreDelegates.ViewDeletedDelegate>(assembly, "AOSharp.Core.UI.UIController", "OnViewDeleted"),
                    DataBlockToMessage = CreateDelegate<CoreDelegates.DataBlockToMessageDelegate>(assembly, "AOSharp.Core.Network", "OnMessage"),
                    JoinTeamRequest = CreateDelegate<CoreDelegates.JoinTeamRequestDelegate>(assembly, "AOSharp.Core.Team", "OnJoinTeamRequest"),
                    ClientPerformedPerk = CreateDelegate<CoreDelegates.ClientPerformedPerkDelegate>(assembly, "AOSharp.Core.Perk", "OnClientPerformedPerk")
                };
            }

            public void LoadPlugin(string assemblyPath)
            {
                try
                {
                    //Load main assembly
                    Assembly assembly = Assembly.LoadFrom(assemblyPath);

                    //Load references
                    foreach (AssemblyName reference in assembly.GetReferencedAssemblies())
                    {
                        if (reference.Name == "AOSharp.Common" ||
                            reference.Name == "AOSharp.Core")
                            continue;

                        try
                        {
                            Assembly.Load(reference);
                        } 
                        catch (FileNotFoundException)
                        {
                            Assembly.LoadFrom($"{Path.GetDirectoryName(assemblyPath)}\\{reference.Name}.dll");
                        }
                    }

                    // Find the first AOSharp.Core.IAOPluginEntry
                    Type[] exportedTypes = assembly.GetExportedTypes();
                    foreach(Type type in exportedTypes)
                    {
                        if (type.GetInterface("AOSharp.Core.IAOPluginEntry") == null)
                            continue;

                        MethodInfo method = type.GetMethod("Run", BindingFlags.Public | BindingFlags.Instance);

                        if (method == null) //Notify of plugin error somewhere?
                            continue;

                        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);

                        if (constructor == null)
                            continue;

                        object instance = constructor.Invoke(null);

                        if (instance == null) //Is this even possible?
                            continue;

                        method.Invoke(instance, new object[] { Path.GetDirectoryName(assemblyPath) });
                    }
                }
                catch (Exception ex)
                {
                    //return null;
                    throw new InvalidOperationException(ex.Message);
                }
            }
        }
    }
}
