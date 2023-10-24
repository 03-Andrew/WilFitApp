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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private double caloriesConsumed = 0;
       
        connection con = new connection();
        double _cal;
        SqlConnection conn;
        SqlCommand cmd;
        string _name;
        public int newCalories;
        DataTable dataTable;


        public HomePage(string name, double cal)
        {
            try
            {
                InitializeComponent();

                //SetLabel2();
                _name = name;
                _cal = cal;
                updateProgressBar();
                updateWaterProgressBar();
                LoadDataIntoDataGrid();
            } catch(Exception ex)
            {
                MessageBox.Show("" + ex);
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

                    double amount = Convert.ToDouble(amountTxtBox.Text);
                    double foodCalories = CalculateCalories(foodInput.Text, amount);

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
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AddCalories_Click2(object sender, RoutedEventArgs e)
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

                    double amount = Convert.ToDouble(amountTxtBox.Text);
                    double foodCalories = CalculateCalories(caloriesInput2.Text, 1);

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
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void checkCalBtn_Click(object sender, RoutedEventArgs e)
        {
            double amount = Convert.ToDouble(amountTxtBox.Text);
            double foodCalories = CalculateCalories(foodInput.Text, amount);
            calLbl.Content = "Calories: " + foodCalories;

        }
        

        private double CalculateCalories(string foodName, double amount)
        {
            double foodCalories = 0.0;
            double servingSize = 0.0;

            using (SqlConnection conn = con.getCon())
            {
                conn.Open();

                // Use a parameterized query to retrieve the calorie value for the given food
                using (SqlCommand sqlCmd = new SqlCommand("SELECT calPerServing, servingSize FROM FoodData WHERE foodName = @foodName", conn))
                {
                    sqlCmd.Parameters.AddWithValue("@foodName", foodName);

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            foodCalories = reader.GetDouble(0);
                            servingSize = reader.GetDouble(1);
                        }
                    }
                }
            }

            return (foodCalories / servingSize) * amount;
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



        public void SetLabel2()
        {
            try
            {

                using (conn = con.getCon())
                {
                    conn.Open();
                    string query2 = $"SELECT * FROM calorieHistory WHERE userName = '{_name}'";
                    using (cmd = new SqlCommand(query2, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            caloriesLabel.Content = "Your Calorie Intake: "+reader["calories"];

                        }

                    }
                }
            }
            catch (Exception ex) { }
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = con.getCon())
            {
                conn.Open();
                SqlCommand selectCmd = new SqlCommand("SELECT progressBarValue FROM progressBar;", conn);
                int currentValue = Convert.ToInt32(selectCmd.ExecuteScalar());
                SqlCommand updateCmd = new SqlCommand($"UPDATE progressBar SET progressBarValue = {currentValue - newCalories};", conn);
                updateCmd.ExecuteNonQuery();
                conn.Close();

                updateProgressBar();
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = con.getCon())
                {
                    conn.Open();

                    // Use a parameterized query to reset the calorie count to 0 for the given user
                    SqlCommand updateCmd = new SqlCommand("UPDATE calorieHistory SET calories = @calories WHERE userName = @name", conn);
                    updateCmd.Parameters.AddWithValue("@calories", 0);
                    updateCmd.Parameters.AddWithValue("@name", _name);
                    updateCmd.ExecuteNonQuery();

                    conn.Close();

                    updateProgressBar();
                }
            }
            catch (Exception ex)
            {
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
                // apply a filter to the dataview based on the search text
                dataView.RowFilter = $"foodname like '%{searchText}%'";
            }
            else
            {
                // clear the filter if the search text is empty
                dataView.RowFilter = string.Empty;
            }

            //update the datagrid with the filtered data
            foodTable1.ItemsSource = dataView;
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

       

        private void EnterCaloriesTxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
                amountTxtBox.Text = "";
            
        }

        private void EnterCaloriesTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(amountTxtBox.Text))
            {
                amountTxtBox.Text = "Amount";
            }
        }
        private void searchTxtBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTxtBox1.Text == "Search Food")
            {
                foodInput.Text = "";
            }
        }

        private void search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(foodInput.Text))
            {
                foodInput.Text = "Search Food";
            }
        }

        private void foodTable1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calLbl.Content = "Calories: ";
            try
            {
                if (foodTable1.SelectedItem != null)
                {
                    DataRowView selectedRow = (DataRowView)foodTable1.SelectedItem;

                    string name = selectedRow["foodName"]?.ToString();
                    string measurement = selectedRow["measurement"]?.ToString(); // Use ?. for Measurement as well

                    foodInput.Text = name ?? "Food"; // Use a default value if 'name' is null
                    amountTxtBox.Text = " Input by " + measurement;// The "Input by" part is removed for clarity
                   
                }
                else
                {
                    foodInput.Text = "Food";
                    amountTxtBox.Text = "Amount";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }


        //Water

        public int newWater;
        private void AddWater_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = con.getCon())
            {
                conn.Open();

                if (int.TryParse(inputWater.Text, out newCalories))
                {
                    SqlCommand selectCmd = new SqlCommand($"SELECT waterML FROM CalorieAndWaterHistory where userName='{_name}';", conn);
                    int currentValue = Convert.ToInt32(selectCmd.ExecuteScalar());

                    int updatedValue = currentValue + newCalories;

                    SqlCommand updateCmd = new SqlCommand($"UPDATE CalorieAndWaterHistory SET waterML = {updatedValue} where userName = '{_name}';", conn);
                    updateCmd.ExecuteNonQuery();

                    conn.Close();

                    updateWaterProgressBar();
                    SetLabel3();
                }
            }
        }
        public void updateWaterProgressBar()
        {
            try
            {
                using (conn = con.getCon())
                {

                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT waterML FROM CalorieAndWaterHistory where userName = '{_name}", conn);
                    object result = cmd.ExecuteScalar();
                    if (result != null && double.TryParse(result.ToString(), out double progressValue))
                    {
                        waterProgress.Value = progressValue;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as displaying an error message or logging the error.
            }
        }
        private void BtnClearWater_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = con.getCon())
            {
                conn.Open();


                SqlCommand updateCmd = new SqlCommand($"UPDATE CalorieAndWaterHistory SET waterML = {0};", conn);
                updateCmd.ExecuteNonQuery();

                conn.Close();

                updateWaterProgressBar();
                SetLabel3();

            }
        }

        private void EnterWaterTxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (inputWater.Text == "Water Amount...")
            {
                inputWater.Text = "";
            }
        }

        private void EnterWaterTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputWater.Text))
            {
                inputWater.Text = "Water Amount...";
            }
        }


        public void SetLabel3()
        {
            try
            {

                using (conn = con.getCon())
                {
                    conn.Open();
                    string query2 = $"SELECT waterML FROM CalorieAndWaterHistory where userName = {_name};";
                    using (cmd = new SqlCommand(query2, conn))
                    using (SqlDataReader reader2 = cmd.ExecuteReader())
                    {

                        if (reader2.Read())
                        {
                            waterLabel.Content = reader2["waterProgress"] + " mL";

                        }



                    }
                }
            }
            catch (Exception ex) { }
        }

        




        /*
        private void UpdateProgressBar()
        {
            progress.Value = caloriesConsumed;
        }

        private void UpdateCaloriesLabel()
        {
            caloriesLabel.Content = $"Calories Consumed: {caloriesConsumed}";
        }
        */
    }
}

       
    

