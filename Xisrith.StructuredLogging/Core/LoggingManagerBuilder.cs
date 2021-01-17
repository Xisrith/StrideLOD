using System;
using System.Collections.Generic;
using System.Text;

namespace Xisrith.StructuredLogging.Core
{
    public class LoggingManagerBuilder
    {
        public LoggingManager Build()
        {
            return new LoggingManager(LogQueueWriterProcessor, LogWriterFactory);
        }

        public ILogQueueWriterProcessor LogQueueWriterProcessor { get; set; }
        public ILogWriterFactory LogWriterFactory { get; set; }
    }
}
