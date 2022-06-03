// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TowerInfo.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the TowerInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Messaging.GameData
{
    using AOSharp.Common.GameData;
    using SmokeLounge.AOtomation.Messaging.Serialization;
    using SmokeLounge.AOtomation.Messaging.Serialization.MappingAttributes;

    public class MissionInfo
    {
        #region AoMember Properties

        [AoMember(0)]
        public Identity MissionIdentity { get; set; }

        [AoMember(1)]
        public string Title { get; set; }

        [AoMember(2)]
        public string Description { get; set; }

        [AoMember(3)]
        public Identity TerminalIdentity { get; set; }

        [AoMember(4)]
        public int MissionSlots { get; set; }

        [AoMember(5)]
        public int Credits { get; set; }

        [AoMember(6)]
        public int MinXp { get; set; }

        [AoMember(7)]
        public int MaxXp { get; set; }

        [AoMember(8, SerializeSize = ArraySizeType.Byte)]
        public MissionItemData[] MissionItemData { get; set; }
       
        [AoMember(9)]
        public int MissionType { get; set; }

        [AoMember(10)]
        public Identity Playfield{ get; set; }

        [AoMember(11)]
        public Vector3 Location { get; set; }

        #endregion
    }
}