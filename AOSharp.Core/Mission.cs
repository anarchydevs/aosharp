using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOSharp.Core.GameData;
using AOSharp.Common.GameData;

namespace AOSharp.Core
{
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public unsafe struct Mission
    {
        [FieldOffset(0x00)]
        public Identity Identity;

        [FieldOffset(0x28)]
        private StdObjList ActionList;

        [FieldOffset(0x38)]
        public Identity Source;

        [FieldOffset(0xB4)]
        public Identity Playfield;

        public List<MissionAction> Actions => GetActions();

        public static List<Mission> List => GetMissions();

        private static List<Mission> GetMissions()
        {
            LocalPlayer localPlayer = DynelManager.LocalPlayer;

            if (localPlayer == null)
                return null;

            return localPlayer.GetMissionList();
        }

        private List<MissionAction> GetActions()
        {
            List<MissionAction> actions = new List<MissionAction>();

            foreach(IntPtr pAction in ActionList.ToList())
            {
                IntPtr pObjective = *(IntPtr*)(pAction + 0x8);

                if (pObjective == IntPtr.Zero)
                    continue;

                _MissionAction action = *(_MissionAction*)pObjective;

                switch(action.Type)
                {
                    case MissionActionType.FindPerson:
                        actions.Add(new FindPersonAction(action.Type, action.CharIdentity1));
                        break;
                    case MissionActionType.FindItem:
                        actions.Add(new FindItemAction(action.Type, action.ItemIdentity1));
                        break;
                    case MissionActionType.UseItemOnItem:
                        actions.Add(new UseItemOnItemAction(action.Type, action.ItemIdentity1, action.ItemIdentity2));
                        break;
                    case MissionActionType.KillPerson:
                        actions.Add(new KillPersonAction(action.Type, action.CharIdentity1));
                        break;
                }
            }

            return actions;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct _MissionAction
        {
            [FieldOffset(0x00)]
            public MissionActionType Type;

            [FieldOffset(0x4)]
            public Identity ItemIdentity1;

            [FieldOffset(0xC)]
            public Identity ItemIdentity2;

            [FieldOffset(0x14)]
            public Identity CharIdentity1;

            [FieldOffset(0x1C)]
            public Identity CharIdentity2;

            [FieldOffset(0x20)]
            public Identity MobHash;
        }
    }

    public class MissionAction
    {
        public MissionActionType Type;

        public MissionAction(MissionActionType type)
        {
            Type = type;
        }
    }

    public class FindPersonAction : MissionAction
    {
        public Identity Target;

        public FindPersonAction(MissionActionType type, Identity target) : base(type)
        {
            Target = target;
        }
    }

    public class FindItemAction : MissionAction
    {
        public Identity Target;

        public FindItemAction(MissionActionType type, Identity target) : base(type)
        {
            Target = target;
        }
    }

    public class UseItemOnItemAction : MissionAction
    {
        public Identity Source;
        public Identity Destination;

        public UseItemOnItemAction(MissionActionType type, Identity source, Identity destination) : base(type)
        {
            Source = source;
            Destination = destination;
        }
    }

    public class KillPersonAction : MissionAction
    {
        public Identity Target;

        public KillPersonAction(MissionActionType type, Identity target) : base(type)
        {
            Target = target;
        }
    }
}
