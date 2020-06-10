using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Bootstrap.Imports
{
    public class TeamViewModule_c
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public unsafe delegate void DSlotJoinTeamRequest(IntPtr pThis, IntPtr identity, IntPtr pName);
    }
}
