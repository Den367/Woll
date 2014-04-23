
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;

namespace WakeOnLAN.DAL
{
	/// <summary>
	/// Предназначен для создания соединения с базой данных
	/// </summary>
	public class DBCntxt : IDisposable, IDBConnectType
	{
		#region Fields

		/// <summary>
		/// Экземпляр класса, предоставляющего интерфейс доступа к взаимодействию с базой данных
		/// </summary>
		IConnect _connection;

		/// <summary>
		/// Было ли открыто соединение в внутри данного класса или его отрыли извне и передали внутрь класса
		/// </summary>
		readonly bool _isInnerConnection;

		/// <summary>
		/// Были ли освобожденый ресурсы, занятые классом
		/// </summary>
		bool _isDisposed;

		/// <summary>
		/// Была ли открыта транзакция в экземпляре этого класса, или транзакция была открыта другими экземплярами данного класса
		/// </summary>
		bool _isInnerTransaction;

		#endregion

        #region Properties

        /// <summary>
        /// Строка соединения с базой данных
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connection.ConnectionString;
            }
        }

        /// <summary>
        /// Экземпляр класса, предоставляющего интерфейс доступа к взаимодействию с базой данных
        /// </summary>
        public IConnect Connect
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
				return _connection.SqlConnection.State;
			}
		}

        #endregion

        #region Constructor&Destructor

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dbConnectionType">Параметры соединения</param>       
	    public DBCntxt(IDBConnectType dbConnectionType)
	    {
            Debug.Assert(dbConnectionType != null, "Передан неинициализированный экземпляр класса параметров соеденения с БД");

            _isInnerConnection = false;
            _isInnerTransaction = false;
            _isDisposed = false;
            _connection = null;

            if (!(dbConnectionType is DBCntxt))
            {
                if (dbConnectionType.Connect == null)
                {
                    _connection = dbConnectionType.CreateConnect();
#if DEBUG
                    //new SqlConnectDebug(_connection);
#endif
                    _isInnerConnection = true;

                    _connection.Open();
                }
                else
                {
                    _connection = dbConnectionType.Connect;
                }

                if (_connection.State != ConnectionState.Open)
                {
                    throw new Exception("Не удалось открыть соеденение");
                }
            }
            else
            {
                _connection = ((DBCntxt)dbConnectionType)._connection;
            }

           

          
	    }

	    ~DBCntxt()
	    {
            Dispose(false);
        }

	    #endregion

		#region Methods

		public IConnect CreateConnect()
		{
			return _connection;
		}

	    public IConnect CreateConnect(string externalCS)
	    {
	        return CreateConnect();
	    }

	    /// <summary>
		/// Открытие транзакции
		/// </summary>
		public void BeginTransaction()
		{
			if (_connection.SqlTransaction == null)
			{
				_connection.BeginTransaction();
				_isInnerTransaction = true;
			}
		}

		/// <summary>
		/// Фиксация транзакции
		/// </summary>
		public void CommitTransaction()
		{
			if (_isInnerTransaction)
			{
				_connection.Commit();
			}
		}

		/// <summary>
		/// Создание объекта DbCommand для хранимой процедуры (инициализированного текущими соединением и транзакцией)
		/// </summary>
		/// <returns>Экземпляр созданного класса DbCommand</returns>
		public DbCommand CreateCommand(string spName)
		{
			var cmd = new SqlCommand
			{
			    CommandText = spName,
			    CommandType = CommandType.StoredProcedure,
			    Connection = _connection.SqlConnection,
			    Transaction = _connection.SqlTransaction
			};

		    return cmd;
		}

        /// <summary>
        /// Создание объекта DbCommand для текстового запроса (инициализированного текущими соединением и транзакцией)
        /// </summary>
        /// <returns>Экземпляр созданного класса DbCommand</returns>
        public DbCommand CreateTextCommand(string commandText)
        {
            var cmd = new SqlCommand
            {
                CommandText = commandText,
                CommandType = CommandType.Text,
                Connection = _connection.SqlConnection,
                Transaction = _connection.SqlTransaction
            };

            return cmd;
        }

		/// <summary>
		/// Закрытие соединения
		/// </summary>
		public void Close()
		{
			Dispose();
		}

        /// <summary>
        /// Освобождение памяти объекта
        /// </summary>
        private void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                if (isDisposing)
                {
                    // Очищаем память управляемых ресурсов
                }

                // Очищаем память неуправляемых ресурсов

                if (_isInnerConnection)
                {
                    _connection.Close();
                }

                _connection = null;

                _isDisposed = true;
            }
        }

        /// <summary>
		/// Освобождение памяти объекта
		/// </summary>
		public void Dispose()
		{
            Dispose(true);

            GC.SuppressFinalize(this);
        }

		#endregion
	}
}
