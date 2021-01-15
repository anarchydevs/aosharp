using System;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Common.Unmanaged.DataTypes
{ 
    public class Variant
    {
        public const int SizeOf = 0x10;
        public readonly IntPtr Pointer;

        private Variant(IntPtr pointer)
        {
            Pointer = pointer;
        }

        public static Variant Create()
        {
            return new Variant(Variant_c.Constructor(MSVCR100.New(SizeOf)));
        }

        public static Variant Create(int value)
        {
            return new Variant(Variant_c.Constructor(MSVCR100.New(SizeOf), value));
        }

        public static Variant Create(float value)
        {
            return new Variant(Variant_c.Constructor(MSVCR100.New(SizeOf), value));
        }

        public static Variant Create(bool value)
        {
            return new Variant(Variant_c.Constructor(MSVCR100.New(SizeOf), value));
        }

        public static Variant FromPointer(IntPtr pointer)
        {
            return new Variant(pointer);
        }

        public override string ToString()
        {
            StdString str = StdString.Create();
            Variant_c.SaveToString(Pointer, str.Pointer);
            return str.ToString();
        }

        public static Variant LoadFromString(string value)
        {
            Variant variant = Variant.Create(0);
            Variant_c.LoadFromString(variant.Pointer, value);
            return variant;
        }

        public void Dispose() => Variant_c.Deconstructor(Pointer);

        public int AsInt32() => Variant_c.AsInt32(Pointer);

        public float AsFloat() => Variant_c.AsFloat(Pointer);

        public bool AsBool() => Variant_c.AsBool(Pointer);

        public string AsString()
        {
            StdString str = StdString.Create();
            Variant_c.AsString(Pointer, str.Pointer);
            return str.ToString();
        }

        public void SetBool(bool value) => Variant_c.SetBool(Pointer, value);

        public static implicit operator Variant(int v) => Variant.Create(v);
        public static implicit operator Variant(float v) => Variant.Create(v);
        public static implicit operator Variant(bool v) => Variant.Create(v);
    }
}
