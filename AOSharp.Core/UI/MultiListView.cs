using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Interfaces;

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

        public void SetBackgroundBitmap(string gfxName)
        {
            MultiListView_c.SetBackgroundBitmap(_pointer, DynamicID.GetID(gfxName, true));
        }

        public void SetBackgroundBitmap(int gfxId)
        {
            MultiListView_c.SetBackgroundBitmap(_pointer, gfxId);
        }

        public void SetGridIconSpacing(Vector2 spacing)
        {
            MultiListView_c.SetGridIconSpacing(_pointer, ref spacing);
        }

        public void SetGridLabelsOnTop(bool value)
        {
            MultiListView_c.SetGridLabelsOnTop(_pointer, value);
        }

        public void SetViewCellCounts(Vector2 unk1, Vector2 unk2)
        {
            MultiListView_c.SetViewCellCounts(_pointer, ref unk1, ref unk2);
        }

        public bool AddItem(Vector2 slot, MultiListViewItem listViewItem, bool unk)
        {
            return MultiListView_c.AddItem(_pointer, ref slot, listViewItem.Pointer, unk);
        }

        public void RemoveItem(MultiListViewItem listViewItem)
        {
            MultiListView_c.RemoveItem(_pointer, listViewItem.Pointer);
        }

        public Vector2 GetFirstFreePos()
        {
            Vector2 pos = Vector2.Zero;
            MultiListView_c.GetFirstFreePos(_pointer, ref pos);
            return pos;
        }

        public bool GetSelectedItem<T>(out T listViewItem) where T : MultiListViewItem
        {
            listViewItem = null;
            IntPtr pSelectedItem = MultiListView_c.GetSelectedItem(_pointer);

            if (pSelectedItem == IntPtr.Zero)
                return false;

            listViewItem = (T)Activator.CreateInstance(typeof(T), pSelectedItem);

            return true;
        }
    }
}
