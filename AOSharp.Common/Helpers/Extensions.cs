using System;
using System.Runtime.CompilerServices;

namespace AOSharp.Common
{
    public static class Extensions
    {
        public static string ToHexString(this byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "");
        }
    }
}
