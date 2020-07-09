using System;
using System.Reflection;
using AOSharp.Common.GameData;
using System.IO;

namespace AOSharp.Bootstrap
{
    public class CoreDelegates
    {
        public delegate void InitDelegate();
        public InitDelegate Init;
        public delegate void OnPluginLoadedDelegate(Assembly assembly);
        public OnPluginLoadedDelegate OnPluginLoaded;
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
        public delegate void AttemptingSpellCastDelegate(AttemptingSpellCastEventArgs args);
        public AttemptingSpellCastDelegate AttemptingSpellCast;
        public delegate void UnknownCommandDelegate(IntPtr pWindow, string command);
        public UnknownCommandDelegate UnknownChatCommand;
    }

    public class PluginProxy : MarshalByRefObject
    {
        private static CoreDelegates _coreDelegates;

        public void UnknownChatCommand(IntPtr pWindow, string command)
        {
            _coreDelegates.UnknownChatCommand?.Invoke(pWindow, command);
        }

        public void DataBlockToMessage(byte[] datablock)
        {
            _coreDelegates.DataBlockToMessage?.Invoke(datablock);
        }

        public unsafe void JoinTeamRequest(IntPtr identity, IntPtr pName)
        {
            _coreDelegates.JoinTeamRequest?.Invoke(*(Identity*)identity, pName);
        }

        public unsafe bool AttemptingSpellCast(IntPtr nanoIdentity, IntPtr targetIdentity)
        {
            AttemptingSpellCastEventArgs eventArgs = new AttemptingSpellCastEventArgs(*(Identity*)nanoIdentity, *(Identity*)targetIdentity);
            _coreDelegates.AttemptingSpellCast?.Invoke(eventArgs);
            return eventArgs.Blocked;
        }

        public unsafe void ClientPerformedPerk(IntPtr identity)
        {
            _coreDelegates.ClientPerformedPerk?.Invoke(*(Identity*)identity);
        }

        public void DynelSpawned(IntPtr pDynel)
        {
            _coreDelegates.DynelSpawned?.Invoke(pDynel);
        }

        public void Update(float deltaTime)
        {
            _coreDelegates.Update?.Invoke(deltaTime);
        }

        public void EarlyUpdate(float deltaTime)
        {
            _coreDelegates.EarlyUpdate?.Invoke(deltaTime);
        }

        public void TeleportStarted()
        {
            _coreDelegates.TeleportStarted?.Invoke();
        }

        public unsafe void TeleportEnded()
        {
            _coreDelegates.TeleportEnded?.Invoke();
        }

        public void TeleportFailed()
        {
            _coreDelegates.TeleportFailed?.Invoke();
        }

        public void PlayfieldInit(uint id)
        {
            _coreDelegates.PlayfieldInit?.Invoke(id);
        }

        public void OptionPanelActivated(IntPtr pOptionPanelModule, bool unk)
        {
            _coreDelegates.OptionPanelActivated?.Invoke(pOptionPanelModule, unk);
        }

        public void ViewDeleted(IntPtr pView)
        {
            _coreDelegates.ViewDeleted?.Invoke(pView);
        }

        private T CreateDelegate<T>(Assembly assembly, string className, string methodName) where T : class
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
                Init = CreateDelegate<CoreDelegates.InitDelegate>(assembly, "AOSharp.Core.Game", "Init"),
                OnPluginLoaded = CreateDelegate<CoreDelegates.OnPluginLoadedDelegate>(assembly, "AOSharp.Core.Game", "OnPluginLoaded"),
                Update = CreateDelegate<CoreDelegates.UpdateDelegate>(assembly, "AOSharp.Core.Game", "OnUpdateInternal"),
                EarlyUpdate = CreateDelegate<CoreDelegates.EarlyUpdateDelegate>(assembly, "AOSharp.Core.Game", "OnEarlyUpdateInternal"),
                DynelSpawned = CreateDelegate<CoreDelegates.DynelSpawnedDelegate>(assembly, "AOSharp.Core.DynelManager", "OnDynelSpawned"),
                TeleportStarted = CreateDelegate<CoreDelegates.TeleportStartedDelegate>(assembly, "AOSharp.Core.Game", "OnTeleportStarted"),
                TeleportEnded = CreateDelegate<CoreDelegates.TeleportEndedDelegate>(assembly, "AOSharp.Core.Game", "OnTeleportEnded"),
                TeleportFailed = CreateDelegate<CoreDelegates.TeleportFailedDelegate>(assembly, "AOSharp.Core.Game", "OnTeleportFailed"),
                PlayfieldInit = CreateDelegate<CoreDelegates.PlayfieldInitDelegate>(assembly, "AOSharp.Core.Game", "OnPlayfieldInit"),
                OptionPanelActivated = CreateDelegate<CoreDelegates.OptionPanelActivatedDelegate>(assembly, "AOSharp.Core.UI.Options.OptionPanel", "OnOptionPanelActivated"),
                ViewDeleted = CreateDelegate<CoreDelegates.ViewDeletedDelegate>(assembly, "AOSharp.Core.UI.UIController", "OnViewDeleted"),
                DataBlockToMessage = CreateDelegate<CoreDelegates.DataBlockToMessageDelegate>(assembly, "AOSharp.Core.Network", "OnMessage"),
                JoinTeamRequest = CreateDelegate<CoreDelegates.JoinTeamRequestDelegate>(assembly, "AOSharp.Core.Team", "OnJoinTeamRequest"),
                ClientPerformedPerk = CreateDelegate<CoreDelegates.ClientPerformedPerkDelegate>(assembly, "AOSharp.Core.Perk", "OnClientPerformedPerk"),
                AttemptingSpellCast = CreateDelegate<CoreDelegates.AttemptingSpellCastDelegate>(assembly, "AOSharp.Core.MiscClientEvents", "OnAttemptingSpellCast"),
                UnknownChatCommand = CreateDelegate<CoreDelegates.UnknownCommandDelegate>(assembly, "AOSharp.Core.UI.Chat", "OnUnknownCommand")
            };

            _coreDelegates.Init();
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
                        reference.Name == "AOSharp.Bootstrap" ||
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
                foreach (Type type in exportedTypes)
                {
                    if (type.GetInterface("AOSharp.Core.IAOPluginEntry") == null)
                        continue;

                    MethodInfo method = type.GetMethod("Run", BindingFlags.Public | BindingFlags.Instance);

                    if (method == null) //Notify of plugin error somewhere?
                        continue;

                    ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);

                    if (constructor == null)
                        continue;

                    _coreDelegates.OnPluginLoaded(assembly);

                    object instance = constructor.Invoke(null);

                    if (instance == null) //Is this even possible?
                        continue;

                    method.Invoke(instance, new object[] { Path.GetDirectoryName(assemblyPath) });
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
