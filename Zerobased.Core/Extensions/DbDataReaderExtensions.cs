using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Zerobased.Extensions
{
    public static class DbDataReaderExtensions
    {
        public static string GetString(this DbDataReader reader, string colName, string @default = null)
        {
            int index = reader.GetOrdinal(colName);
            string value = reader[index] == null || reader[index] == DBNull.Value 
                ? null 
                : reader.GetString(index);
            return value ?? @default;
        }

        public static bool? GetBooleanOrNull(this DbDataReader reader, string colName)
        {
            return GetValue<bool>(reader, colName, true);
        }

        public static bool GetBoolean(this DbDataReader reader, string colName)
        {
            // ReSharper disable once PossibleInvalidOperationException
            // It'll throw exception in NULL case
            return GetValue<bool>(reader, colName, false).Value;
        }

        public static Int32? GetInt32OrNull(this DbDataReader reader, string colName)
        {
            return GetValue<Int32>(reader, colName, true);
        }

        public static Int32 GetInt32(this DbDataReader reader, string colName)
        {
            // ReSharper disable once PossibleInvalidOperationException
            // It'll throw exception in NULL case
            return GetValue<Int32>(reader, colName, false).Value;
        }

        public static Int64? GetInt64OrNull(this DbDataReader reader, string colName)
        {
            return GetValue<Int64>(reader, colName, false);
        }

        public static Int64 GetInt64(this DbDataReader reader, string colName)
        {
            // ReSharper disable once PossibleInvalidOperationException
            // It'll throw exception in NULL case
            return GetValue<Int64>(reader, colName, false).Value;
        }

        public static DateTime? GetDateTimeOrNull(this DbDataReader reader, string colName)
        {
            return GetValue<DateTime>(reader, colName, true);
        }

        public static DateTime GetDateTime(this DbDataReader reader, string colName)
        {
            // ReSharper disable once PossibleInvalidOperationException
            // It'll throw exception in NULL case
            return GetValue<DateTime>(reader, colName, false).Value;
        }

        public static decimal? GetDecimalOrNull(this DbDataReader reader, string colName)
        {
            return GetValue<decimal>(reader, colName, true);
        }

        public static decimal GetDecimal(this DbDataReader reader, string colName)
        {
            // ReSharper disable once PossibleInvalidOperationException
            // It'll throw exception in NULL case
            return GetValue<decimal>(reader, colName, false).Value;
        }

        public static List<T> ToList<T>(this DbDataReader reader, Func<DbDataReader, T> selector)
        {
            var list = new List<T>();
            while (reader.Read())
            {
                list.Add(selector(reader));
            }
            return list;
        }

        public static async Task<List<T>> ToListAsync<T>(this DbDataReader reader, Func<DbDataReader, T> selector)
        {
            var list = new List<T>();
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                list.Add(selector(reader));
            }
            return list;
        }

        public static T FirstOrDefault<T>(this DbDataReader reader, Func<DbDataReader, T> selector)
        {
            return reader.Read() ? selector(reader) : default(T);
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this DbDataReader reader, Func<DbDataReader, T> selector)
        {
            return await reader.ReadAsync().ConfigureAwait(false) 
                ? selector(reader) 
                : default(T);
        }

        private static T? GetValue<T>(DbDataReader reader, string colName, bool allowNull) where T : struct
        {
            int index = reader.GetOrdinal(colName);
            object value = reader[index];

            if (value == null || value is DBNull)
            {
                if (!allowNull)
                {
                    throw new InvalidOperationException("Column [{0}] contains null, it can't be used for type {1}.".FormatWith(colName, typeof(T).Name));
                }

                return null;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
