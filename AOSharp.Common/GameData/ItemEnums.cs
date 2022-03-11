using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Common.GameData
{
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
        Wear = 14
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
        Modify = 53045,
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
        VisualEffectAnim = 15,
        VisualRadius = 16,
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
        PetReq1 = 128,
        PetReq2 = 129,
        PetReq3 = 130,
        PetReqVal1 = 131,
        PetReqVal2 = 132,
        PetReqVal3 = 133
    }
}
