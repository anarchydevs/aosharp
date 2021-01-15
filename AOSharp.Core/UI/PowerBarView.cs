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
    public class PowerBarView : View
    {
        public float Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        internal PowerBarView(IntPtr pointer) : base(pointer)
        {
        }

        private void SetValue(float value)
        {
            PowerBarView_c.SetValue(Pointer, Variant.Create(value).Pointer, false);
        }

        private float GetValue()
        {
            Variant value = Variant.Create();
            PowerBarView_c.GetValue(Pointer, value.Pointer);
            return value.AsFloat();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
