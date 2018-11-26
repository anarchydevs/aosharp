using System;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;

namespace AOSharp.Core
{
    public unsafe class LocalPlayer : SimpleChar
    {
        public LocalPlayer(IntPtr pointer) : base(pointer)
        {
        }
    }
}
