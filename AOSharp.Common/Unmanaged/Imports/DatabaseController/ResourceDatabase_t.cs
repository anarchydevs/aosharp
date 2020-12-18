using System;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports
{
    public class ResourceDatabase_t
    {
        [DllImport("DatabaseController.dll", EntryPoint = "?GetDbObject@ResourceDatabase_t@@UAEPAVDbObject_t@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetDbObject(IntPtr pThis, IntPtr pIdentity);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
        public delegate IntPtr DGetDbObject(IntPtr pThis, IntPtr pIdentity);
    }
}
