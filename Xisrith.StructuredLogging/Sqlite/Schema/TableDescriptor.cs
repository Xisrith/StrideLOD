using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Sqlite;

namespace Xisrith.StructuredLogging.Sqlite.Schema
{
    public class TableDescriptor
    {
        public TableDescriptor(Type type)
        {
            ColumnDescriptors = CreateColumnDescriptors(type);
            TableName = $"\"{GetTableName(type)}\"";
        }

        public IEnumerable<ColumnDescriptor> ColumnDescriptors { get; }
        public string TableName { get; }

        private List<ColumnDescriptor> CreateColumnDescriptors(Type type)
        {
            var descriptors = new List<ColumnDescriptor>();
            var properties = GetProperties(type);

            foreach (PropertyDescriptor property in properties)
            {
                var propName = property.Name;
                var sqlType = SqliteType.Text;
                var hasAttribute = false;
                foreach (Attribute attribute in property.Attributes)
                {
                    if (attribute.GetType() == typeof(ColumnAttribute))
                    {
                        var attr = (ColumnAttribute)attribute;

                        propName = GetColumnName(property, attr);
                        sqlType = GetColumnType(property, attr);
                        hasAttribute = true;
                        break;
                    }
                }

                if (!hasAttribute)
                    continue;

                var columnName = $"\"{propName}\"";
                var paramName = $"${propName}";

                var descriptor = new ColumnDescriptor
                {
                    ColumnName = columnName,
                    ParameterName = paramName,
                    Property = property,
                    SqliteType = sqlType
                };

                descriptors.Add(descriptor);
            }

            return descriptors;
        }


        private string GetColumnName(PropertyDescriptor property, ColumnAttribute attribute)
        {
            return string.IsNullOrEmpty(attribute.TypeName) ? property.Name : attribute.TypeName;
        }

        private SqliteType GetColumnType(PropertyDescriptor property, ColumnAttribute attribute)
        {
            var type = property.PropertyType;
            var name = attribute.Name;

            if (!string.IsNullOrEmpty(name))
            {
                if (string.Equals(name, "blob", StringComparison.OrdinalIgnoreCase))
                    return SqliteType.Blob;
                if (string.Equals(name, "integer", StringComparison.OrdinalIgnoreCase))
                    return SqliteType.Integer;
                if (string.Equals(name, "real", StringComparison.OrdinalIgnoreCase))
                    return SqliteType.Real;
                if (string.Equals(name, "text", StringComparison.OrdinalIgnoreCase))
                    return SqliteType.Text;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return SqliteType.Integer;
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return SqliteType.Real;
                default:
                    return SqliteType.Text;
            }
        }

        
        /// <inheritdoc cref="GetProperties(Type, HashSet{string})"/>
        private IEnumerable<PropertyDescriptor> GetProperties(Type type) => GetProperties(type, new HashSet<string>());

        /// <summary>
        /// Lets us get properties in an order where parent class properties appear at the top of the results
        /// (which is the opposite of the default behavior).
        /// </summary>
        /// <remarks>
        /// This is done so the SQLite tables all start with the  default log properties.
        /// </remarks>
        private IEnumerable<PropertyDescriptor> GetProperties(Type type, HashSet<string> properties)
        {
            var props = new List<PropertyDescriptor>();
            if (type.BaseType != null)
            {
                props.AddRange(GetProperties(type.BaseType, properties));
            }

            var thisProperties = TypeDescriptor.GetProperties(type);
            foreach (PropertyDescriptor property in thisProperties)
            {
                if (!properties.Contains(property.Name))
                {
                    props.Add(property);
                    properties.Add(property.Name);
                }
            }

            return props;
        }

        private string GetTableName(Type type)
        {
            var defaultName = TypeDescriptor.GetClassName(type);
            var attributes = TypeDescriptor.GetAttributes(type);
            foreach (Attribute attribute in attributes)
            {
                if (attribute.GetType() == typeof(TableAttribute))
                {
                    var attr = (TableAttribute)attribute;
                    return string.IsNullOrEmpty(attr.Name) ? defaultName : attr.Name;
                }
            }

            return defaultName;
        }
    }
}
