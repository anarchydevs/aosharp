using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.Unmanaged.Imports.DatabaseController;
using AOSharp.Common.Unmanaged.Interfaces;

namespace AOSharp.Common.Unmanaged.DbObjects
{
    public class PlayfieldDistrictInfo : DbObject
    {
        public static PlayfieldDistrictInfo Get(int instance)
        {
            Identity identity = new Identity(IdentityType.PlayfieldDistrictInfo, instance);
            return ResourceDatabase.GetDbObject<PlayfieldDistrictInfo>(identity);
        }

        internal PlayfieldDistrictInfo(IntPtr pointer) : base(pointer)
        {
        }

        public IntPtr GetDistrictData(uint unk)
        {
            return PlayfieldDistrictInfo_t.GetDistrictData(Pointer, unk);
        }
    }
}
