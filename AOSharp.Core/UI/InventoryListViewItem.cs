using System;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Core.GameData;

namespace AOSharp.Core.UI
{
    public class InventoryListViewItem : MultiListViewItem
    {
        protected InventoryListViewItem(IntPtr pointer) : base(pointer)
        {
        }

        public virtual void Dispose()
        {
        }

        public static new InventoryListViewItem Create(int unk1, Identity dummyItem, bool unk2)
        {
            IntPtr pView = InventoryListViewItem_c.Create(unk1, dummyItem, unk2);

            if (pView == IntPtr.Zero)
                return null;

            return new InventoryListViewItem(pView);
        }
    }
}
