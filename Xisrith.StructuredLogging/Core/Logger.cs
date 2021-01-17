using System;
using System.Collections.Generic;
using System.Text;

namespace Xisrith.StructuredLogging.Core
{
    public class Logger<S> : ILogger<S>
    {
        private readonly ILogQueueWriterProvider _queueWriterProvider;
        private readonly string _source;

        public Logger(ILogQueueWriterProvider queueWriterProvider)
        {
            _queueWriterProvider = queueWriterProvider;
            _source = typeof(S).Name;
        }

        public void Log<T>(T log) where T : Log
        {
            log.Source = _source;

            var queue = _queueWriterProvider.GetLogQueueWriter<T>();
            queue.EnqueueLog(log);
        }
    }
}
