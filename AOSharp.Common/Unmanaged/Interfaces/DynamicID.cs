using System;
using AOSharp.Common.GameData;
using AOSharp.Common.Helpers;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Common.Unmanaged.Interfaces
{
    public class DynamicID
    {
        public static int GetID(string name, bool unk)
        {
            IntPtr pDynamicID = DynamicID_t.GetInstance();

            if (pDynamicID == IntPtr.Zero)
                return 0;

            return DynamicID_t.GetID(pDynamicID, name, unk);
        }
    }
}
