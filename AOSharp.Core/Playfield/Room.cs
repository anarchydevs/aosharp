using AOSharp.Common.Unmanaged.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using AOSharp.Common.Helpers;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    public class Room : Zone
    {
        public string Name => Utils.UnsafePointerToString(N3Room_t.GetName(Pointer));
        public unsafe Vector3 Position => *N3Room_t.GetPos(Pointer);
        public unsafe Vector3 Center => *N3Room_t.GetCenter(Pointer);
        public unsafe Vector3 TemplatePos => (*(MemStruct*)Pointer).TemplatePos;
        public float Rotation => N3Room_t.GetRot(Pointer);
        public unsafe float YOffset => (*(MemStruct*)Pointer).YOffset;
        public int NumDoors => N3Room_t.GetNumDoors(Pointer);
        public int Floor => N3Room_t.GetFloor(Pointer);
        public Rect Rect => GetRect();
        public unsafe Rect LocalRect => new Rect((*(MemStruct*)Pointer).TileX1, (*(MemStruct*)Pointer).TileY1, (*(MemStruct*)Pointer).TileX2, (*(MemStruct*)Pointer).TileY2);
        public IEnumerable<Door> Doors => Playfield.Doors.Where(x => x.RoomLink1 == this || x.RoomLink2 == this);

        public Room(IntPtr pointer) : base(pointer)
        {
        }

        private Rect GetRect()
        {
            Rect rect;
            N3Room_t.GetRoomRect(Pointer, out rect.MinX, out rect.MinY, out rect.MaxX, out rect.MaxY);
            return rect;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct MemStruct
        {
            [FieldOffset(0x38)]
            public float YOffset;

            [FieldOffset(0x48)]
            public short TileX1;

            [FieldOffset(0x4A)]
            public short TileY1;

            [FieldOffset(0x4C)]
            public short TileX2;

            [FieldOffset(0x4E)]
            public short TileY2;

            [FieldOffset(0x5C)]
            public Vector3 TemplatePos;
        }
    }
}
