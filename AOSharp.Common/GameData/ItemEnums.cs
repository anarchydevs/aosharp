﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Common.GameData
{
    [Flags]
    public enum SimpleItemFlags
    {
        Locked = 0x40,
        Open = 0x80
    }

    public enum ItemClass
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Implant = 3
    }

    public enum SpellListType
    {
        Use = 0,
        Hit = 5,
        Wear = 14,
        Failure = 27
    }

    public enum SpellModifierTarget
    {
        Self = 1,
        User = 2,
        Target = 3
    }

    public enum SpellFunction
    {
        Hit = 53002,
        AnimEffect = 53003,
        ModifyNanoStat = 53012,
        ModifyTemp = 53014,
        TeleportPerk = 53016,
        Upload = 53019,
        Set = 53026,
        HeadMesh = 53035,
        AddSkill = 53028,
        GfxEffect = 53030,
        LockSkill = 53033,
        BackMesh = 53037,
        ShoulderMesh = 53038,
        ApplyTexture = 53039,
        SystemText = 53044,
        ModifyStat = 53045,
        CastNano = 53051,
        BodyMesh = 53054,
        AttractorMesh = 53055,
        FloatText = 53057,
        ChangeMesh = 53060,
        SpawnMonster = 53063,
        SpawnItem = 53064,
        CastTeam = 53066,
        ImplantAccess = 53067,
        Disallow = 53068,
        AoeDmg = 53073,
        ScreenEffect = 53079,
        Teleport = 53083,
        RefreshModel = 53086,
        CastPerk = 53087,
        CastChance = 53089,
        OpenBank = 53092,
        NpcSay = 53104,
        Remove = 53105,
        TempChange = 53110,
        Taunt = 53117,
        ClearHateList = 53126,
        DestroyItem = 53130,
        SetGovernType = 53133,
        Text = 53134,
        ClearFlag = 53140,
        LockPerk = 53187,
        EnableFlight = 53138,
        SetFlag = 53139,
        TeleportLastSave = 53144,
        ResistNano = 53162,
        GenerateName = 53166,
        SummonPet = 53167,
        Deploy = 53173,
        ModifyLvlScaling = 53175,
        Reduce = 53177,
        DisableDefShield = 53178,
        AddAction = 53182,
        DrainDmg = 53185,
        Update = 53189,
        Polymorph = 53193,
        HitPerk = 53196,
        AttractorGfx = 53204,
        RunScript = 53221,
        AddDefProc = 53224,
        CreateCityGuestKey = 53235,
        SpawnQuest = 53226,
        AddOffProc = 53227,
        CastOnPf = 53228,
        SolveQuest = 53229,
        Knockback = 53230,
        EnableRaidLockOnPf = 53231,
        ResetAllPerks = 53234,
        RemoveStrain = 53236,
        ChangeBreed = 53238,
        ChangeGender = 53239,
        CastOnPets = 53240,
        CastBuff = 53242,
        Charge = 53243,
        Transfer = 53249,
        DeleteQuest = 53250,
        FailQuest = 53251,
        SendMail = 53252,
        EndFight = 53253
    }

    public enum SpellPropertyOperator
    {
        Stat = 0,
        Type = 1,
        Min = 2,
        Duration = 3,
        Interval = 4,
        TargetType = 5,
        TargetInstance = 6,
        AnimEffect = 7,
        MeshEffect = 8,
        ItemType = 9,
        ItemInstance = 10,
        Radius = 11,
        RemoveType = 12,
        TextID = 13,
        VisualEffectMesh = 14,
        VisualEffectAnim = 0xF,
        VisualRadius = 0x10,
        AudioEffectID = 17,
        AudioEffectDuration = 18,
        AudioEffectRepeat = 19,
        AudioEffectVolume = 20,
        PoisonType = 21,
        PoisonDifficulty = 22,
        SkillType = 24,
        TimedLength = 25,
        Criteria = 26,
        Operator = 27,
        RelXPos = 28,
        RelYPos = 29,
        RelZPos = 30,
        TeleportDest = 31,
        ApplyOn = 32,
        PoisonSpellType = 33,
        PoisonSpellInstance = 34,
        TargetList = 35,
        Music = 36,
        Max = 37,
        AnimReverse = 38,
        Value = 39,
        TargetExpression = 40,
        Pos = 41,
        Play = 42,
        Speed = 43,
        CatMeshEffect = 44,
        AnimFlag = 45,
        Icon = 46,
        Sex = 47,
        Breed = 48,
        GfxLife = 49,
        GfxSize = 50,
        GfxRed = 51,
        GfxGreen = 52,
        GfxBlue = 53,
        GfxFade = 54,
        Action = 55,
        BodyPart = 56,
        Texture = 57,
        SeeSkin = 58,
        WeaponEffect = 59,
        BaseAmount = 60,
        RegenAmount = 61,
        RegenInterval = 62,
        RarityValue = 63,
        Cost = 64,
        Text = 65,
        Arg1 = 66,
        Arg2 = 67,
        Arg3 = 68,
        Arg4 = 69,
        DamageType = 71,
        TimeExist = 72,
        Flags = 73,
        StrMesh = 74,
        StrTexture = 75,
        BodyCatMesh = 76,
        Hash = 77,
        ToClient = 78,
        Unk1 = 81,
        HighItemIdMaxQl = 84,
        HighItemIdMinQl = 85,
        ScreenVfx = 87,
        ItemId = 88,
        PfModelIdentityType = 94,
        PfModelIdentityInstance = 95,
        Unk4 = 96,
        Unk5 = 97,
        Unk6 = 98,
        Unk7 = 99,
        Unk8 = 100,
        Unk2 = 102,
        Unk3 = 103,
        NanoSchool = 116,
        Unk10 = 120,
        Unk11 = 121,
        Unk21 = 124,
        PetReq1 = 128,
        PetReq2 = 129,
        PetReq3 = 130,
        PetReqVal1 = 131,
        PetReqVal2 = 132,
        PetReqVal3 = 133,
        NanoProperty = 152,
        Unk12 = 153,
        Unk13 = 154,
        Unk14 = 155,
        Unk15 = 156,
        Unk16 = 157,
        Unk17 = 158,
        Unk = 159,
        Unk19 = 160,
        Unk20 = 161,
        Unk9 = 169
    }
}
