using System;
using System.Text;
using System.Runtime.InteropServices;

namespace AOSharp.Core
{
    public class AOString
    {
        [DllImport("Utils.dll", EntryPoint = "??0String@@QAE@PBDH@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern IntPtr String(IntPtr obj, byte[] str, int len);

        public unsafe static IntPtr Create(string str)
        {
            byte[] obj = new byte[20];
            fixed (byte* pObj = obj)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(str);
                return String((IntPtr)pObj, bytes, bytes.Length);
            }
        }
    }
}
