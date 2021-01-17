using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Xisrith.StructuredLogging.Core;

namespace Xisrith.StructuredLogging.Sqlite.Implementation
{
    public class LogQueueWriterProcessor : ILogQueueWriterProcessor
    {
        private readonly SqliteConnection _connection;

        public LogQueueWriterProcessor(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void Process(IEnumerable<ILogQueueWriter> writers)
        {
            _connection.Open();
            foreach (var writer in writers)
            {
                writer.WriteLogs();
            }
            _connection.Close();
        }

        public Task ProcessAsync(IEnumerable<ILogQueueWriter> writers)
        {
            return Task.Run(() => Process(writers));
        }
    }
}
