using System;
using System.Collections.Generic;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public static class Chat
    {
        //Currently only supports Default Window
        //Switched to system messages due to a crash with this. TBD whether or not I fix this.
        /*
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

            IntPtr pString = StdString.Create(message);

            ChatWindowNode_t.AppendText(pChatWindowNode, pString, color);

            StdString.Dispose(pString);
        }
        */

        private static Queue<(string, ChatColor)> _messageQueue = new Queue<(string, ChatColor)>();

        internal static void Update()
        {
            while (_messageQueue.Count > 0)
            {
                (string text, ChatColor color) msg = _messageQueue.Dequeue();
                GamecodeUnk.AppendSystemText(0, msg.text, msg.color);
            }
        }

        public static void WriteLine(string text, ChatColor color = ChatColor.Gold)
        {
            _messageQueue.Enqueue((text, color));
        }
    }
}