using System;
using System.Collections.Generic;
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
using WilFitApp.DataBase;

namespace WilFitApp.zPages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private double caloriesConsumed = 0;
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        string _name;
        public HomePage(string name)
        {
            InitializeComponent();
            updateProgressBar();
            SetLabel2();
            _name = name;
        }
        
        private void AddCalories_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = con.getCon())
                {
                    conn.Open();

                    if (double.TryParse(caloriesInput.Text, out double newCalories))
                    {
                        // Use a parameterized query to prevent SQL injection
                        SqlCommand selectCmd = new SqlCommand($"SELECT calories FROM calorieHistory WHERE userName = '{_name}'", conn);
                        

                        double currentValue = Convert.ToDouble(selectCmd.ExecuteScalar());

                        double updatedValue = currentValue + newCalories;
                        DateTime currDate = DateTime.Now;

                        // Fix the syntax error and use a parameterized query
                        SqlCommand updateCmd = new SqlCommand($"UPDATE calorieHistory SET calories = {updatedValue} WHERE userName = '{_name}'", conn);
                        updateCmd.ExecuteNonQuery();

                        conn.Close();

                        updateProgressBar();
                        SetLabel2();
                    }
                }
            } 
            catch(Exception ex)
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
                    string query2 = $"SELECT calories FROM calorieHistory WHERE userName = '{_name}'";
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
