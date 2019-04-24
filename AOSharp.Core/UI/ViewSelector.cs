using System;
using AOSharp.Common.GameData;
using AOSharp.Core.Imports;

namespace AOSharp.Core.UI
{
    public class ViewSelector : View
    {
        internal ViewSelector(IntPtr pointer, bool register) : base(pointer, register)
        {
        }

        public static ViewSelector FromPointer(IntPtr pointer, bool register)
        {
            return new ViewSelector(pointer, register);
        }

        public static ViewSelector Create(Rect rect, string name)
        {
            IntPtr pView = ViewSelector_c.Create(rect, name, -1, 0, 0);

            if (pView == IntPtr.Zero)
                return null;

            return new ViewSelector(pView, true);
        }

        public override void Dispose()
        {
            ViewSelector_c.Deconstructor(_pointer);
        }

        public void SetListView(ListViewBase listViewBase)
        {
            ViewSelector_c.SetListView(_pointer, listViewBase.Pointer);
        }

        public ListViewBase GetListView()
        {
            return ListViewBase.FromPointer(ViewSelector_c.GetListView(_pointer), false);
        }

        public void AppendView(View view)
        {
            ViewSelector_c.AppendView(_pointer, view.Pointer);
        }
    }
}
