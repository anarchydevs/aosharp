﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatFeedbackMessage.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the FormatFeedbackMessage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Messaging.Messages.N3Messages
{
    using SmokeLounge.AOtomation.Messaging.Serialization;
    using SmokeLounge.AOtomation.Messaging.Serialization.MappingAttributes;

    [AoContract((int)N3MessageType.FormatFeedback)]
    public class FormatFeedbackMessage : N3Message
    {
        #region Constructors and Destructors

        public FormatFeedbackMessage()
        {
            this.N3MessageType = N3MessageType.FormatFeedback;
        }

        #endregion

        #region AoMember Properties

        [AoMember(0)]
        public int Unknown1 { get; set; }

        [AoMember(1, SerializeSize = ArraySizeType.Int16)]
        public string Message { get; set; }
      
        [AoMember(2)]
        public int Unknown2 { get; set; }
        #endregion
    }
}