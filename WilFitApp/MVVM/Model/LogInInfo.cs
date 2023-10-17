using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WilFitApp.DataBase;

namespace WilFitApp.MVVM.Model
{

    public class LogInInfo
    {
        public string Name { get; set; }
        /*
        connection con = new connection();
        SqlConnection conn;
        public string Name { get; set; }
        public string Password { get; set; }

        public bool isValid()
        { 
            try
            {
                using (conn = con.getCon())
                {

                    conn.Open();

                    string query = "SELECT COUNT(*) FROM [dbo].[UserInfo] WHERE userName=@userName AND password=@password";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@userName", Name);
                    cmd.Parameters.AddWithValue("@password", Password);

                    int count = (int)cmd.ExecuteScalar();
                    


                    if (count > 0)
                    {
                        // Successful login
                        //LoggedIn li = new LoggedIn(Name)
                        //li.ShowDialog();
                        //this.Close();
                        return true;

                    }
                    else
                    {
                        // Invalid login, handle appropriately (e.g., show an error message).
                        MessageBox.Show("Invalid username or password. Please try again.");
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }
        */
        

    }
    
}
