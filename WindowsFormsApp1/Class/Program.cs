using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Поварёнок
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            DBhelper.connection = new SqlConnection(); //Создание объекта подключения
            DBhelper.connection.ConnectionString = DBhelper.connectionString;
            try
            {
                DBhelper.connection.Open();      //Опасная команда
                Application.Run(new FormLogin());
            }
            catch (SqlException ex)     //Обработка сбоя при подключении
            {
                MessageBox.Show("Ошибка подключения к БД");
            }
        }
    }
}
