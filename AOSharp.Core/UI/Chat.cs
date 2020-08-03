using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.GameData;
using System.Linq;
using AOSharp.Bootstrap;
using AOSharp.Core.UI;

namespace AOSharp.Core.UI
{
    public static class Chat
    {
        private static Dictionary<string, Action<string, string[], ChatWindow>> _customCommands = new Dictionary<string, Action<string, string[], ChatWindow>>();
        private static ConcurrentQueue<(string, ChatColor)> _messageQueue = new ConcurrentQueue<(string, ChatColor)>();
        public static EventHandler<GroupMessageEventArgs> GroupMessageReceived;
        public static void RegisterCommand(string command, Action<string, string[], ChatWindow> callback)
        {
            if(!_customCommands.ContainsKey(command))
                _customCommands.Add(command, callback);
        }

        internal static void Update()
        {
            while (_messageQueue.TryDequeue(out (string text, ChatColor color) msg))
                GamecodeUnk.AppendSystemText(0, msg.text, msg.color);
        }

        private static void OnUnknownCommand(IntPtr pWindow, string command)
        {
            ChatWindow chatWindow = new ChatWindow(pWindow);
            string[] commandParts = command.Remove(0, 1).Trim().Split(' ');

            if (_customCommands.ContainsKey(commandParts[0]))
                _customCommands[commandParts[0]]?.Invoke(commandParts[0], commandParts.Skip(1).ToArray(), chatWindow);
            else
                chatWindow.WriteLine($"No chat command or script named \"{commandParts[0]}\" available.", ChatColor.LightBlue);
        }

        private static void OnGroupMessage(GroupMessageEventArgs args)
        {
            GroupMessageReceived?.Invoke(null, args);
        }

        public static void WriteLine(object obj, ChatColor color = ChatColor.Gold)
        {
            WriteLine(obj.ToString(), color);
        }

        public static void WriteLine(string text, ChatColor color = ChatColor.Gold)
        {
            _messageQueue.Enqueue((text, color));
        }
    }
}