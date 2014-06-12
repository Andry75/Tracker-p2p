using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;

namespace Tracker
{
    class Conector
    {
       

        private SqlConnection create_con()
        {
            // Функция приватного типа «create_con» отвичает за создание и потдержания соединения с базой даных.
            string connectionString = "user id=P2P_Server;" + "password=123;server= WIN-RGJKGPSWNNS;" + "Trusted_Connection=false;" + "database=P2P; " + "connection timeout=30";
            // Слздаем переменую текстового типа и присваиваем значение. В эту переменую записываем строку подключения к базе данных 
            SqlConnection connect = new SqlConnection(connectionString);
            // Создаем обьект «connect» типа «OleDbConnection» который выполняет подключение к базе данных.
            return connect;
            // Возвращяем подключение.
            
        }

        public SqlDataReader select(string comand)
        {
            
            // Функция публичного типа «select» возвращяющяя обьект типа «OleDbDataReade», отвичает за выполнение запросов к базе данных типа «select» .
            SqlConnection con = this.create_con();
            // Создаем подключение к базе данных
            SqlCommand sel = new SqlCommand(comand, con);
            sel.CommandType = CommandType.Text;
            // Создаем обьект «sel» типа «OleDbCommand», записываем в него запрос и предаем ссылку на открытое подключение
            try
            {
                con.Open();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("No connection to the database server");
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                Console.WriteLine("No connection to the database server");
            }
            // Открываем подключение к базе данных
            SqlDataReader r = sel.ExecuteReader();
            // Записываем в обьект «r» типа «OleDbDataReader» значения полученые из базы данных
            return r;
            // Возвращяем обьект «r»

        }
        public void con_close()
        {
            // Функция публичного типа «con_close» , отвичает за закрытия подключения к базе данных .
            SqlConnection connect = this.create_con();
            connect.Close();
            // Закрываем подключение
        }
        public void insert(string comand)
        {
            // Функция публичного типа «insert» , отвичает за выполнение запросов к базе данных типа «insert» .
            try
            // Деректива «try» отлавливает возможные ошибки при выполнении данного кода
            {
                SqlConnection con = this.create_con();
                // Создаем подключение к базе данных
                SqlCommand insert = new SqlCommand(comand, con);
                insert.CommandType = CommandType.Text;
                // Создаем обьект «insert» типа «OleDbCommand», записываем в него запрос и предаем ссылку на открытое подключение
                con.Open();
                // Открываем подключение к базе данных
                insert.ExecuteNonQuery();
                // Указываем параметр говорящий о том что, запрос не запрашивает значения, а вносит изменения в базу данных 
                this.con_close();
                // Закрываем подключение
            }
            catch
            // Деректива «catch» выполняет следующий код в случае ошибки
            {
                Console.WriteLine("Недопустимая ошибка ввода-вывода!");
                // Выводим на экран диалоговое окно с сообщением об ошибке
            }
        }
        public void delete(string comand)
        {
            // Функция публичного типа «delete» , отвичает за выполнение запросов к базе данных типа «delete» 
            SqlConnection con = this.create_con();
            // Создаем подключение к базе данных
            SqlCommand delete = new SqlCommand(comand, con);
            delete.CommandType = CommandType.Text;
            // Создаем обьект «delete» типа «OleDbCommand», записываем в него запрос и предаем ссылку на открытое подключение
            con.Open();
            // Открываем подключение к базе данных
            delete.ExecuteNonQuery();
            // Указываем параметр говорящий о том что, запрос не запрашивает значения, а вносит изменения в базу данных 
            this.con_close();
            // Закрываем подключение
        }
        public void update(string comand)
        {
            // Функция публичного типа «update» , отвичает за выполнение запросов к базе данных типа «update»
            SqlConnection con = this.create_con();
            // Создаем подключение к базе данных
            SqlCommand update = new SqlCommand(comand, con);
            update.CommandType = CommandType.Text;
            // Создаем обьект «update» типа «OleDbCommand», записываем в него запрос и предаем ссылку на открытое подключение
            con.Open();
            // Открываем подключение к базе данных
            update.ExecuteNonQuery();
            // Указываем параметр говорящий о том что, запрос не запрашивает значения, а вносит изменения в базу данных
            this.con_close();
            // Закрываем подключение
        }
    }        
}
