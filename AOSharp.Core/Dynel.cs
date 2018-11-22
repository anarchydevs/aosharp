using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public unsafe struct Dynel
    {
        public int vtble; //0x00
        public int Unk1; //0x04
        public int Unk2; //0x08
        public int Unk3; //0x0C
        public int Unk4; //0x10
        public IdentityType Type; //0x14
    }
}
