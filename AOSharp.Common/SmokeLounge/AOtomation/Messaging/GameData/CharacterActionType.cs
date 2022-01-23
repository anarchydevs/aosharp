﻿namespace SmokeLounge.AOtomation.Messaging.GameData
{
    public enum CharacterActionType
    {
        TeamRequest = 0x0000001A,
        CastNano = 0x00000013,
        TeamRequestReply = 0x0000001C,
        TeamKickMember = 0x00000016,
        LeaveTeam = 0x00000018,
        AcceptTeamRequest = 0x00000023,
        RemoveFriendlyNano = 0x00000041,
        UseItemOnItem = 0x00000051,
        StandUp = 0x00000057,
        Unknown3 = 0x00000061,
        SetNanoDuration = 0x00000062,
        Death = 0x00000063,
        InfoRequest = 0x00000069,
        FinishNanoCasting = 0x0000006B,
        InterruptNanoCasting = 0x0000006C,
        DeleteItem = 0x00000070,
        Logout = 0x00000078,
        StopLogout = 0x0000007A,
        Equip = 0x00000083,
        StartedSneaking = 0x000000A2,
        StartSneak = 0x000000A3,
        SpecialAvailable = 0x000000A4,
        SpecialUsed = 0x000000AA,
        Search = 0x00000066,
        DisableXP = 0x000000A5,
        ChangeVisualFlag = 0x000000A6,
        ChangeAnimationAndStance = 0x000000A7,
        ShipInvite = 0x000000BA,
        TrainPerk = 0x000000BB,
        UploadNano = 0x000000CC,
        TradeskillSourceChanged = 0x000000DC,
        TradeskillTargetChanged = 0x000000DD,
        TradeskillBuildPressed = 0x000000DE,
        TradeskillSource = 0x000000DF,
        TradeskillTarget = 0x000000E0,
        TradeskillNotValid = 0x000000E1,
        TradeskillOutOfRange = 0x000000E2,
        TradeskillRequirement = 0x000000E3,
        TradeskillResult = 0x000000E4,
        TransferLeader = 0x00000019,
        TeamRequestInvite = 0x0000001A,
        Split = 0x00000022,
        DuelUpdate = 0x00000106,
        TeamRequestResponse = 0x00000023,
        SplitItem = 0x00000034,
        QueuePerk = 0x00000050,
        UsePerk = 0x000000B3,
        PerkAvailable = 0x000000CE,
        PerkUnavailable = 0x000000CF,
        JoinBattlestationQueue = 0x000000FD,
        LeaveBattlestationQueue = 0x000000FF
    }
}