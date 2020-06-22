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
    public class TabView : View
    {
        public int TabCount => GetTabCount();

        internal TabView(IntPtr pointer) : base(pointer)
        {
        }

        private int GetTabCount()
        {
            return TabView_c.GetTabCount(_pointer);
        }

        public void AppendTab(string name, IntPtr pView)
        {
            IntPtr pName = StdString.Create(name);
            TabView_c.AppendTab(_pointer, pName, pView);
            StdString.Dispose(pName);
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
