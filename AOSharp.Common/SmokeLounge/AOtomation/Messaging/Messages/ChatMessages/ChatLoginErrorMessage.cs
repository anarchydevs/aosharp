namespace SmokeLounge.AOtomation.Messaging.Messages.ChatMessages
{
    using SmokeLounge.AOtomation.Messaging.Serialization;
    using SmokeLounge.AOtomation.Messaging.Serialization.MappingAttributes;

    [AoContract((int)ChatMessageType.LoginError)]
    public class ChatLoginErrorMessage : ChatMessageBody
    {
        #region Public Properties

        public override ChatMessageType PacketType
        {
            get
            {
                return ChatMessageType.LoginError;
            }
        }

        #endregion
    }
}