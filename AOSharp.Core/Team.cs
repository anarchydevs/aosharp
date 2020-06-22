using System;
using System.Collections.Generic;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Imports;
using SmokeLounge.AOtomation.Messaging.Messages;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core
{
    public static class Team
    {
        public static bool IsInTeam => GetIsInTeam();
        public static bool IsLeader => GetIsTeamLeader();
        public static bool IsRaid => GetIsRaid();
        public static List<TeamMember> Members => GetMemberList();

        public static EventHandler<TeamRequestEventArgs> TeamRequest;
        public static EventHandler<Identity> MemberLeft;

        public static void Invite(Identity target)
        {
            Network.Send(new CharacterActionMessage()
            {
                Action = CharacterActionType.TeamRequest,
                Target = target
            });
        }

        public static void Kick(Identity target)
        {
            Network.Send(new CharacterActionMessage()
            {
                Action = CharacterActionType.TeamKick,
                Target = target
            });
        }

        public static void Accept(Identity target)
        {
            //TODO: Actually call the game function to accept so that the client doesn't bug out.
            Network.Send(new CharacterActionMessage()
            {
                Action = CharacterActionType.TeamRequestReply,
                Target = target,
                Parameter2 = (int)TeamRequestResponseAction.Accept
            });
        }

        public static void Decline(Identity target)
        {
            Network.Send(new CharacterActionMessage()
            {
                Action = CharacterActionType.TeamRequestResponse,
                Target = target,
                Parameter2 = (int)TeamRequestResponseAction.Decline
            });
        }

        public static void Leave()
        {
            Network.Send(new CharacterActionMessage()
            {
                Action = CharacterActionType.LeaveTeam
            });
        }

        public static void ConvertToRaid()
        {
            Network.Send(new RaidCmdMessage()
            {
                CommandType = RaidCmdType.CreateRaid
            });
        }

        private static bool GetIsTeamLeader()
        {
            IntPtr pTeamViewModule = TeamViewModule_c.GetInstanceIfAny();

            if (pTeamViewModule == IntPtr.Zero)
                return false;

            return TeamViewModule_c.IsTeamLeader(pTeamViewModule) == 1;
        }

        private static bool GetIsInTeam()
        {
            IntPtr pTeamViewModule = TeamViewModule_c.GetInstanceIfAny();

            if (pTeamViewModule == IntPtr.Zero)
                return false;

            return TeamViewModule_c.IsInTeam(pTeamViewModule) == 1;
        }

        private static bool GetIsRaid()
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return false;

            return N3EngineClientAnarchy_t.IsInRaidTeam(pEngine) == 1;
        }

        private static unsafe List<TeamMember> GetMemberList()
        {
            List<TeamMember> teamMembers = new List<TeamMember>();
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return teamMembers;


            for (int i = 0; i < (IsRaid ? 6 : 1); i++)
            {
                StdObjVector* pMemberList = N3EngineClientAnarchy_t.GetTeamMemberList(pEngine, i);

                if (pMemberList == null)
                    continue;

                foreach (IntPtr pMember in pMemberList->ToList())
                    teamMembers.Add(new TeamMember(pMember, i));
            }

            return teamMembers;
        }

        private static unsafe void OnJoinTeamRequest(Identity identity, IntPtr pName)
        {
            TeamRequestEventArgs args = new TeamRequestEventArgs(identity);
            TeamRequest?.Invoke(null, args);

            //Kinda weird but basically this is to call the original event that spawns the team dialog box.
            //We only want to spawn the dialog box if we didn't reply immediately via event.
            if(!args.Responded)
            {
                IntPtr pTeamViewModule = TeamViewModule_c.GetInstanceIfAny();

                if (pTeamViewModule == IntPtr.Zero)
                    return;

                TeamViewModule_c.SlotJoinTeamRequest(pTeamViewModule, &identity, pName);
            }
        }

        internal static void OnMemberLeft(Identity leaver)
        {
            MemberLeft?.Invoke(null, leaver);
        }
    }

    public class TeamRequestEventArgs : EventArgs
    {
        public Identity Requester { get; }
        public bool Responded { get; set; }

        public TeamRequestEventArgs(Identity requester)
        {
            Requester = requester;
        }

        public void Accept()
        {
            Team.Accept(Requester);
            Responded = true;
        }

        public void Decline()
        {
            Team.Decline(Requester);
            Responded = true;
        }
    }
}
