using System;

namespace AOSharp.Core
{
    public static class Chat
    {
        //Currently only supports Default Window
        public unsafe static void WriteLine(string message, ChatColor color = ChatColor.Gold)
        {
            IntPtr pWindowController = *(IntPtr*)ChatWindowNode_t.ChatWindowController;

            if (pWindowController == IntPtr.Zero)
                return;

            IntPtr pUnk1 = *(IntPtr*)(pWindowController + 0x18);

            if (pUnk1 == IntPtr.Zero)
                return;

            IntPtr pUnk2 = *(IntPtr*)(pUnk1 + 0x4);

            if (pUnk2 == IntPtr.Zero)
                return;

            IntPtr pChatWindowNode = *(IntPtr*)(pUnk2 + 0x28);

            if (pChatWindowNode == IntPtr.Zero)
                return;

            ChatWindowNode_t.AppendText(pChatWindowNode, AOString.Create(message), color);
        }
    }

    public enum ChatColor
    {
        White = 0,
        LightBlue = 4,
        Yellow = 5,
        Green = 8,
        DarkPink = 9,
        Black = 11,
        Red = 12,
        DarkBlue = 14,
        Gold = 17,
        Orange = 27
    }
}
