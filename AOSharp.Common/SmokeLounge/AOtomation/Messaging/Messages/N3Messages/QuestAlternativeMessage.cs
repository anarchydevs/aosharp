// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuestAlternativeMessage.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the QuestAlternativeMessage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using AOSharp.Common.GameData;

namespace SmokeLounge.AOtomation.Messaging.Messages.N3Messages
{
    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Serialization.MappingAttributes;

    [AoContract((int)N3MessageType.QuestAlternative)]
    public class QuestAlternativeMessage : N3Message
    {
        #region Constructors and Destructors

        public QuestAlternativeMessage()
        {
            this.N3MessageType = N3MessageType.QuestAlternative;
        }

        #endregion

        #region AoMember Properties

        [AoMember(0)]
        public byte Unk1 { get; set; }

        [AoMember(1)]
        public byte Difficulty { get; set; }

        [AoMember(2)]
        public byte GoodBad { get; set; }

        [AoMember(3)]
        public byte OrderChaos { get; set; }

        [AoMember(4)]
        public byte OpenHidden { get; set; }

        [AoMember(5)]
        public byte PhysicalMystical { get; set; }

        [AoMember(6)]
        public byte HeadonStealth { get; set; }

        [AoMember(7)]
        public byte CreditsXp { get; set; }

        [AoMember(8)]
        public byte Unk2 { get; set; }

        [AoMember(9)]
        public MissionScope Scope { get; set; }

        [AoMember(10)]
        public Identity Terminal { get; set; }

        [AoMember(11)]
        public byte Unk3 { get; set; }
        #endregion
    }
}