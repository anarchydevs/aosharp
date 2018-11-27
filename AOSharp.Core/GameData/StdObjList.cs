using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AOSharp.Core.GameData
{
    [StructLayout(LayoutKind.Sequential, Pack=0)]
    public unsafe struct StdObjList
    {
        private IntPtr pFirst;
        private int Count;

        public List<IntPtr> ToList()
        {
            List<IntPtr> pointers = new List<IntPtr>();
            IntPtr pCurrent = pFirst;

            for(int i = 0; i < Count; i++)
            {
                pCurrent = *(IntPtr*)pCurrent;
                pointers.Add(pCurrent);
            }

            return pointers;
        }
    }
}
