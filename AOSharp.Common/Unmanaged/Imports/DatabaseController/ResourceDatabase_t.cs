using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;

namespace AOSharp.Common.Unmanaged.Imports.DatabaseController
{
    public class ResourceDatabase_t
    {
        [DllImport("DatabaseController.dll", EntryPoint = "?GetDbObject@ResourceDatabase_t@@UAEPAVDbObject_t@@ABVIdentity_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr GetDbObject(IntPtr pThis, ref Identity identity);

        [DllImport("DatabaseController.dll", EntryPoint = "?PutDbBlob@ResourceDatabase_t@@QAEXABVIdentity_t@@PBXI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void PutDbBlob(IntPtr pThis, ref Identity identity, [MarshalAs(UnmanagedType.LPArray)] byte[] data, int size);

        [DllImport("DatabaseController.dll", EntryPoint = "?PutDbObject@ResourceDatabase_t@@UAEXPAVDbObject_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void PutDbObject(IntPtr pThis, IntPtr pDBObject);
    }
}

