using System;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Core
{
    public static class Time
    {
        public static double NormalTime => GetNormalTime();
       
        private  static double GetNormalTime()
        {
            IntPtr pGameTime = GameTime_t.GetInstance();

            if (pGameTime == IntPtr.Zero)
                return 0;

            return GameTime_t.GetNormalTime(pGameTime);
        }
    }
}
