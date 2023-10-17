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
    /// Interaction logic for HomePage1.xaml
    /// </summary>
    public partial class HomePage1 : Page
    {
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        string _name;
        public HomePage1(string name)
        {
            InitializeComponent();
            _name = name;
            greetLbl.Content = _name;
            SetLabels();
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
                    using (SqlDataReader reader = cmd.ExecuteReader())
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



    }
}
