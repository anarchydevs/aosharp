using System.Text;
using System.Runtime.InteropServices;

namespace AOSharp.Core.GameData
{
    [StructLayout(LayoutKind.Explicit, Pack=0)]
    public unsafe struct StdString
    {
        [FieldOffset(0)]
        private fixed byte _shortBuffer[16];
        [FieldOffset(0)]
        private byte* _pLongBuffer;
        [FieldOffset(16)]
        public int Length;

        public override string ToString()
        {
            if(Length < 16)
            {
                fixed (byte* bytes = _shortBuffer)
                {
                    return Encoding.ASCII.GetString(bytes, Length);
                }
            }
            else
            {
                return Encoding.ASCII.GetString(_pLongBuffer, Length);
            }
        }
    }
}
