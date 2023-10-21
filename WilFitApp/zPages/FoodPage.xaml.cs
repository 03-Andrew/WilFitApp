using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WilFitApp.MVVM.Model;
using WilFitApp.DataBase;
using System.Windows.Markup;

namespace WilFitApp.zPages
{
    /// <summary>
    /// Interaction logic for FoodPage.xaml
    /// </summary>
    public partial class FoodPage : Page
    {
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        DataTable dataTable;

        public FoodPage(String name)
        {
            InitializeComponent();
            Name_Lbl.Content = name;
            LoadDataIntoDataGrid();
        }
        private void LoadDataIntoDataGrid()
        {

            try
            {
                string query = $"SELECT * FROM FoodData";

                using (conn = con.getCon())
                {
                    conn.Open();
                    cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    foodTable1.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTxtBox1.Text.ToLower();

            // Use a DataView to filter the DataTable
            DataView dataView = dataTable.DefaultView;

            if (!string.IsNullOrEmpty(searchText))
            {
                // Apply a filter to the dataView based on the search text
                dataView.RowFilter = $"FoodName LIKE '%{searchText}%'";
            }
            else
            {
                // Clear the filter if the search text is empty
                dataView.RowFilter = string.Empty;
            }

            // Update the DataGrid with the filtered data
            foodTable1.ItemsSource = dataView;
        }
    }
}
