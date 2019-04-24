using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Core.Imports
{
    public class N3Playfield_t
    {
        [DllImport("N3.dll", EntryPoint = "?LineOfSight@n3Playfield_t@@QBE_NABVVector3_t@@0H_N@Z", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern byte LineOfSight(IntPtr pThis, Vector3* pos1, Vector3* pos2, int zoneCell, bool unknown);

        [DllImport("N3.dll", EntryPoint = "?AddChildDynel@n3Playfield_t@@QAEXPAVn3Dynel_t@@ABVVector3_t@@ABVQuaternion_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void AddChildDynel(IntPtr pThis, IntPtr pDynel, IntPtr pos, IntPtr rot);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate void DAddChildDynel(IntPtr pThis, IntPtr pDynel, IntPtr pos, IntPtr rot);

        [DllImport("N3.dll", EntryPoint = "?IsDungeon@n3Playfield_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern bool IsDungeon(IntPtr pThis);

        [DllImport("N3.dll", EntryPoint = "?IsBattleStation@n3Playfield_t@@QBE_NXZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern bool IsBattleStation(IntPtr pThis);

        [DllImport("N3.dll", CharSet = CharSet.Ansi, EntryPoint = "?GetName@n3Playfield_t@@UBEPBDXZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern IntPtr GetName(IntPtr pThis);

        [DllImport("N3.dll", EntryPoint = "?GetIdentity@n3Playfield_t@@QBEABVIdentity_t@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern Identity* GetIdentity(IntPtr pThis);

        [DllImport("N3.dll", EntryPoint = "?GetModelID@n3Playfield_t@@QBEABVIdentity_t@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public unsafe static extern Identity* GetModelID(IntPtr pThis);
    }
}
