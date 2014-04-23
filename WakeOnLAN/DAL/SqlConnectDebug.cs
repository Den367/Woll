using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;


namespace WakeOnLAN.DAL
{
    /// <summary>
    /// Класс - Предназначен для отладочных целей.
    /// </summary>
    public sealed class SqlConnectDebug
    {
        #region fields
        /// <summary>
        /// Экземпляр класса предоставляющего интерфейс доступа к базе данных.
        /// </summary>
        IConnect _Connect;
        /// <summary>
        /// Хеш таблица открытых соеденений
        /// </summary>
        static Hashtable _currentConnects = new Hashtable();
        #endregion

        #region properties
        #region ConnectCount
        /// <summary>
        /// Get - Количество соеденения с базой данных.
        /// </summary>
        public static int ConnectCount
        {
            get
            {
                return _currentConnects.Count;
            }
        }
        #endregion
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="conn">Экземпля класса CConnect.</param>
        public SqlConnectDebug(IConnect conn)
        {
            if (conn.SqlConnection == null)
            {
                throw new Exception("Не правильный вызов отладочного класса. Свойство conn у класса CConnect должно быть проинициализировано.");
            }
            _Connect = conn;
            _Connect.SqlConnection.StateChange += new StateChangeEventHandler(conn_StateChange);
        }
        #endregion

        #region methods
        #region Unload(int threadID)
        /// <summary>
        /// Проверка перед закрытием потока, не осталось ли в нем открытых соеденений.
        /// </summary>
        /// <param name="threadID">Идентификатор потока проверяемого на оставшиеся в нем открытые соеденения.</param>
        public static void Unload(int threadID)
        {
            lock (_currentConnects)
            {
                if (_currentConnects.Count > 0)
                {
                    IDictionaryEnumerator i = _currentConnects.GetEnumerator();
                    IConnect conn = null;
                    while (i.MoveNext())
                    {
                        if (i.Key.ToString().Contains("__" + threadID.ToString()))
                        {
                            if ((i.Value as IConnect).State == ConnectionState.Open)
                            {
                                conn = (i.Value as IConnect);
                            }
                        }
                    }
                    if (conn != null)
                    {
                        _currentConnects.Remove(conn.ConnectionString);
                        conn.Close();
                        throw new Exception("Вы забыли закрыть соденение с базой данных.\r\n\r\n\r\nStack Info:\r\n" + System.Environment.StackTrace);
                    }
                }
            }
        }
        #endregion

        #region conn_StateChange(object sender, StateChangeEventArgs e)
        /// <summary>
        /// Обработчик изменения состояния соеденения с базой данных.
        /// </summary>
        private void conn_StateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Open)
            {
                if (_Connect.SqlConnection.State == ConnectionState.Open)
                {
                    if (_currentConnects.ContainsKey(_Connect.ConnectionString + "__" + Thread.CurrentThread.ManagedThreadId.ToString()))
                    {
                        if ((_currentConnects[_Connect.ConnectionString + "__" + Thread.CurrentThread.ManagedThreadId.ToString()] as IConnect).State == ConnectionState.Open)
                        {
                            _Connect.SqlConnection.Close();
                            throw new Exception("Вы пытаетесь открыть два соеденения в одном потоке к одной базе данных.\r\n\r\n\r\nStack Info:\r\n" + System.Environment.StackTrace);
                        }
                    }
                    else
                    {
                        _currentConnects.Add(_Connect.ConnectionString + "__" + Thread.CurrentThread.ManagedThreadId.ToString(), _Connect);
                    }
                }
            }
            if (e.CurrentState == ConnectionState.Closed)
            {
                if (_currentConnects.ContainsKey(_Connect.ConnectionString + "__" + Thread.CurrentThread.ManagedThreadId.ToString()))
                {
                    _currentConnects.Remove(_Connect.ConnectionString + "__" + Thread.CurrentThread.ManagedThreadId.ToString());
                }
            }
        }
        #endregion
        #endregion
    }
}
