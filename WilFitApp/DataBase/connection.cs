using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;


namespace WilFitApp.DataBase
{
    internal class connection
    {
        SqlConnection conn;
        public SqlConnection getCon()
        {
            conn = new SqlConnection("Data Source=LAPTOP-KUFQMU26\\SQLEXPRESS;Initial Catalog=WilFitApp;Integrated Security=True");
            return conn;
        }
    }
}
