using System;

namespace Stride.Profiling.Logging
{
    public class ProfileLogEntry
    {
        /// <summary>
        /// Freeform text label for grouping profile logs.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// FPS recorded at time of log.
        /// </summary>
        public double Fps { get; set; }
    }
}
