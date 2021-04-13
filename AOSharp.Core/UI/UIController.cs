using AOSharp.Common.Unmanaged.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSharp.Core.UI
{
    internal class UIController
    {
        private static Dictionary<int, View> _views = new Dictionary<int, View>();
        private static List<Window> _windows = new List<Window>();

        internal static void RegisterView(View view)
        {
            if (_views.ContainsKey(view.Handle))
                return;

            _views.Add(view.Handle, view);
        }

        internal static void AddWindow(Window window)
        {
            _windows.Add(window);
        }

        internal static bool FindWindow(string name, out Window window)
        {
            return (window = _windows.FirstOrDefault(x => x.Name == name)) != null;
        }

        internal static void UpdateViews()
        {
            try
            {
                foreach(View view in _views.Values)
                {
                    view.Update();
                }
            }
            catch (Exception e)
            {
                Chat.WriteLine($"This shouldn't happen pls report (UIController): {e.Message}");
            }
        }

        internal static void Cleanup()
        {
            foreach(Window window in _windows)
                window.Close();
        }

        private static void OnViewDeleted(IntPtr pView)
        {
            View view = new View(pView, false);

            if (!_views.ContainsKey(view.Handle))
                return;

            _views.Remove(view.Handle);
        }

        private static void OnWindowDeleted(IntPtr pWindow)
        {
            Window window = _windows.FirstOrDefault(x => x.Pointer == pWindow);

            if (window == null)
                return;

            window.IsValid = false;
            _windows.Remove(window);
        }
    }
}
