using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using EasyHook;
using AOSharp.Bootstrap.IPC;

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
            LoadAssemblyMessage msg = message as LoadAssemblyMessage;

            if (_pluginAppDomain != null)
            {
                //Release existing AppDomain
                AppDomain.Unload(_pluginAppDomain);
                _pluginAppDomain = null;
            }

            /*
            if (m.Assemblies.Count == 0)
                return;
            */

            //TODO: load assemblies sent by loader
            List<string> plugins = new List<string>()
            {
                AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\TestPlugin\bin\Debug\TestPlugin.dll"
            };

            try
            {
                AppDomainSetup setup = new AppDomainSetup()
                {
                    ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
                };

                _pluginAppDomain = AppDomain.CreateDomain("plugins", null, setup);

                Type type = typeof(PluginProxy);
                _pluginProxy = (PluginProxy)_pluginAppDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);

                _pluginProxy.LoadCore(_pluginAppDomain.BaseDirectory + "\\AOSharp.Core.dll");

                foreach (string assembly in plugins)
                {
                    _pluginProxy.LoadPlugin(assembly);
                }
            }
            catch (Exception e)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"AOHookException.txt", true))
                {
                    file.WriteLine($"{DateTime.Now}: {e.Message}");
                }
            }
        }

        private void SetupHooks()
        {
            CreateHook("N3.dll",
                        "?AddChildDynel@n3Playfield_t@@QAEXPAVn3Dynel_t@@ABVVector3_t@@ABVQuaternion_t@@@Z",
                        new N3Playfield_t.DAddChildDynel(N3Playfield_t__AddChildDynel_Hook));

            CreateHook("DisplaySystem.dll",
                        "?FrameProcess@VisualEnvFX_t@@QAEXMMIMAAVVector3_t@@AAVQuaternion_t@@@Z",
                        new VisualEnvFX_t.DFrameProcess(VisualEnvFX_FrameProcess_Hook));
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

        public int VisualEnvFX_FrameProcess_Hook(IntPtr pThis, float unk1, float unk2, int unk3, float unk4, int unk5, int unk6)
        {
            try
            {
                _pluginProxy.Update();
            }
            catch (Exception) { }

            return VisualEnvFX_t.FrameProcess(pThis, unk1, unk2, unk3, unk4, unk5, unk6);
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
            public delegate void DynelSpawnedDelegate(IntPtr pDynel);
            public DynelSpawnedDelegate DynelSpawned;
            public delegate void UpdateDelegate();
            public UpdateDelegate Update;
        }


        public class PluginProxy : MarshalByRefObject
        {
            private static CoreDelegates _coreDelegates;

            public void DynelSpawned(IntPtr pDynel)
            {
                if(_coreDelegates.DynelSpawned != null)
                    _coreDelegates.DynelSpawned(pDynel);
            }

            public void Update()
            {
                if (_coreDelegates.Update != null)
                    _coreDelegates.Update();
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
                    Update = CreateDelegate<CoreDelegates.UpdateDelegate>(assembly, "AOSharp.Core.Game", "UpdateInternal"),
                    DynelSpawned = CreateDelegate<CoreDelegates.DynelSpawnedDelegate>(assembly, "AOSharp.Core.DynelManager", "DynelSpawnedInternal")
                };
            }

            public void LoadPlugin(string assemblyPath)
            {
                try
                {
                    //Load main assembly
                    Assembly assembly = Assembly.LoadFile(assemblyPath);

                    //Load references
                    foreach (AssemblyName reference in assembly.GetReferencedAssemblies())
                    {
                        if (reference.Name == "AOSharp.Common" ||
                            reference.Name == "AOSharp.Core")
                            continue;

                        Assembly.Load(reference);
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

                        ConstructorInfo contructor = type.GetConstructor(Type.EmptyTypes);

                        if (contructor == null)
                            continue;

                        object instance = contructor.Invoke(null);

                        if (instance == null) //Is this even possible?
                            continue;

                        method.Invoke(instance, null);
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
