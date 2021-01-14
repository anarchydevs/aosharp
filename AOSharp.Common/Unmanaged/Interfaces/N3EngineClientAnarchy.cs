using System;
using AOSharp.Common.GameData;
using AOSharp.Common.Helpers;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Common.Unmanaged.Interfaces
{
    public class N3EngineClientAnarchy
    {
        public static string GetName(Identity identity)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                return null;
            
            Identity garbage = new Identity();
            
            return Utils.UnsafePointerToString(N3EngineClientAnarchy_t.GetName(pEngine, ref identity, ref garbage));
        }

        public static bool HasPerk(int perkId)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine == IntPtr.Zero)
                throw new NullReferenceException("Could not get N3Engine instance");

            return N3EngineClientAnarchy_t.HasPerk(pEngine, perkId);
        }
        
        public static void UseItem(Identity identity, bool unknown = false)
        {
            IntPtr pEngine = N3Engine_t.GetInstance();

            if (pEngine != IntPtr.Zero)
            {
                N3EngineClientAnarchy_t.UseItem(pEngine, ref identity, unknown);
            }
        }
    }
}
