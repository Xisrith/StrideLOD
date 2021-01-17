using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Xisrith.StructuredLogging.Sqlite.Schema;

namespace Xisrith.StructuredLogging.Sqlite.Commands
{
    public class InsertCommand
    {
        private readonly SqliteCommand _command;
        private readonly IEnumerable<CommandParameter> _parameters;

        public InsertCommand(SqliteConnection connection, TableDescriptor tableDescriptor)
        {
            var cnames = new List<string>();
            var pnames = new List<string>();
            var parameters = new List<CommandParameter>();

            var command = connection.CreateCommand();
            foreach (var descriptor in tableDescriptor.ColumnDescriptors)
            {
                cnames.Add(descriptor.ColumnName);
                pnames.Add(descriptor.ParameterName);

                var parameter = descriptor.CreateSqlParameter(command);
                command.Parameters.Add(parameter);

                var cparam = new CommandParameter(descriptor, parameter);
                parameters.Add(cparam);
            }
            command.CommandText = GetCommandText(tableDescriptor.TableName, cnames, pnames);

            _command = command;
            _parameters = parameters;
        }

        public int Execute(Log log)
        {
            foreach(var parameter in _parameters)
            {
                parameter.Set(log);
            }
            return _command.ExecuteNonQuery();
        }

        private string GetCommandText(string tableName, IEnumerable<string> columnNames, IEnumerable<string> parameterNames)
            => $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", parameterNames)})";
    }

    public static class InsertCommandExtensions
    {
        public static InsertCommand CreateInsertCommand(this SqliteConnection connection, TableDescriptor tableDescriptor)
            => new InsertCommand(connection, tableDescriptor);
    }
}
