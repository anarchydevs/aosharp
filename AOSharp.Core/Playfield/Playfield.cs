using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Core.Imports;

namespace AOSharp.Core
{
    public static unsafe class Playfield
    {
        ///<summary>
        ///Playfield Identity
        ///</summary>
        public static Identity Identity => GetIdentity();

        ///<summary>
        ///Playfield's model identity
        ///</summary>
        public static Identity ModelIdentity => GetModelIdentity();

        ///<summary>
        ///Are mechs allowed on the playfield
        ///</summary>
        public static bool AllowsVehicles => AreVehiclesAllowed();

        ///<summary>
        ///Is Shadowlands playfield
        ///</summary>
        public static bool IsShadowlands => IsShadowlandPF();

        ///<summary>
        ///Is BattleStation
        ///</summary>
        public static bool IsBattleStation => IsBattleStationPF();

        ///<summary>
        ///Is playfield a dungeon
        ///</summary>
        public static bool IsDungeon => IsDungeonPF();

        ///<summary>
        ///Playfield name
        ///</summary>
        public static string Name => GetName();

        ///<summary>
        ///Get zones for playfield. Will convert to Zone objects later..
        ///</summary>
        public static List<Zone> Zones => GetZones();

        //TODO: Convert to use n3Playfield_t::GetPlayfieldDynels() to remove dependencies on hard-coded offsets
        internal static unsafe List<IntPtr> GetPlayfieldDynels()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return new List<IntPtr>();

            return (*(Playfield_MemStruct*)pPlayfield).Dynels.ToList();
        }

        private static unsafe Identity GetIdentity()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return Identity.None;

            return *N3Playfield_t.GetIdentity(pPlayfield);
        }

        private static unsafe Identity GetModelIdentity()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return Identity.None;

            return *N3Playfield_t.GetModelID(pPlayfield);
        }

        private static string GetName()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return String.Empty;

            return Marshal.PtrToStringAnsi(N3Playfield_t.GetName(pPlayfield));
        }

        private static unsafe List<Zone> GetZones()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return new List<Zone>();

            return (*N3Playfield_t.GetZones(pPlayfield)).ToList().Select(x => new Zone(x)).ToList();
        }

        private static bool AreVehiclesAllowed()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return false;

            return N3PlayfieldAnarchy_t.AreVehiclesAllowed(pPlayfield);
        }

        private static bool IsShadowlandPF()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return false;

            return N3PlayfieldAnarchy_t.IsShadowlandPF(pPlayfield);
        }

        public static bool LineOfSight(Vector3 pos1, Vector3 pos2, int zoneCell = 1, bool unknown = false)
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return false;

            return N3Playfield_t.LineOfSight(pPlayfield, &pos1, &pos2, zoneCell, false) == 1;
        }

        private static bool IsDungeonPF()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return false;

            return N3Playfield_t.IsDungeon(pPlayfield);
        }

        private static bool IsBattleStationPF()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return false;

            return N3Playfield_t.IsBattleStation(pPlayfield);
        }

        internal static IntPtr GetSurface()
        {
            IntPtr pPlayfield = N3EngineClient_t.GetPlayfield();

            if (pPlayfield == IntPtr.Zero)
                return IntPtr.Zero;

            return N3Playfield_t.GetSurface(pPlayfield);
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct Playfield_MemStruct
        {
            [FieldOffset(0x30)]
            public StdObjVector Dynels;
        }
    }
}
