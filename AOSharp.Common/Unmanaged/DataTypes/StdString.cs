using System;
using System.Text;
using System.Runtime.InteropServices;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Common.Unmanaged.DataTypes
{
    public class StdString : IDisposable
    {
        public readonly IntPtr Pointer;
        public unsafe int Length => ((StdStringStruct*)Pointer)->Length;
        private bool disposedValue;

        internal StdString(IntPtr pointer)
        {
            Pointer = pointer;
        }

        public static StdString FromPointer(IntPtr pointer)
        {
            return new StdString(pointer);
        }

        public static StdString Create()
        {
            return Create(string.Empty);
        }

        public static StdString Create(string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            return new StdString(String_c.Constructor(MSVCR100.New(0x14), bytes, bytes.Length));
        }

        public unsafe override string ToString()
        {
            return ((StdStringStruct*)Pointer)->ToString();
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            return ToString() == obj.ToString();
        }

        public static bool operator ==(StdString str1, StdString str2)
        {
            if (Object.ReferenceEquals(str1, null))
            {
                if (Object.ReferenceEquals(str2, null))
                    return true;

                return false;
            }

            return str1.Equals(str2);
        }

        public static bool operator !=(StdString str1, StdString str2)
        {
            return !(str1 == str2);
        }

        public static bool operator ==(StdString str1, string str2)
        {
            if (Object.ReferenceEquals(str1, null))
            {
                if (Object.ReferenceEquals(str2, null))
                    return true;

                return false;
            }

            return str1.Equals(str2);
        }

        public static bool operator !=(StdString str1, string str2)
        {
            return !(str1 == str2);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                String_c.Deconstructor(Pointer);
                disposedValue = true;
            }
        }

        ~StdString()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack=0)]
    public unsafe struct StdStringStruct
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
