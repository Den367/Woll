
using System;
using System.Data;
using System.Data.SqlClient;

namespace WakeOnLAN.DAL
{
    public class DBConnect : IConnect, IDisposable
	{
		SqlConnection _connection;
		SqlTransaction _transaction;

		public string ConnectionString
		{
			get
			{
				return _connection.ConnectionString;
			}
		}

		public SqlTransaction SqlTransaction
		{
			get
			{
				return _transaction;
			}
		}

		public SqlConnection SqlConnection
		{
			get
			{
				return _connection;
			}
		}

		public ConnectionState State
		{
			get
			{
				return _connection.State;
			}
		}

		public void Open()
		{
			_connection.Open();
		}

		public void Close()
		{
			_connection.Close();
		}

		public void BeginTransaction()
		{
			if (_transaction == null)
			{
				_transaction = _connection.BeginTransaction();
			}
		}

		public void Commit()
		{
			if (_transaction != null)
			{
				_transaction.Commit();
			}
		}

		public DBConnect(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
		}

        public DBConnect(SqlConnection connection)
		{
			_connection = connection;
		}

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }

            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}
