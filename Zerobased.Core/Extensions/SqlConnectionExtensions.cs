using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Zerobased.Extensions
{
    public static class SqlConnectionExtensions
    {
        public static SqlCommand CreateCommand(this SqlConnection connection, string commandText)
        {
            return CreateCommand(connection, commandText, CommandType.Text);
        }

        public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, object @params)
        {
            return CreateCommand(connection, commandText, CommandType.Text, @params);
        }

        public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, CommandType commandType, object @params = null)
        {
            var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = commandText;
            sqlCommand.CommandType = commandType;

            if (@params != null)
            {
                sqlCommand.AddParameters(@params);
            }

            return sqlCommand;
        }

        public static SqlCommand CreateInsertCommand(this SqlConnection connection, string tableName, object obj, SqlTransaction transaction = null, ICollection<string> exclude = null)
        {
            var sqlCommand = connection.CreateCommand();
            sqlCommand.Transaction = transaction;
            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propertyNames = new List<string>(properties.Length);
            var columnsNames = new List<string>(properties.Length);

            if (exclude == null)
            {
                exclude = new string[] { };
            }

            foreach (PropertyInfo property in properties.Where(pr => !exclude.Contains(pr.Name)))
            {
                propertyNames.Add(property.Name);
                columnsNames.Add(GetColumnName(property));
                var value = property.PropertyType.IsEnum
                    ? (int)property.GetValue(obj)
                    : property.GetValue(obj);

                var sqlParameter = sqlCommand.Parameters.AddWithValue(property.Name, value);
                if (value == null)
                {
                    sqlParameter.Value = DBNull.Value;
                }
            }

            sqlCommand.CommandText = "Insert Into {0} ({1}) VALUES ({2});SELECT SCOPE_IDENTITY();"
                .FormatWith(tableName, columnsNames.Join(",", "[{0}]"), propertyNames.Join(",", "@{0}"));

            return sqlCommand;
        }

        private static string GetColumnName(PropertyInfo propInfo)
        {
            var columnAttribute = propInfo.GetCustomAttribute<ColumnAttribute>();
            return columnAttribute == null ? propInfo.Name : columnAttribute.Name;
        }
    }
}
