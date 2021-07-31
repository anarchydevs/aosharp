using System;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Core.GameData;

namespace AOSharp.Core.UI
{
    public class MultiListViewItem
    {
        protected readonly IntPtr _pointer;

        public IntPtr Pointer
        {
            get
            {
                return _pointer;
            }
        }

        protected MultiListViewItem(IntPtr pointer)
        {
            _pointer = pointer;
        }

        public Variant GetID()
        {
            IntPtr pId = MultiListViewItem_c.GetID(_pointer);

            if (pId == IntPtr.Zero)
                return null;

            return Variant.FromPointer(pId, false);
        }

        public void Select(bool selected, bool unk = false)
        {
            MultiListViewItem_c.Select(_pointer, selected, unk);
        }
    }
}
