using System;
using System.Runtime.InteropServices;

namespace AOSharp.Common.GameData
{
    [StructLayout(LayoutKind.Sequential)]
    public class Item
    {
        public int Flags; //?
        public Identity Identity;
        public int LowId;
        public int HighId;
        public int Ql;
        public Identity Placement;
        public int Slot;
    }
}
