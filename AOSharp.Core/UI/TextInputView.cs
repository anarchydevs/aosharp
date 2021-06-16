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
    public class TextInputView : View
    {
        public string Text
        {
            get { return GetText(); }
            set { SetText(value); }
        }

        internal TextInputView(IntPtr pointer, bool track = false) : base(pointer, track)
        {
        }

        private void SetText(string text)
        {
            TextInputView_c.SetText(Pointer, StdString.Create(text).Pointer);
        }
        private string GetText()
        {
            IntPtr pStr = TextInputView_c.GetText(Pointer);

            if (pStr == IntPtr.Zero)
                return string.Empty;

            return StdString.FromPointer(pStr, false).ToString();
        }
        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
