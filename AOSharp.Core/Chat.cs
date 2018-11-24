using System;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public static class Chat
    {
        public unsafe static void WriteLine(string message)
        {
            IntPtr pChatGUIModule = ChatGUIModule_t.GetInstance();

            if (pChatGUIModule == null)
                return;

            IntPtr pUnk1 = *(IntPtr*)(pChatGUIModule + 0xB8);

            if (pUnk1 == IntPtr.Zero)
                return;

            IntPtr ecx = *(IntPtr*)(pUnk1 + 0x38);

            if (ecx == IntPtr.Zero)
                return;

            /*byte[] desu = System.Text.Encoding.ASCII.GetBytes(message);
            fixed (byte* pDesu = desu)
            {
                ChatUnnamed.ChatAppendTextSimple(ecx, pDesu, 82);
            }*/ 
        }
    }
}
