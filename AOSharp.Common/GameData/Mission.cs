namespace AOSharp.Common.GameData
{
    public class Mission
    {
        public Identity Identity;
        public string Name;

        public Mission(Identity identity, string name)
        {
            Identity = identity;
            Name = name;
        }
    }
}
