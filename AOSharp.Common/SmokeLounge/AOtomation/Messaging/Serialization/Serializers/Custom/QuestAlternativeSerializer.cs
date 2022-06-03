using System;
using System.Linq.Expressions;
using System.Text;
using AOSharp.Common;
using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.GameData;
using SmokeLounge.AOtomation.Messaging.Messages;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace SmokeLounge.AOtomation.Messaging.Serialization.Serializers.Custom
{
    class QuestAlternativeSerializer : ISerializer
    {
        public Type Type { get; }

        public object Deserialize(StreamReader streamReader, SerializationContext serializationContext, PropertyMetaData propertyMetaData = null)
        {
            QuestAlternativeMessage qMsg = new QuestAlternativeMessage();

            qMsg.N3MessageType = (N3MessageType)streamReader.ReadInt32();
            qMsg.Identity = new Identity((IdentityType)streamReader.ReadInt32(), streamReader.ReadInt32());
            qMsg.Unknown = streamReader.ReadByte();

            streamReader.Position = streamReader.Position + 1;
            qMsg.MissionSliders = new MissionSliders();
            qMsg.MissionSliders.Difficulty = streamReader.ReadByte();
            qMsg.MissionSliders.GoodBad = streamReader.ReadByte();
            qMsg.MissionSliders.OrderChaos = streamReader.ReadByte();
            qMsg.MissionSliders.OpenHidden = streamReader.ReadByte();
            qMsg.MissionSliders.PhysicalMystical = streamReader.ReadByte();
            qMsg.MissionSliders.HeadonStealth = streamReader.ReadByte();
            qMsg.MissionSliders.CreditsXp = streamReader.ReadByte();
            streamReader.Position = streamReader.Position + 4;
            qMsg.MissionSliders.Scope = (MissionScope)streamReader.ReadByte();
            qMsg.Terminal = new Identity((IdentityType)streamReader.ReadInt32(), streamReader.ReadInt32());
            qMsg.MissionDetails = new MissionInfo[streamReader.ReadByte()];

            for (int i = 0; i < qMsg.MissionDetails.Length; i++)
            {
                qMsg.MissionDetails[i] = new MissionInfo();
                qMsg.MissionDetails[i].MissionIdentity = new Identity((IdentityType)streamReader.ReadInt32(), streamReader.ReadInt32());
                streamReader.Position = streamReader.Position + 16;
                qMsg.MissionDetails[i].Title = streamReader.ReadNullTerminatedString();
                qMsg.MissionDetails[i].Description = Encoding.UTF8.GetString(streamReader.ReadBytes(streamReader.ReadInt32()));
                qMsg.MissionDetails[i].TerminalIdentity = new Identity((IdentityType)streamReader.ReadInt32(), streamReader.ReadInt32());
                qMsg.MissionDetails[i].MissionSlots = streamReader.ReadInt32();
                qMsg.MissionDetails[i].Credits = streamReader.ReadInt32();
                qMsg.MissionDetails[i].MinXp = streamReader.ReadInt32();
                qMsg.MissionDetails[i].MaxXp = streamReader.ReadInt32();
                streamReader.Position = streamReader.Position + 8;
                qMsg.MissionDetails[i].MissionItemData = new MissionItemData[(streamReader.ReadInt32() - 0x3F1) / 0x3F1];

                for (int e = 0; e < qMsg.MissionDetails[i].MissionItemData.Length; e++)
                {
                    qMsg.MissionDetails[i].MissionItemData[e] = new MissionItemData();
                    qMsg.MissionDetails[i].MissionItemData[e].LowId = streamReader.ReadInt32();
                    qMsg.MissionDetails[i].MissionItemData[e].HighId = streamReader.ReadInt32();
                    qMsg.MissionDetails[i].MissionItemData[e].Ql = streamReader.ReadInt32();
                    streamReader.Position = streamReader.Position + 4;
                }

                streamReader.Position = streamReader.Position + 44;
                qMsg.MissionDetails[i].MissionType = streamReader.ReadInt32();
                streamReader.Position = streamReader.Position + 120;
                qMsg.MissionDetails[i].Playfield = new Identity((IdentityType)streamReader.ReadInt32(), streamReader.ReadInt32());
                streamReader.Position = streamReader.Position + 8;
                qMsg.MissionDetails[i].Location = new Vector3(streamReader.ReadSingle(), streamReader.ReadSingle(), streamReader.ReadSingle());
                streamReader.Position = streamReader.Position + 61;
            };

            return qMsg;
        }

        public Expression DeserializerExpression(ParameterExpression streamReaderExpression,
            ParameterExpression serializationContextExpression, Expression assignmentTargetExpression,
            PropertyMetaData propertyMetaData)
        {
            var deserializerMethodInfo =
                ReflectionHelper
                    .GetMethodInfo
                        <QuestAlternativeSerializer, Func<StreamReader, SerializationContext, PropertyMetaData, object>>
                        (o => o.Deserialize);
            var serializerExp = Expression.New(this.GetType());
            var callExp = Expression.Call(
                serializerExp,
                deserializerMethodInfo,
                new Expression[]
                {
                    streamReaderExpression, serializationContextExpression,
                    Expression.Constant(propertyMetaData, typeof(PropertyMetaData))
                });

            var assignmentExp = Expression.Assign(
                assignmentTargetExpression, Expression.TypeAs(callExp, assignmentTargetExpression.Type));
            return assignmentExp;
        }

        public void Serialize(StreamWriter streamWriter, SerializationContext serializationContext, object value, PropertyMetaData propertyMetaData = null)
        {
            var qMsg = (QuestAlternativeMessage)value;

            streamWriter.WriteInt32((int)qMsg.N3MessageType);
            streamWriter.WriteInt32((int)qMsg.Identity.Type);
            streamWriter.WriteInt32(qMsg.Identity.Instance);
            streamWriter.WriteByte(qMsg.Unknown);
            streamWriter.WriteByte(0x4);
            streamWriter.WriteByte(qMsg.MissionSliders.Difficulty);
            streamWriter.WriteByte(qMsg.MissionSliders.GoodBad);
            streamWriter.WriteByte(qMsg.MissionSliders.OrderChaos);
            streamWriter.WriteByte(qMsg.MissionSliders.OpenHidden);
            streamWriter.WriteByte(qMsg.MissionSliders.PhysicalMystical);
            streamWriter.WriteByte(qMsg.MissionSliders.HeadonStealth);
            streamWriter.WriteByte(qMsg.MissionSliders.CreditsXp);
            streamWriter.WriteInt32(0);
            streamWriter.WriteByte((byte)qMsg.MissionSliders.Scope);
            streamWriter.WriteInt32((int)qMsg.Terminal.Type);
            streamWriter.WriteInt32(qMsg.Terminal.Instance);
        }

        public Expression SerializerExpression(ParameterExpression streamWriterExpression,
            ParameterExpression serializationContextExpression, Expression valueExpression, PropertyMetaData propertyMetaData)
        {
            var serializerMethodInfo =
                ReflectionHelper
                    .GetMethodInfo
                    <QuestAlternativeSerializer,
                        Action<StreamWriter, SerializationContext, object, PropertyMetaData>>(o => o.Serialize);
            var serializerExp = Expression.New(this.GetType());
            var callExp = Expression.Call(
                serializerExp,
                serializerMethodInfo,
                new[]
                {
                    streamWriterExpression, serializationContextExpression, valueExpression,
                    Expression.Constant(propertyMetaData, typeof(PropertyMetaData))
                });
            return callExp;
        }
    }
}
