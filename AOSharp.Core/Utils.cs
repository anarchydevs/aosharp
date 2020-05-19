using AOSharp.Core.Imports;
using System;
using System.Text;

namespace AOSharp.Core
{
    public static class Utils
    {
        public unsafe static string N3MessageKeyToString(int key)
        {
            return (*N3InfoItemRemote_t.KeyToString(key)).ToString();
        }

        public unsafe static string UnsafePointerToString(IntPtr pointer)
        {
            byte* pStr = (byte*)pointer.ToPointer();

            int cLen = 0;
            while (pStr[cLen] != 0)
                cLen++;

            char[] buffer = new char[cLen];

            fixed (char* pBuffer = buffer)
            {
                Encoding.ASCII.GetChars(pStr, cLen, pBuffer, cLen);
            }

            return new string(buffer);
        }
    }
}
