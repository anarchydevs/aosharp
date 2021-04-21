using System;

namespace AOSharp.Common.GameData
{
    [Flags]
    public enum DoorState : byte
    {
        Locked = 0x40,
        Open = 0x80
    }
}
