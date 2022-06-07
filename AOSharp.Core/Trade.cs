using System;
using System.Collections.Generic;
using System.Linq;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Core.Inventory;
using SmokeLounge.AOtomation.Messaging.GameData;
using SmokeLounge.AOtomation.Messaging.Messages;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core
{
    public static class Trade
    {
        public static void Open(Identity trader)
        {
            Network.Send(new TradeMessage
            {
                Unknown1 = 2,
                Action = TradeAction.Open,
                Param1 = (int)trader.Type,
                Param2 = trader.Instance,
            });
        }

        public static void AddItem(Identity trade, Identity slot)
        {
            Network.Send(new TradeMessage
            {
                Unknown1 = 2,
                Action = TradeAction.AddItem,
                Param1 = (int)trade.Type,
                Param2 = trade.Instance,
                Param3 = (int)slot.Type,
                Param4 = slot.Instance,
            });
        }

        public static void SetCredits(int credits)
        {
            Network.Send(new TradeMessage
            {
                Unknown1 = 2,
                Action = TradeAction.UpdateCredits,
                Param2 = credits
            });
        }

        public static void Accept(Identity trade)
        {
            Network.Send(new TradeMessage
            {
                Unknown1 = 2,
                Action = TradeAction.Accept,
                Param1 = (int)trade.Type,
                Param2 = trade.Instance,
            });
        }

        public static void Confirm(Identity trade)
        {
            Network.Send(new TradeMessage
            {
                Unknown1 = 2,
                Action = TradeAction.Confirm,
                Param1 = (int)trade.Type,
                Param2 = trade.Instance,
            });
        }

        //To be continued.  Maybe.. one day..
        /*
        internal static void OnTradeMessage(TradeMessage tradeMsg)
        {
            switch(tradeMsg.Action)
            {
                case TradeAction.Open:
                    ActiveTrade = new Trade(new Identity((IdentityType)tradeMsg.Param1, tradeMsg.Param2));
                    TradeStateChanged?.Invoke(ActiveTrade);
                    break;
                case TradeAction.Confirm:
                case TradeAction.Accept:
                    if (ActiveTrade != null)
                    {
                        ActiveTrade.State = (TradeState)tradeMsg.Action;
                        TradeStateChanged?.Invoke(ActiveTrade);
                    }
                    break;
            }
        }*/
    }
}
