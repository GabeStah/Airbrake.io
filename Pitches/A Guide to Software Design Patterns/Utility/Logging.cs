using System;

namespace Utility
{
    public static class Logging
    {
        public static void Log(object value)
        {
            #if DEBUG
                System.Diagnostics.Debug.WriteLine(value);
            #else
                Console.WriteLine(value);
            #endif
        }
    }
}
