using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using Xisrith.StructuredLogging.Core;
using Xisrith.StructuredLogging.Sqlite.Implementation;

namespace Xisrith.StructuredLogging.Sqlite
{
    public static class LoggingManagerBuilderExtensions
    {
        public static LoggingManagerBuilder UseSqlite(this LoggingManagerBuilder builder, string connectionString)
            => UseSqlite(builder, new SqliteConnection(connectionString));

        public static LoggingManagerBuilder UseSqlite(this LoggingManagerBuilder builder, SqliteConnection connection)
        {
            builder.LogQueueWriterProcessor = new LogQueueWriterProcessor(connection);
            builder.LogWriterFactory = new LogWriterFactory(connection);
            return builder;
        }
    }
}
