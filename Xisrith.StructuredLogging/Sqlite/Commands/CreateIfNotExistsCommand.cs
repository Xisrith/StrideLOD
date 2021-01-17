using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Xisrith.StructuredLogging.Sqlite.Schema;

namespace Xisrith.StructuredLogging.Sqlite.Commands
{
    public class CreateIfNotExistsCommand
    {
        private readonly SqliteCommand _command;

        public CreateIfNotExistsCommand(SqliteConnection connection, TableDescriptor tableDescriptor)
        {
            _command = connection.CreateCommand();

            var tableLines = new List<string>();
            tableLines.Add("\"ID\" INTEGER");
            tableLines.Add("\"TIMESTAMP\" TEXT DEFAULT CURRENT_TIMESTAMP");

            foreach (var descriptor in tableDescriptor.ColumnDescriptors)
            {
                tableLines.Add($"{descriptor.ColumnName} {descriptor.SqliteType.ToString().ToUpper()}");
            }

            tableLines.Add("PRIMARY KEY(\"ID\")");

            _command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableDescriptor.TableName} ({string.Join(", ", tableLines)})";
        }

        public int Execute()
        {
            return _command.ExecuteNonQuery();
        }
    }

    public static class CreateIfNotExistsCommandExtensions
    {
        public static CreateIfNotExistsCommand CreateCreateIfNotExistsCommand(this SqliteConnection connection, TableDescriptor tableDescriptor)
            => new CreateIfNotExistsCommand(connection, tableDescriptor);
    }
}
