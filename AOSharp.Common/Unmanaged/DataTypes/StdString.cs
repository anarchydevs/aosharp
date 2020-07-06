using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Common.Unmanaged.DataTypes
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

        public static IntPtr Create()
        {
            return Create(string.Empty);
        }

        public static IntPtr Create(string str)
        {
            IntPtr pNew = MSVCR100.New(0x14);
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            return String_c.Constructor(pNew, bytes, bytes.Length);
        }

        public static int Dispose(IntPtr pString)
        {
            return String_c.Deconstructor(pString);
        }
    }
}
