using System.Collections.Concurrent;

namespace Xisrith.StructuredLogging.Core
{
    public class LogQueueWriter : ILogQueueWriter
    {
        private readonly ConcurrentQueue<Log> _queue;
        private readonly ILogWriter _writer;

        public LogQueueWriter(ConcurrentQueue<Log> queue, ILogWriter writer)
        {
            _queue = queue;
            _writer = writer;
        }

        public void EnqueueLog(Log log)
        {
            _queue.Enqueue(log);
        }

        public void WriteLogs()
        {
            int length;
            Log[] logs;
            lock (_queue)
            {
                length = _queue.Count;
                logs = new Log[length];
                _queue.CopyTo(logs, 0);
            }

            _writer.WriteLogs(logs);

            lock (_queue)
            {
                for (var i = 0; i < length; i++)
                {
                    _queue.TryDequeue(out _);
                }
            }
        }
    }
}
