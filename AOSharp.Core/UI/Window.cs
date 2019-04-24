using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Core.Imports;
namespace AOSharp.Core.UI
{
    public class Window
    {
        public List<View> Views = new List<View>();

        private readonly IntPtr _pointer;

        public Window(IntPtr pointer)
        {
            _pointer = pointer;
        }

        public static Window Create(Rect rect, string string1, string string2, WindowStyle style, WindowFlags flags)
        {
            IntPtr pWindow = Window_c.Create(rect, string1, string2, style, flags);

            if (pWindow == IntPtr.Zero)
                return null;

            return new Window(pWindow);
        }

        public void Show(bool visible)
        {
            Window_c.Show(_pointer, visible);
        }

        public unsafe Rect GetBounds()
        {
            IntPtr pRect = Rect_c.Create();
            Rect unmanagedRect = *(Rect*)Window_c.GetBounds(_pointer, pRect);

            Rect rect = new Rect()
            {
                MinX = unmanagedRect.MinX,
                MinY = unmanagedRect.MinY,
                MaxX = unmanagedRect.MaxX,
                MaxY = unmanagedRect.MaxY
            };

            Rect_c.Deconstructor(pRect);

            return rect;
        }

        public void SetTitle(string name)
        {
            IntPtr pName = StdString.Create(name);
            Window_c.SetTitle(_pointer, pName);
            StdString.Dispose(pName);
        }

        public void AppendTab(string name, View view)
        {
            IntPtr pName = StdString.Create(name);
            Window_c.AppendTab(_pointer, pName, view.Pointer);
            StdString.Dispose(pName);
            Views.Add(view);
        }

        public void AppendChild(View view, bool unk)
        {
            Window_c.AppendChild(_pointer, view.Pointer, unk);
        }

        public TabView GetTabView()
        {
            IntPtr pTabView = Window_c.GetTabView(_pointer);

            if (pTabView == IntPtr.Zero)
                return null;

            return new TabView(pTabView);
        }
    }
}
