using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Core
{
    public static class Playfield
    {
        public delegate void DynelSpawnedEventHandler(Dynel dynel);
        public static event DynelSpawnedEventHandler DynelSpawned;

        private unsafe static void DynelSpawnedInternal(IntPtr pDynel)
        {
            if (DynelSpawned != null)
                DynelSpawned(*((Dynel*)pDynel));
        }
    }
}
