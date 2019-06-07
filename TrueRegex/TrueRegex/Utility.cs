using System;
using System.Collections.Generic;
using System.Text;

namespace TrueRegex
{
    internal static class Utility
    {
#if DEBUG
        public static void DebugWrite(object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj);
        }
#endif
    }
}
