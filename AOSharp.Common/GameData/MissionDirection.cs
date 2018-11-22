namespace AOSharp.Common.GameData
{
    public enum MissionDirection
    {
        Ascending,
        Descending,
        Boss
    }

    public static class MissionDirectionMethods
    {
        public static MissionDirection Invert(this MissionDirection direction)
        {
            return direction == MissionDirection.Descending ? MissionDirection.Ascending : MissionDirection.Descending;
        }
    }
}
