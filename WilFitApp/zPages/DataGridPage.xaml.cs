using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using WilFitApp.DataBase;
using System.Xml.Linq;

namespace WilFitApp.zPages
{
    /// <summary>
    /// Interaction logic for DataGridPage.xaml
    /// </summary>
    public partial class DataGridPage : Page
    {
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader reader;

        public DataGridPage(string name)
        {
            InitializeComponent();
            LoadDataIntoDataGrid(name);
            
        }
        private void LoadDataIntoDataGrid(string name)
        {

            try
            {
                string query = $"SELECT * FROM FoodData";

                using (conn = con.getCon())
                {
                    conn.Open();
                    cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    Foods.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error"  + ex);
                // Handle the exception (e.g., log it, display an error message)
            }
        }

        private void checkTableBtn_Click(object sender, RoutedEventArgs e)
        {
            string food = FoodTxtBox.Text.ToLower();
            using (conn = con.getCon())
            {
                conn.Open();
                string query = "SELECT * FROM FoodData WHERE foodName = @foodName";
                using (cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@foodName", food);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inputTypeLbl.Content = "Enter amount in " + reader["measurement"].ToString();
                            foodName.Content = reader["foodName"].ToString();
                            calories.Content = reader["calPerServing"].ToString();
                        }
                        else
                        {
                            // Display a message box when the information is not found
                            MessageBox.Show("Food not found in the database.");
                        }
                    }
                }
            }
        }

    }

}
