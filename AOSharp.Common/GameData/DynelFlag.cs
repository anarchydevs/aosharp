using System;

namespace AOSharp.Common.GameData
{
    [Flags]
    public enum CharacterFlags
    {
        None = 0x00,
        Tower = 0x184,
        PetTower = 0x200,
        HasVisibleName = 0x400000,
        Pet = 0x8000000
    }
}
