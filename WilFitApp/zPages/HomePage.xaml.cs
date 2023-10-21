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
        public HomePage()
        {
            InitializeComponent();
            updateProgressBar();
            SetLabel2();
        }
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        private void AddCalories_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = con.getCon())
            {
                conn.Open();

                if (int.TryParse(caloriesInput.Text, out int newCalories))
                {
                    SqlCommand selectCmd = new SqlCommand("SELECT progressBarValue FROM progressBar;", conn);
                    int currentValue = Convert.ToInt32(selectCmd.ExecuteScalar());

                    int updatedValue = currentValue + newCalories;

                    SqlCommand updateCmd = new SqlCommand($"UPDATE progressBar SET progressBarValue = {updatedValue};", conn);
                    updateCmd.ExecuteNonQuery();

                    conn.Close();

                    updateProgressBar();                }
            }
        }



        public void updateProgressBar()
        {
            try
            {
                using (conn = con.getCon())
                {

                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT progressBarValue FROM progressBar;", conn);
                    object result = cmd.ExecuteScalar();
                    if (result != null && double.TryParse(result.ToString(), out double progressValue))
                    {
                        progress.Value = progressValue;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as displaying an error message or logging the error.
            }
        }


        public void SetLabel2()
        {
            try
            {

                using (conn = con.getCon())
                {
                    conn.Open();
                    string query2 = $"SELECT progressBarValue FROM progressBar";
                    using (cmd = new SqlCommand(query2, conn))
                    using (SqlDataReader reader2 = cmd.ExecuteReader())
                    {

                        if (reader2.Read())
                        {
                            caloriesLabel.Content = "Your Calorie Intake: "+reader2["progressBarValue"];

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
