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

        internal static void RegisterView(View view)
        {
            _views.Add(view.Handle, view);
        }

        internal static void UpdateViews()
        {
            foreach(View view in _views.Values)
            {
                view.Update();
            }
        }

        private static void OnViewDeleted(IntPtr pView)
        {
            View view = new View(pView, false);

            if (!_views.ContainsKey(view.Handle))
                return;

            _views.Remove(view.Handle);
        }
    }
}
