using System;
using System.Collections.Generic;
using System.Linq;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;

namespace AOSharp.Core
{
    public static class DynelManager
    {
        public delegate void DynelSpawnedEventHandler(Dynel dynel);
        public static event DynelSpawnedEventHandler DynelSpawned;

        public static LocalPlayer LocalPlayer => GetLocalPlayer();

        public static List<Dynel> AllDynels => GetDynels();

        public static IEnumerable<SimpleChar> Characters => GetDynels().Where(x => x.Identity.Type == IdentityType.SimpleChar).Select(x => new SimpleChar(x));

        public static IEnumerable<SimpleChar> Players => Characters.Where(x => x.IsPlayer);

        private static LocalPlayer GetLocalPlayer()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return null;

            IntPtr pLocalPlayer = N3EngineClient_t.GetClientControlDynel(pEngine);


            if (pLocalPlayer == IntPtr.Zero)
                return null;

            return new LocalPlayer(pLocalPlayer);
        }

        private unsafe static List<Dynel> GetDynels()
        {
            List<Dynel> dynels = new List<Dynel>();

            foreach (IntPtr pDynel in Playfield.GetPlayfieldDynels())
                dynels.Add(new Dynel(pDynel));

            return dynels;
        }

        private unsafe static void DynelSpawnedInternal(IntPtr pDynel)
        {
            if (DynelSpawned != null)
                DynelSpawned(new Dynel(pDynel));
        }
    }
}
