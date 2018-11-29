using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AOSharp.Core.GameData
{
    [StructLayout(LayoutKind.Sequential, Pack=0)]
    public unsafe struct StdStructVector
    {
        private IntPtr pFirst;
        private IntPtr pLast;

        public List<IntPtr> ToList(int size)
        {
            List<IntPtr> pointers = new List<IntPtr>();

            for (IntPtr pCurrent = pFirst; pCurrent.ToInt32() < pLast.ToInt32(); pCurrent += size)
                pointers.Add(pCurrent);

            return pointers;
        }
    }
}
