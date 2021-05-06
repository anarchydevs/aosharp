using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.GameData.UI;

namespace AOSharp.Core.UI
{
    public class Button : ButtonBase
    {
        public string Label => GetLabel();

        internal Button(IntPtr pointer, bool track = false) : base(pointer, track)
        {
        }

        public void SetLabel(string text)
        {
            Button_c.SetLabel(Pointer, StdString.Create(text).Pointer);
        }
        
        private string GetLabel()
        {
            IntPtr pStr = Button_c.GetLabel(Pointer);

            if (pStr == IntPtr.Zero)
                return string.Empty;

            return StdString.FromPointer(pStr, false).ToString();
        }

        public void SetGfx(ButtonState state, int gfxId)
        {
            Button_c.SetGfx(Pointer, state, gfxId);
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
