using System;
using System.Collections.Generic;
using System.Linq;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;

namespace AOSharp.Core
{
    public static class DynelManager
    {
        public static EventHandler<Dynel> DynelSpawned;

        public static LocalPlayer LocalPlayer => GetLocalPlayer();

        public static List<Dynel> AllDynels => GetDynels();

        public static IEnumerable<SimpleChar> Characters => GetDynels().Where(x => x.Identity.Type == IdentityType.SimpleChar).Select(x => new SimpleChar(x));

        public static IEnumerable<SimpleChar> NPCs => Characters.Where(x => x.IsNpc && !x.IsPet);

        public static IEnumerable<SimpleChar> Players => Characters.Where(x => x.IsPlayer);

        public static Dynel GetDynel(Identity identity)
        {
            return AllDynels.FirstOrDefault(x => x.Identity == identity);
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
            return AllDynels.Any(x => x.Pointer == dynel.Pointer);
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
            return Playfield.GetPlayfieldDynels().Select(pDynel => new Dynel(pDynel)).ToList();
        }

        private static void OnDynelSpawned(IntPtr pDynel)
        {
            DynelSpawned?.Invoke(null, new Dynel(pDynel));
        }
    }
}
