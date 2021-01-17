using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using Xisrith.StructuredLogging.Core;
using Xisrith.StructuredLogging.Sqlite.Schema;

namespace Xisrith.StructuredLogging.Sqlite.Implementation
{
    public class LogWriterFactory : ILogWriterFactory
    {
        private readonly SqliteConnection _connection;
        private Dictionary<Type, TableDescriptor> _tableDescriptors = new Dictionary<Type, TableDescriptor>();

        public LogWriterFactory(SqliteConnection connection)
        {
            _connection = connection;
        }

        public ILogWriter CreateLogWriter<T>() where T : Log
        {
            var c = _connection;
            var td = GetTableDescriptor<T>();
            return new LogWriter(c, td);
        }

        private TableDescriptor GetTableDescriptor<T>()
        {
            var t = typeof(T);

            // Descriptor exists, return it.
            if (_tableDescriptors.ContainsKey(t))
                return _tableDescriptors[t];

            // Descriptor doesn't exist, create it, then return it.
            lock (_tableDescriptors)
            {
                // Descriptor was created by the time we got a lock on the collection.
                if (_tableDescriptors.ContainsKey(t))
                    return _tableDescriptors[t];

                // Create a new descriptor.
                var tableDescriptor = new TableDescriptor(t);
                _tableDescriptors.Add(t, tableDescriptor);
                return tableDescriptor;
            }
        }
    }

    
}
