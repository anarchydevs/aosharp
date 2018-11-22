using System.Runtime.InteropServices;
using SmokeLounge.AOtomation.Messaging.Serialization.MappingAttributes;

namespace AOSharp.Common.GameData
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Identity
    {
        [AoMember(0)]
        public IdentityType Type { get; set; }

        [AoMember(1)]
        public int Instance { get; set; }

        public static Identity None
        {
            get { return new Identity(IdentityType.None, 0); }
        }

        public Identity(IdentityType type, int instance)
        {
            Type = type;
            Instance = instance;
        }

        public Identity(int instance)
        {
            Type = IdentityType.SimpleChar;
            Instance = instance;
        }

        public override string ToString()
        {
            return string.Format("({0}:{1})", Type, Instance);
        }
        public static bool operator == (Identity identity1, IdentityType Type)
        {
            return identity1 != null && identity1.TypeEquals(Type);
        }

        public static bool operator != (Identity identity1, IdentityType Type)
        {
            return !identity1.TypeEquals(Type);
        }

        public static bool operator == (Identity identity1, Identity identity2)
        {
            return identity1.Equals(identity2);
        }

        public static bool operator != (Identity identity1, Identity identity2)
        {
            return !identity1.Equals(identity2);
        }

        public bool TypeEquals(object obj)
        {
            return (obj is IdentityType) && Type.Equals((IdentityType)obj);
        }

        public override bool Equals(object obj)
        {
            return (obj is Identity) && Type.Equals(((Identity)obj).Type)
                   && Instance.Equals(((Identity)obj).Instance);
        }

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = (23 * hashCode) + Type.GetHashCode();
            hashCode = (23 * hashCode) + Instance.GetHashCode();
            return hashCode;
        }
    }
}
