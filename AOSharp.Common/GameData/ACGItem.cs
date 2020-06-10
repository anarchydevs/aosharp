using System;
using System.Text;
using System.Runtime.InteropServices;

namespace AOSharp.Common.GameData
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ACGItem
    {
        public int LowId;
        public int HighId;
        public int QL;
    }
}
