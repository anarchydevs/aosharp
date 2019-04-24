using System;
using AOSharp.Core.Imports;

namespace AOSharp.Core.GameData
{
    public class Variant
    {
        private readonly IntPtr _pointer;

        public IntPtr Pointer
        {
            get
            {
                return _pointer;
            }
        }

        public Variant(IntPtr pointer)
        {
            _pointer = pointer;
        }

        public static Variant Create(int value)
        {
            return new Variant(Variant_c.Constructor(MSVCR100.New(0x10), value));
        }

        public bool AsBool()
        {
            return Variant_c.AsBool(_pointer);
        }

        public int AsInt32()
        {
            return Variant_c.AsInt32(_pointer);
        }
    }
}
