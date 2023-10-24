using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using WilFitApp.DataBase;
using WilFitApp.MVVM.Model;

namespace WilFitApp.zPages
{
    /// <summary>
    /// Interaction logic for FoodPage1.xaml
    /// </summary>
    public partial class FoodPage1 : Page
    {
        

        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        DataTable dataTable;

        private double caloriesConsumed = 0;
        public int newCalories;
        double _cal;
        string _name;

        public FoodPage1(string name , double cal)
        {
            try
            {
                InitializeComponent();
                _name = name;
                _cal = cal;
                //updateProgressBar();
                //LoadDataIntoDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void EnterCaloriesTxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (foodInput.Text == "Enter Food")
            {
                foodInput.Text = "";
            }
        }

        private void EnterCaloriesTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(foodInput.Text))
            {
                foodInput.Text = "Enter Food";
            }
        }

        private void addFoodBtn_Click(object sender, RoutedEventArgs e)
        {
            Foods food = new Foods();
            food.name = foodTxtBox.Text;
            food.measurment = measurementTxtBox.Text;
            food.servingSize = Convert.ToDouble(servingSizeTxtBox.Text);
            food.calPerServing = Convert.ToDouble(calPerServingTxtBox.Text);

            using (conn = con.getCon())
            {
                conn.Open();
                string query = $"insert into FoodData values('{food.name}', {food.servingSize}, '{food.measurment}', {food.calPerServing})";
                using (cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Success");
                    LoadDataIntoDataGrid();

                }
            }
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

        private void AddCalories_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (conn = con.getCon())
                {
                    conn.Open();

                    // Use a parameterized query to prevent SQL injection
                    SqlCommand selectCmd = new SqlCommand("SELECT calories FROM calorieHistory WHERE userName = @name", conn);
                    selectCmd.Parameters.AddWithValue("@name", _name);

                    double currentValue = Convert.ToDouble(selectCmd.ExecuteScalar());

                    // Use a parameterized query to retrieve the calorie value for the given food
                    SqlCommand sqlCmd = new SqlCommand("SELECT calPerServing FROM FoodData WHERE foodName = @foodName", conn);
                    sqlCmd.Parameters.AddWithValue("@foodName", foodInput.Text);

                    double foodCalories = Convert.ToDouble(sqlCmd.ExecuteScalar());

                    double updatedValue = currentValue + foodCalories;
                    DateTime currDate = DateTime.Now;

                    // Update the calories in the calorieHistory table
                    SqlCommand updateCmd = new SqlCommand("UPDATE calorieHistory SET calories = @updatedValue WHERE userName = @name", conn);
                    updateCmd.Parameters.AddWithValue("@updatedValue", updatedValue);
                    updateCmd.Parameters.AddWithValue("@name", _name);

                    updateCmd.ExecuteNonQuery();

                    conn.Close();

                    updateProgressBar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        public void updateProgressBar()
        {
            try
            {
                using (SqlConnection conn = con.getCon())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT calories FROM calorieHistory WHERE userName = '{_name}'", conn);


                    object result = cmd.ExecuteScalar();

                    if (result != null && double.TryParse(result.ToString(), out double progressValue))
                    {
                        caloriesLabel.Content = progressValue.ToString();
                        progress.Value = progressValue;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as displaying an error message or logging the error.
                MessageBox.Show("Error: " + ex.Message);
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
