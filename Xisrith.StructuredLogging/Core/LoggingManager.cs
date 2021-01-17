using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xisrith.StructuredLogging.Core
{
    public class LoggingManager : ILoggerProvider, ILogQueueWriterProvider, ILogSaver
    {
        private readonly ILogQueueWriterProcessor _logQueueWriterProcessor;
        private readonly ILogWriterFactory _logWriterFactory;

        private readonly Dictionary<Type, ILogger> _loggers = new Dictionary<Type, ILogger>();
        private readonly Dictionary<Type, ILogQueueWriter> _queues = new Dictionary<Type, ILogQueueWriter>();

        public LoggingManager(ILogQueueWriterProcessor logQueueWriterProcessor, ILogWriterFactory logWriterFactory)
        {
            _logQueueWriterProcessor = logQueueWriterProcessor;
            _logWriterFactory = logWriterFactory;
        }

        public ILogger<S> GetLogger<S>()
        {
            var type = typeof(S);

            // Logger exists, return it.
            if (_loggers.ContainsKey(type))
                return (ILogger<S>)_loggers[type];

            // Logger doesn't exist, create it, then return it.
            lock (_loggers)
            {
                // Logger was created by the time we got a lock on the collection.
                if (_loggers.ContainsKey(type))
                    return (ILogger<S>)_loggers[type];

                // Create a new logger.
                var logger = new Logger<S>(this);
                _loggers.Add(type, logger);
                return logger;
            }
        }

        public ILogQueueWriter GetLogQueueWriter<T>() where T : Log
        {
            var type = typeof(T);

            // Processor exists, return it.
            if (_queues.ContainsKey(type))
                return _queues[type];

            // Processor doesn't exist, create it, then return it.
            lock (_queues)
            {
                // Processor was created by the time we got a lock on the collection.
                if (_queues.ContainsKey(type))
                    return _queues[type];

                // Create a new processor.
                var queue = new ConcurrentQueue<Log>();
                var writer = _logWriterFactory.CreateLogWriter<T>();

                var writerQueue = new LogQueueWriter(queue, writer);
                _queues.Add(type, writerQueue);
                return writerQueue;
            }
        }

        public void SaveLogs()
        {
            lock (_logQueueWriterProcessor)
            {
                _logQueueWriterProcessor.Process(_queues.Values);
            }
        }

        public Task SaveLogsAsync()
        {
            lock (_logQueueWriterProcessor)
            {
                return _logQueueWriterProcessor.ProcessAsync(_queues.Values);
            }
        }
    }
}
