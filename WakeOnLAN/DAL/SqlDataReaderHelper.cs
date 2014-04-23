using System;
using System.Data;
using System.Data.Common;

namespace WakeOnLAN.DAL
{
    public static class SqlDataReaderHelper
    {
        public static T GetValue<T>(this DbDataReader reader, string columnName)
        {
            object obj = reader[columnName];
            if (obj is T)
            {
                return (T)obj;
            }
            else
            {
                if (obj == null || obj == DBNull.Value)
                {
                    Type t = typeof(T);
                    if (t == typeof(string) || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) || t.IsArray)
                    {
                        object obj1 = null;
                        return (T)obj1;
                    }
                    throw new Exception(string.Format("Не удалось привести значение({0}) к следующему типу данных - {1}", obj, typeof(T).ToString()));
                }
                return (T)Convert.ChangeType(obj, typeof(T));
            }
        }

        public static T GetEnum<T>(this DbDataReader reader, string columnName)
        {
            object obj = reader[columnName];
            object obj1 = (T)Convert.ChangeType(obj, Enum.GetUnderlyingType(typeof(T)));
            if (!Enum.IsDefined(typeof(T), obj1))
            {
                throw new Exception(string.Format("Не удалось привести значение({0}) к следующему типу enum - {1}", obj, typeof(T).ToString()));
            }
            return (T)obj1;
        }

        public static T GetEnum<T>(this DbDataReader reader, string columnName, T defaultValue)
        {
            var value = reader[columnName];
            if (value == null || value == DBNull.Value) return defaultValue;
            var enumValue = (T)Convert.ChangeType(value, Enum.GetUnderlyingType(typeof(T)));
            return Enum.IsDefined(typeof(T), enumValue) ? enumValue : defaultValue;
        }

        public static T GetValue<T>(this DataRow row, string columnName)
        {
            object obj = row[columnName];
            if (obj is T)
            {
                return (T)obj;
            }
            else
            {
                if (obj == null || obj == DBNull.Value)
                {
                    Type t = typeof(T);
                    if (t == typeof(string) || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) || t.IsArray)
                    {
                        object obj1 = null;
                        return (T)obj1;
                    }
                    throw new Exception(string.Format("Не удалось привести значение({0}) к следующему типу данных - {1}", obj, typeof(T).ToString()));
                }
                return (T)Convert.ChangeType(obj, typeof(T));
            }
        }

        public static T GetEnum<T>(this DataRow row, string columnName)
        {
            object obj = row[columnName];
            object obj1 = (T)Convert.ChangeType(obj, Enum.GetUnderlyingType(typeof(T)));
            if (!Enum.IsDefined(typeof(T), obj1))
            {
                throw new Exception(string.Format("Не удалось привести значение({0}) к следующему типу enum - {1}", obj, typeof(T).ToString()));
            }
            return (T)obj1;
        }

        public static T GetEnum<T>(this DataRow row, string columnName, T defaultValue)
        {
            var value = row[columnName];
            var enumValue = (T)Convert.ChangeType(value, Enum.GetUnderlyingType(typeof(T)));
            return Enum.IsDefined(typeof(T), enumValue) ? enumValue : defaultValue;
        }
    }
}
