namespace AOSharp.Common.GameData
{
    public enum SpecialActionOpCode
    {
        Pickup = 0x01,
        Use = 0x03,
        StartCombat = 0x0B,
        Walk = 0x11,
        Sneak = 0x13,
        Crawl = 0x14,
        Sit = 0x4C,
        EndCombat = 0x4E,
        Exit = 0x51,
        Search = 0x86,
        AimedShot = 0x97,
        Backstab = 0x01E9,
        Brawl = 0x8E,
        Burst = 0x94,
        Dimach = 0x90,
        FastAttack = 0x93,
        FlingShot = 0x96,
        FullAuto = 0xA7,
        SneakAttack = 0x92
    }
}
