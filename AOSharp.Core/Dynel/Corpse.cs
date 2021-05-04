using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.Unmanaged.Interfaces;
using AOSharp.Core.Inventory;

namespace AOSharp.Core
{
    public class Corpse : Dynel
    {
        public bool IsOpen => Container.IsOpen;
        public Container Container;

        public Corpse(IntPtr pointer) : base(pointer)
        {
            Container = new Container(Identity);
        }
        
        public Corpse(Dynel dynel) : this(dynel.Pointer)
        {
        }

        public void Open()
        {
            N3EngineClientAnarchy.UseItem(Identity);
        }
    }
}
