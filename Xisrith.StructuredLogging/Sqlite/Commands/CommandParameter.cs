using Microsoft.Data.Sqlite;
using Xisrith.StructuredLogging.Sqlite.Schema;

namespace Xisrith.StructuredLogging.Sqlite.Commands
{
    class CommandParameter
    {
        private readonly ColumnDescriptor _columnDescriptor;
        private readonly SqliteParameter _sqliteParameter;

        public CommandParameter(ColumnDescriptor columnDescriptor, SqliteParameter sqliteParameter)
        {
            _columnDescriptor = columnDescriptor;
            _sqliteParameter = sqliteParameter;
        }

        public void Set(object value)
        {
            _columnDescriptor.UpdateSqlParameter(_sqliteParameter, value);
        }
    }
}
