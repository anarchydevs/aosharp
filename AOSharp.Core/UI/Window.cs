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
using System.Reflection;

namespace AOSharp.Core.UI
{
    public class Window
    {
        public List<View> Views = new List<View>();
        public bool IsVisible => Window_c.IsVisible(Pointer);
        public bool IsValid = true;
        public readonly string Name;

        public readonly IntPtr Pointer;

        private Window(IntPtr pointer)
        {
            Pointer = pointer;
            Name = GetName();
            UIController.AddWindow(this);
        }

        public static Window Create(Rect rect, string name, string name2, WindowStyle style, WindowFlags flags)
        {
            if (UIController.FindWindow(name, out Window existingWindow))
                return existingWindow;
            
            IntPtr pWindow = Window_c.Create(rect, name, name2, style, flags);

            if (pWindow == IntPtr.Zero)
                return null;

            return new Window(pWindow);
        }

        public static Window CreateFromXml(string name, string path, WindowStyle windowStyle = WindowStyle.Default,
            WindowFlags windowFlags = WindowFlags.None)
        {
            return CreateFromXml(name, path, new Rect(50, 50, 300, 300), windowStyle, windowFlags);
        }
        
        public static Window CreateFromXml(string name, string path, Rect windowSize, WindowStyle windowStyle = WindowStyle.Default, WindowFlags windowFlags = WindowFlags.None)
        {
            if (UIController.FindWindow(name, out Window existingWindow))
                return existingWindow;

            if (!File.Exists(path))
                return null;

            Window window = Create(windowSize, name, name, windowStyle, windowFlags);

            if (!GUIUnk.LoadViewFromXml(out IntPtr pView, StdString.Create(path).Pointer, StdString.Create().Pointer))
                return null;

            window.AppendTab(name, new View(pView, false));

            window.MoveToCenter();

            return window;
        }

        public bool FindView<T>(string name, out T view) where T : View
        {
            view = null;
            IntPtr pView = Window_c.FindView(Pointer, StdString.Create(name).Pointer);

            if (pView == IntPtr.Zero)
                return false;

            view = Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { pView }, null) as T;
            return true;
        }

        public void Show(bool visible)
        {
            if (!IsValid)
                return;

            if(!IsVisible)
                Window_c.Show(Pointer, visible);
        }

        public void Close()
        {
            if (!IsValid)
                return;

            Window_c.Close(Pointer);
        }

        private string GetName()
        {
            StdString name = StdString.Create();
            Looper.GetName(Pointer, name.Pointer);
            return name.ToString();
        }

        public unsafe Rect GetBounds()
        {
            IntPtr pRect = Rect_c.Create();
            Rect unmanagedRect = *(Rect*)Window_c.GetBounds(Pointer, pRect);

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
            Window_c.SetTitle(Pointer, nameStr.Pointer);
        }

        public void AppendTab(string name, View view)
        {
            StdString nameStr = StdString.Create(name);
            Window_c.AppendTab(Pointer, nameStr.Pointer, view.Pointer);
            Views.Add(view);
        }

        public void AppendChild(View view, bool unk)
        {
            Window_c.AppendChild(Pointer, view.Pointer, unk);
        }

        public void MoveToCenter()
        {
            Window_c.MoveToCenter(Pointer);
        }

        public TabView GetTabView()
        {
            IntPtr pTabView = Window_c.GetTabView(Pointer);

            if (pTabView == IntPtr.Zero)
                return null;

            return new TabView(pTabView);
        }
    }
}
