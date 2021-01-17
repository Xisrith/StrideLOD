using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using Xisrith.StructuredLogging.Core;
using Xisrith.StructuredLogging.Sqlite.Commands;
using Xisrith.StructuredLogging.Sqlite.Schema;

namespace Xisrith.StructuredLogging.Sqlite.Implementation
{
    public class LogWriter : ILogWriter
    {
        private readonly SqliteConnection _connection;
        private readonly TableDescriptor _tableDescriptor;

        private bool _checkedTable = false;

        public LogWriter(SqliteConnection connection, TableDescriptor tableDescriptor)
        {
            _connection = connection;
            _tableDescriptor = tableDescriptor;
        }

        public void WriteLogs(IEnumerable<Log> logs)
        {
            if (!_checkedTable)
            {
                var create = _connection.CreateCreateIfNotExistsCommand(_tableDescriptor);
                create.Execute();
                _checkedTable = true;
            }

            using (var transaction = _connection.BeginTransaction())
            {
                var insert = _connection.CreateInsertCommand(_tableDescriptor);
                foreach (var log in logs)
                {
                    insert.Execute(log);
                }
                transaction.Commit();
            }
        }
    }
}
