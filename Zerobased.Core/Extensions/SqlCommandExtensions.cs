using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace Zerobased.Extensions
{
    public static class SqlCommandExtensions
    {
        public static List<SqlParameter> AddListParameter(this SqlCommand sqlCommand, string paramName, IEnumerable values, string delim = null)
        {
            delim = delim ?? ",";
            var parameters = new List<SqlParameter>();
            int index = -1;
            var names = new List<string>();

            foreach (var value in values)
            {
                string name = paramName + "__" + (++index);
                names.Add(name);
                parameters.Add(sqlCommand.Parameters.AddWithValue(name, value));
            }

            string templateName = "{" + paramName + "}";
            sqlCommand.CommandText = sqlCommand.CommandText.Replace(templateName, names.Join(delim));
            return parameters;
        }

        public static SqlCommand AddParameters(this SqlCommand sqlCommand, object @params)
        {
            var type = @params.GetType();
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(@params);
                if (!(value is string) && value is IEnumerable)
                {
                    AddListParameter(sqlCommand, property.Name, (IEnumerable)value);
                }
                else
                {
                    var sqlParameter = sqlCommand.Parameters.AddWithValue(property.Name, value);

                    if (value == null)
                    {
                        sqlParameter.Value = DBNull.Value;
                    }
                }
            }

            return sqlCommand;
        }

        public static T ExecuteFirstOrDefault<T>(this SqlCommand sqlCommand, Func<DbDataReader, T> selector)
        {
            using (var reader = sqlCommand.ExecuteReader())
            {
                return reader.FirstOrDefault(selector);
            }
        }

        public static async Task<T> ExecuteFirstOrDefaultAsync<T>(this SqlCommand sqlCommand, Func<DbDataReader, T> selector)
        {
            using (var reader = await sqlCommand.ExecuteReaderAsync().ConfigureAwait(false))
            {
                return await reader.FirstOrDefaultAsync(selector).ConfigureAwait(false);
            }
        }

        public static List<T> ExecuteList<T>(this SqlCommand sqlCommand, Func<DbDataReader, T> selector)
        {
            using (var reader = sqlCommand.ExecuteReader())
            {
                return reader.ToList(selector);
            }
        }

        public static async Task<List<T>> ExecuteListAsync<T>(this SqlCommand sqlCommand, Func<DbDataReader, T> selector)
        {
            using (var reader = await sqlCommand.ExecuteReaderAsync().ConfigureAwait(false))
            {
                return await reader.ToListAsync(selector).ConfigureAwait(false);
            }
        }

    }
}
