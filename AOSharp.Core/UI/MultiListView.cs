using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Core.UI
{
    public class MultiListView : View
    {
        protected MultiListView(IntPtr pointer, bool track = false) : base(pointer, track)
        {
        }

        public static MultiListView Create(Rect rect, int flags, int unk = 0, int unk2 = 0)
        {
            IntPtr pView = MultiListView_c.Create(rect, flags, unk, unk2);

            if (pView == IntPtr.Zero)
                return null;

            return new MultiListView(pView, true);
        }

        public override void Dispose()
        {
            MultiListView_c.Deconstructor(_pointer);
        }

        public void AddColumn(int index, string name, float width = 100)
        {
            MultiListView_c.AddColumn(_pointer, index, StdString.Create(name).Pointer, width, 0xE);
        }

        public void SetLayoutMode(int mode)
        {
            MultiListView_c.SetLayoutMode(_pointer, mode);
        }
    }
}
