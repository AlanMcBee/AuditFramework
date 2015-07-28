using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCharm.Utility
{
    public static class SystemFlagsHelper
    {
        public static bool IsFlagSet(int tested, int flagToCheck)
        {
            return 0 != (tested & flagToCheck);
        }

        public static bool IsAnyFlagSet(int tested, int flagsToCheck)
        {
            return 0 != (tested & flagsToCheck);
        }

        public static bool IsFlagSet<T>(T tested, T flagToCheck)
            where T : IConvertible
        {
            return 0 != (tested.ToInt32(null) & flagToCheck.ToInt32(null));
        }

        public static bool AreAllFlagsSet(int tested, int flagsToCheck)
        {
            return flagsToCheck == (tested & flagsToCheck);
        }


        public static void ClearFlags<T>(ref T flags, T flagsToClear)
            where T : IConvertible
        {
            int intFlags = flags.ToInt32(null);
            int intFlagsToClear = flagsToClear.ToInt32(null);
            intFlags &= ~intFlagsToClear;
            flags = (T)Convert.ChangeType(intFlags, typeof(T));
        }

        public static void SetFlags<T>(ref T flags, T flagsToSet)
            where T : IConvertible
        {
            int intFlags = flags.ToInt32(null);
            int intFlagsToSet = flagsToSet.ToInt32(null);
            intFlags |= intFlagsToSet;
            flags = (T)Convert.ChangeType(intFlags, typeof(T));
        }
    }
}
