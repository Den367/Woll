
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;


namespace WakeOnLAN.DAL
{
    public static class SqlCommandHelper
    {
        public static void AddParameter(this DbCommand command, string parameterName, SqlDbType dbType, object value)
        {
            command.Parameters.Add(new SqlParameter(parameterName, dbType) { Value = value ?? DBNull.Value });
        }

        public static DbDataReader ExecuteReader(this DbCommand command, bool autoReset)
        {
            var reader = command.ExecuteReader();

            if (autoReset && reader.HasRows)
            {
                reader.Read();
            }

            return reader;
        }

        public static DataTable GetFirstTable(this DbCommand command)
        {
            var ds = new DataSet();
            var adapter = new SqlDataAdapter(command as SqlCommand);

            adapter.Fill(ds);

            Debug.Assert(ds.Tables.Count == 1, string.Format("Хранимая процедура '{0}' вернула не предвиденный резултат.", command.CommandText));

            return ds.Tables[0];
        }

        public static DataSet GetDataSet(this DbCommand command)
        {
            var ds = new DataSet();
            var adapter = new SqlDataAdapter(command as SqlCommand);

            adapter.Fill(ds);

            return ds;
        }
    }
}
