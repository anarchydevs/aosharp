using System;
using AOSharp.Core.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Core.UI
{
    public class Checkbox : View
    {
        public bool IsChecked => GetIsChecked();

        public event EventHandler<CheckChangedEventArgs> CheckChanged;

        private bool _prevValue;

        internal Checkbox(IntPtr pointer) : base(pointer)
        {
            _prevValue = IsChecked;
        }

        public static Checkbox Create(string name, string text, bool defaultValue, bool horizontalSpacer = false)
        {
            IntPtr pView = CheckBox_c.Create(name, text, defaultValue, horizontalSpacer);

            if (pView == IntPtr.Zero)
                return null;

            return new Checkbox(pView);
        }

        public override void Dispose()
        {
            CheckBox_c.Deconstructor(_pointer);
        }

        public override void Update()
        {
            bool currentValue = IsChecked;

            if (currentValue != _prevValue)
            {
                CheckChanged?.Invoke(this, new CheckChangedEventArgs(currentValue));
                _prevValue = currentValue;
            }
        }

        private unsafe bool GetIsChecked()
        {
            Variant pOutput = Variant.Create(0);
            IntPtr what = CheckBox_c.GetValue(_pointer, pOutput.Pointer);
            bool result = Variant.FromPointer(what).AsInt32() == 1;
            pOutput.Dispose();

            return result;
        }

        public class CheckChangedEventArgs : EventArgs
        {
            public readonly bool Checked;

            public CheckChangedEventArgs(bool _checked)
            {
                Checked = _checked;
            }
        }
    }
}
