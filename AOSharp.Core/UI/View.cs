using System;
using AOSharp.Common.GameData;
using AOSharp.Core.GameData;
using AOSharp.Common.Unmanaged.Imports;
using AOSharp.Common.Unmanaged.DataTypes;

namespace AOSharp.Core.UI
{
    public class View
    {
        public readonly string Name;
        public readonly int Handle;

        protected readonly IntPtr _pointer;

        public IntPtr Pointer
        {
            get
            {
                return _pointer;
            }
        }

        internal unsafe View(IntPtr pointer, bool register = false)
        {
            _pointer = pointer;
            Handle = *(int*)(_pointer + 0x44);
            Name = "idk";
            //Name = StdString.FromPointer(*(IntPtr*)(_pointer + 0x20)).ToString();

            if (register)
                UIController.RegisterView(this);
        }

        public static View Create(Rect rect, string name, int unk1, int unk2)
        {
            IntPtr pView = View_c.Create(rect, name, unk1, unk2);

            if (pView == IntPtr.Zero)
                return null;

            return new View(pView);
        }

        public virtual void Dispose()
        {
            View_c.Deconstructor(_pointer);
        }

        public virtual void Update()
        {

        }

        private string GetName()
        {
            return "";
        }

        public void AddChild(View view, bool unk)
        {
            View_c.AddChild(_pointer, view.Pointer, unk);
        }

        public void SetBorders(float x1, float y1, float x2, float y2)
        {
            View_c.SetBorders(_pointer, x1, y1, x2, y2);
        }

        public unsafe void SetFrame(Rect rect, bool unk)
        {
            View_c.SetFrame(_pointer, &rect, unk);
        }

        public unsafe void LimitMaxSize(Vector2 maxSize)
        {
            View_c.LimitMaxSize(_pointer, &maxSize);
        }

        public void SetLayoutNode(LayoutNode layoutNode)
        {
            View_c.SetLayoutNode(_pointer, layoutNode.Pointer);
        }

        public void Show(bool visible, bool unk)
        {
            View_c.Show(_pointer, visible, unk);
        }
    }
}
