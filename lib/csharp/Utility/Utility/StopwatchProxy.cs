using System.Diagnostics;

namespace Utility
{
    /// <summary>
    /// Stores singleton Stopwatch reference.
    /// </summary>
    public class StopwatchProxy
    {
        private readonly Stopwatch _stopwatch;

        public static StopwatchProxy Instance { get; } = new StopwatchProxy();

        static StopwatchProxy() { }

        private StopwatchProxy()
        {
            _stopwatch = new Stopwatch();
        }

        public Stopwatch Stopwatch => _stopwatch;
    }
}
