using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITService.Data.SqlServer
{
    internal class DataBase
    {
        
        
            /// Объявление строки подключения с БД
            SqlConnection sqlConnection = new SqlConnection(@"Server=Dbsrv\GOR2025;Database=ITServiceRepairDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;encr
      ypt=false");
            /// Метод для открытия соединения с БД
            public void OpenConnection()
            {
                // если состояние строки закрыто, то открываем
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
            }
            /// Метод для закрытия соединения с БД, обратный методу выше

            public void CloseConnection()
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            /// Метод, возвращающий строку подключения
            public SqlConnection GetConnection()
            {
                return sqlConnection;
            }
            /// Метод для обработки запросов типа Select
            public DataTable SqlSelect(string s)
            {
                SqlCommand command = new SqlCommand(s); // создаем команду с запросом
                command.Connection = GetConnection(); // открываем соединение с БД
                SqlDataAdapter adapter = new SqlDataAdapter(command); // адаптируем данные
                DataTable table = new DataTable(); // создаем таблицу
                adapter.Fill(table); // через адаптер заполняем ее данными
                return table; // возвращаем таблицу
            }
            /// Метод для обработки запросов типа Insert
            public SqlCommand SqlInsert(string querystring)
            {
                OpenConnection(); // открыли соединение
                SqlCommand command = new SqlCommand(querystring); //передали команду
                command.Connection = GetConnection(); //передали строку подключения
                command.ExecuteNonQuery(); // просто выполняет sql-выражение и возвращает количество
                
         // Подходит для sql-выражений INSERT, UPDATE, DELETE, CREATE.
         CloseConnection(); // закрыли соединение
                return command; // вернули команду
            }
        }
    }

