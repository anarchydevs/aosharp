using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;

namespace AOSharp.Core
{
    public static class Playfield
    {
        ///<summary>
        ///Are mechs allowed on the playfield
        ///</summary>
        public static bool AllowsVehicles => AreVehiclesAllowed();

        public static bool IsShadowlands => IsShadowlandPF();

        internal unsafe static List<IntPtr> GetPlayfieldDynels()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return new List<IntPtr>();

            return (*(Playfield_MemStruct*)pPlayfield).Dynels.ToList();
        }

        private static bool AreVehiclesAllowed()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            //TODO: throw playfield not initialized exception?
            if (pPlayfield == IntPtr.Zero)
                return false;

            return N3PlayfieldAnarchy_t.AreVehiclesAllowed(pPlayfield);
        }

        private static bool IsShadowlandPF()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            //TODO: throw playfield not initialized exception?
            if (pPlayfield == IntPtr.Zero)
                return false;

            return N3PlayfieldAnarchy_t.IsShadowlandPF(pPlayfield);
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private unsafe struct Playfield_MemStruct
        {
            [FieldOffset(0x18)]
            public Identity Identity;

            [FieldOffset(0x30)]
            public StdObjVector Dynels;
        }
    }
}
