using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Sqlite;

namespace Xisrith.StructuredLogging.Sqlite.Schema
{
    public class ColumnDescriptor
    {
        public string ColumnName { get; set; }
        public string ParameterName { get; set; }
        public PropertyDescriptor Property { get; set; }
        public SqliteType SqliteType { get; set; }

        public SqliteParameter CreateSqlParameter(SqliteCommand command)
        {
            var parameter = command.CreateParameter();
            parameter.IsNullable = true;
            parameter.ParameterName = ParameterName;
            parameter.SqliteType = SqliteType;
            return parameter;
        }

        public void UpdateSqlParameter(SqliteParameter parameter, object value)
        {
            var v = value != null ? Property.GetValue(value) : null;
            parameter.Value = v ?? DBNull.Value;
        }
    }
}
