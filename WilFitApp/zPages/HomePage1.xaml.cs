using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    /// Interaction logic for HomePage1.xaml
    /// </summary>
    public partial class HomePage1 : Page
    {
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader reader;
        string _name;
        public HomePage1(string name)
        {
            InitializeComponent();
            _name = name;
            greetLbl.Content = _name;
            SetLabels();
            updateProgressBar();

            SetLabel2();
        }
        public void SetLabels()
        {
            try
            {
                using (conn = con.getCon())
                {
                    conn.Open();
                    string query = $"SELECT weight, fullName, goalWeight, caloriesNeeded, recommendedWater, ActivityLevel FROM UserInfo WHERE userName = '{_name}'";
                    using (cmd = new SqlCommand(query, conn))
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            weightLbl.Content = reader["weight"].ToString() + "kg";
                            greetLbl.Content = "Hello " + reader["fullName"].ToString();
                            goalWeightLbl.Content = reader["goalWeight"].ToString() + "kg";
                            activityLvl.Content = reader["ActivityLevel"].ToString();
                            recommendedWaterLbl.Content = reader["recommendedWater"].ToString() + "L";
                            caloriesLbl.Content = string.Format("{0:0.00}", Convert.ToDouble(reader["caloriesNeeded"]));
                            
                        }
                            
                    }
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
                    using (reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            foodBarStat.Content = reader["progressBarValue"];
                            
                        }

                    }
                }
            }catch (Exception ex) { }
        }
        public void updateProgressBar()
        {
            try
            {
                using (conn = con.getCon())
                {
                   
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT calories FROM calorieHistory;", conn);
                    object result = cmd.ExecuteScalar();
                    if (result != null && double.TryParse(result.ToString(), out double progressValue))
                    {
                       calorieProgressBar.Value = progressValue;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as displaying an error message or logging the error.
            }
        }



    }
}
