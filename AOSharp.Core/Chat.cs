using System;
using System.Collections.Generic;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.GameData;
using System.ComponentModel;
using System.Linq;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Core
{
    public static class Chat
    {
        private static Dictionary<string, Action<string, string[], IntPtr>> _customCommands = new Dictionary<string, Action<string, string[], IntPtr>>();
        private static Queue<(string, ChatColor)> _messageQueue = new Queue<(string, ChatColor)>();

        public static void RegisterCommand(string command, Action<string, string[], IntPtr> callback)
        {
            if(!_customCommands.ContainsKey(command))
                _customCommands.Add(command, callback);
        }

        internal static void Update()
        {
            while (_messageQueue.Count > 0)
            {
                (string text, ChatColor color) msg = _messageQueue.Dequeue();
                GamecodeUnk.AppendSystemText(0, msg.text, msg.color);
            }
        }

        private static void OnUnknownCommand(IntPtr pWindow, string command)
        {
            string[] commandParts = command.Remove(0, 1).Split(' ');

            if (_customCommands.ContainsKey(commandParts[0]))
                _customCommands[commandParts[0]]?.Invoke(commandParts[0], commandParts.Skip(1).ToArray(), pWindow);
            else
                WriteLine(pWindow, $"No chat command or script named \"{commandParts[0]}\" available.", ChatColor.LightBlue);
        }

        public static void WriteLine(string text, ChatColor color = ChatColor.Gold)
        {
            _messageQueue.Enqueue((text, color));
        }

        public static unsafe void WriteLine(IntPtr pWindow, string text, ChatColor color = ChatColor.Gold)
        {
            StdString* errorMsg = (StdString*)StdString.Create(text);
            ChatWindowNode_t.AppendText(pWindow, errorMsg, ChatColor.LightBlue);
            StdString.Dispose((IntPtr)errorMsg);
        }
    }
}