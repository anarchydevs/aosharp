using System;
using AOSharp.Core.GameData;
using AOSharp.Core.Imports;

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
            IntPtr pNew = MSVCR100.New(0x100);

            bool result = (new Variant(CheckBox_c.GetValue(_pointer, pNew))).AsInt32() == 1;

            Variant_c.Deconstructor(pNew);

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
