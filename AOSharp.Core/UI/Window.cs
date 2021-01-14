using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.Unmanaged.DataTypes;
using System.IO;
using AOSharp.Common.Helpers;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Core.UI
{
    public class Window
    {
        public List<View> Views = new List<View>();
        public bool IsVisible => Window_c.IsVisible(_pointer);

        private readonly IntPtr _pointer;

        public Window(IntPtr pointer)
        {
            _pointer = pointer;
        }

        public static Window Create(Rect rect, string string1, string string2, WindowStyle style, WindowFlags flags)
        {
            IntPtr pExistingWindow = Window_c.FindWindowName(string1);
            if (pExistingWindow != IntPtr.Zero)
                return new Window(pExistingWindow);
            
            IntPtr pWindow = Window_c.Create(rect, string1, string2, style, flags);

            if (pWindow == IntPtr.Zero)
                return null;

            return new Window(pWindow);
        }

        public static Window CreateFromXml(string name, string path)
        {
            IntPtr pExistingWindow = Window_c.FindWindowName(name);
            if (pExistingWindow != IntPtr.Zero)
            {
                return new Window(pExistingWindow);
            }

            if (!File.Exists(path))
                return null;

            Window window = Create(new Rect(50, 50, 300, 300), name, name, WindowStyle.Default, WindowFlags.AutoScale);

            if (!GUIUnk.LoadViewFromXml(out IntPtr pView, StdString.Create(path).Pointer, StdString.Create().Pointer))
                return null;

            window.AppendTab(name, new View(pView, false));

            window.MoveToCenter();

            return window;
        }

        public void Show(bool visible)
        {
            if(!IsVisible)
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
            StdString nameStr = StdString.Create(name);
            Window_c.SetTitle(_pointer, nameStr.Pointer);
        }

        public void AppendTab(string name, View view)
        {
            StdString nameStr = StdString.Create(name);
            Window_c.AppendTab(_pointer, nameStr.Pointer, view.Pointer);
            Views.Add(view);
        }

        public void AppendChild(View view, bool unk)
        {
            Window_c.AppendChild(_pointer, view.Pointer, unk);
        }

        public void MoveToCenter()
        {
            Window_c.MoveToCenter(_pointer);
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
