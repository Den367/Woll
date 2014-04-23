
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WakeOnLAN.DAL
{
	/// <summary>
	/// Интерфейс доступа серверу баз данных
	/// </summary>
	public interface IConnect
	{
		#region properties
		/// <summary>
		/// Get - Строка соеденения с базой данных.
		/// </summary>
		string ConnectionString { get; }
		/// <summary>
		/// Get - Транзакция которую будут испольовать все методы класса.
		/// </summary>
		SqlTransaction SqlTransaction { get; }
		/// <summary>
		/// Get - Экземпля класса SqlConnection
		/// </summary>
		SqlConnection SqlConnection { get; }
		/// <summary>
		/// Состояние соеденения
		/// </summary>
		ConnectionState State { get; }
		#endregion

		#region methods
		/// <summary>
		/// Открытие соеденения с сервером.
		/// </summary>
		void Open();
		/// <summary>
		/// Закрытие соеденения с сервером.
		/// </summary>
		void Close();
		/// <summary>
		/// Начало транзакции.
		/// </summary>
		/// <returns>true - если открыте состоялось, false в противном случае</returns>
		void BeginTransaction();
		/// <summary>
		/// Фиксация текущей открытой транзакции
		/// </summary>
		void Commit();
		#endregion
	}
}
