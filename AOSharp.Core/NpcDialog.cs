﻿using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.GameData;
using SmokeLounge.AOtomation.Messaging.Messages;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Core
{
    public static class NpcDialog
    {
        public static EventHandler<Dictionary<int, string>> AnswerListChanged;

        internal static void OnKnubotAnswerList(N3Message n3Msg)
        {
            KnuBotAnswerListMessage knubotMsg = (KnuBotAnswerListMessage)n3Msg;
            Dictionary<int, string> options = new Dictionary<int, string>();

            for(int i = 0; i < knubotMsg.DialogOptions.Length; i++)
            {
                options.Add(i, knubotMsg.DialogOptions[i].Text);
            }

            AnswerListChanged?.Invoke(knubotMsg.Target, options);
        }

        public static void SelectAnswer(Identity target, int answer)
        {
            Connection.Send(new KnuBotAnswerMessage()
            {
                Unknown1 = 2,
                Target = target,
                Answer = answer
            });
        }
    }
}
