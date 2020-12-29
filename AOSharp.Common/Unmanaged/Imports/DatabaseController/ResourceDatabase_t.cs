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
    }
}

