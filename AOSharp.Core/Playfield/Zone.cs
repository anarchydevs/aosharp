using AOSharp.Common.Unmanaged.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Core
{
    public class Zone
    {
        public readonly IntPtr Pointer;

        public Zone(IntPtr pointer)
        {
            Pointer = pointer;
        }

        internal unsafe void LoadSurface()
        {
            IntPtr pSurface = Playfield.GetSurface();

            if (pSurface == IntPtr.Zero)
                return;

            IntPtr pSurfaceUnk = *(IntPtr*)(pSurface + 0x0C);

            if (pSurfaceUnk == IntPtr.Zero)
                return;

            N3Zone_t.LoadSurface(Pointer, pSurfaceUnk);
        }
    }
}
