using System;
using System.Collections.Generic;
using System.Linq;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Core.UI;

namespace AOSharp.Core
{
    public static class DynelManager
    {
        public static EventHandler<Dynel> DynelSpawned;
        public static EventHandler<SimpleChar> CharInPlay;

        public static LocalPlayer LocalPlayer => GetLocalPlayer();

        public static List<Dynel> AllDynels => GetDynels();

        internal static IEnumerable<Door> Doors => GetDynels().Where(x => x.Identity.Type == IdentityType.Door).Select(x => new Door(x));
        public static IEnumerable<SimpleChar> Characters => GetDynels().Where(x => x.Identity.Type == IdentityType.SimpleChar).Select(x => new SimpleChar(x));
        public static IEnumerable<Corpse> Corpses => GetDynels().Where(x => x.Identity.Type == IdentityType.Corpse).Select(x => new Corpse(x));
        public static IEnumerable<SimpleChar> NPCs => Characters.Where(x => x.IsNpc && !x.IsPet);

        public static IEnumerable<SimpleChar> Players => Characters.Where(x => x.IsPlayer);

        private static Queue<Dynel> _queuedDynelSpawns = new Queue<Dynel>();

        internal static void Update()
        {
            try
            {
                while (_queuedDynelSpawns.Count > 0)
                {
                    Dynel dynel = _queuedDynelSpawns.Dequeue();
                    DynelSpawned?.Invoke(null, dynel);
                }
            }
            catch (Exception e) 
            {
                Chat.WriteLine($"This shouldn't happen pls report (DynelManager): {e.Message}");
            }
        }

        public static Dynel GetDynel(Identity identity)
        {
            return AllDynels.FirstOrDefault(x => x.Identity == identity);
        }

        public static T GetDynel<T>(Identity identity) where T : Dynel
        {
            Dynel dynel = AllDynels.FirstOrDefault(x => x.Identity == identity);

            return dynel == null ? null : dynel.Cast<T>();
        }

        public static bool Find(Identity identity, out Dynel dynel)
        {
            return (dynel = AllDynels.FirstOrDefault(x => x.Identity == identity)) != null;
        }

        public static bool Find(Identity identity, out SimpleChar simpleChar)
        {
            return (simpleChar = Characters.FirstOrDefault(x => x.Identity == identity)) != null;
        }

        public static bool Find(string name, out SimpleChar simpleChar)
        {
            return (simpleChar = Characters.FirstOrDefault(x => x.Name == name)) != null;
        }

        public static bool Find<T>(string name, out T dynel) where T : Dynel
        {
            Dynel foundDynel;
            if ((foundDynel = AllDynels.FirstOrDefault(x => x.Name == name)) != null)
            {
                dynel = foundDynel.Cast<T>();
                return true;
            }

            dynel = null;
            return false;
        }

        public static bool Exists(string name, bool includePets = false)
        {
            if(includePets)
                return Characters.Any(x => x.Name == name);
            else
                return Characters.Any(x => x.Name == name && !x.IsPet);
        }

        public static bool Exists(Identity identity)
        {
            return AllDynels.Any(x => x.Identity == identity);
        }

        public static bool IsValid(Dynel dynel)
        {
            if (dynel == null)
                return false;

            return AllDynels.Any(x => x.Pointer == dynel.Pointer && x.Identity == dynel.Identity && dynel.VehiclePointer != IntPtr.Zero);
        }

        private static LocalPlayer GetLocalPlayer()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return null;

            IntPtr pLocalPlayer = N3EngineClient_t.GetClientControlDynel(pEngine);

            return pLocalPlayer == IntPtr.Zero ? null : new LocalPlayer(pLocalPlayer);
        }

        private static List<Dynel> GetDynels()
        {
            return Playfield.GetPlayfieldDynels().Select(pDynel => new Dynel(pDynel)).Where(x => x.VehiclePointer != IntPtr.Zero).ToList();
        }

        internal static void OnCharInPlay(Identity identity)
        {
            if(Find(identity, out SimpleChar character))
                CharInPlay?.Invoke(null, character);
        }

        private static void OnDynelSpawned(IntPtr pDynel)
        {
            _queuedDynelSpawns.Enqueue(new Dynel(pDynel));
        }
    }
}
