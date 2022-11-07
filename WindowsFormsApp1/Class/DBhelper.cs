using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Поварёнок
{
    internal class DBhelper
    {
        public static string connectionString = @"Data Source=laptop\SQLEXPRESS;Initial Catalog=Pedich02;Integrated Security=True";  //Строка подключения
        public static SqlConnection connection;
        public static int id;
        public static int idRole;
    }
}
