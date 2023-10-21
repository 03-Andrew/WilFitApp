using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Data;
using System.Data.Sql;

namespace WilFitApp
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        //SqlDataReader reader;
        public Register()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;
                    Border1.CornerRadius = new CornerRadius(30);
                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;
                    Border1.CornerRadius = new CornerRadius(0);

                }
            }
        
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            getUserInfo();
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
            
        }

        private void generateNewTable()
        {
            string tableName = $"{username_txtBox.Text}_workoutLog";
            string createTableQuery = $@"CREATE TABLE {tableName} (
                                         ID INT IDENTITY(1,1) PRIMARY KEY,
                                         dateOfWorkout DATE,
                                         timeOfWorkout TIME,
                                         workoutType VARCHAR(50),
                                         workoutCategory VARCHAR(50),
                                         durationMinutes varchar(20),
                                         description VARCHAR(200))";
            using (conn = con.getCon())
            using (cmd = new SqlCommand(createTableQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }

                MessageBox.Show($"Table '{tableName}' created successfully.");
        }

        private void PackIconMaterial_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void getUserInfo()
        {
            try
            {
                string userName = username_txtBox.Text;
                string fullName = fullname_txtBox.Text;
                string password = password_txtBox.Password;
                string cPassword = cpassword_txtBox.Password;
              
                double weight = Convert.ToDouble(weight_txtBox.Text);
                double weightG = Convert.ToDouble(aimedWright_txtBox.Text);
                string goal = weight < weightG ? "gain" : "lose";
                double height = Convert.ToDouble(height_txtBox.Text);
               
                double waterIntake = weight * 0.033;

                double caloriesNeeded;
                double level = 0;
                string selectedLevel = "";

                int age = Convert.ToInt16(age_txtBox.Text);
                string gender = male_radioBtn.IsChecked == true ? "Male" : "Female";
                
                Dictionary<RadioButton, (string level, double factor)> activityLevels = new Dictionary<RadioButton, (string level, double factor)>
                {
                    { Sedentary_RadioBtn, ("Sedentary", 1.2) },
                    { LightlyActive_RadioBtn, ("Lightly Active", 1.375) },
                    { ModActive_RadioBtn, ("Moderately Active", 1.55) },
                    { Active_RadioBtn, ("Active", 1.725) },
                    { VActive_RadioBtn, ("Very Active", 1.9) }
                };

                foreach (var kvp in activityLevels)
                {
                    if (kvp.Key.IsChecked == true)
                    {
                        selectedLevel = kvp.Value.level;
                        level = kvp.Value.factor;
                        break; // Exit the loop once a match is found
                    }
                }

                //  MessageBox.Show($"You selected {selectedLevel}.");
                double bmr = CalculateBMR(gender, weight, height, age);
                caloriesNeeded = bmr * level;


                if (password != cPassword)
                {
                    MessageBox.Show("Confirm Passowrd and Passoword are not eqaul");
                    return;
                }
                using(conn = con.getCon())
                {
                    conn.Open();
                    string checkUserQuery = $"SELECT COUNT(*) FROM UserInfo WHERE userName = '{userName}'";
                    using (cmd = new SqlCommand(checkUserQuery, conn))
                    {
                        int existingUserCount = (int)cmd.ExecuteScalar();
                        if (existingUserCount > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.");
                        }
                        else
                        {
                            //Store values in the database
                            using (SqlCommand cmd = new SqlCommand($"INSERT INTO UserInfo VALUES('{fullName}', '{userName}', '{password}', {age}, '{gender}', {weight}, " +
                                            $"{weightG}, {height} ,{caloriesNeeded}, {waterIntake}, '{selectedLevel}', '{goal}')", conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                            MessageBox.Show($"Hi {userName}, you need {caloriesNeeded} calories, Registration complete");
                            generateNewTable();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid inputs: " + ex );
            }

        }

        private double CalculateBMR(string gender, double weight, double height, int age)
        {
            if (gender == "Male")
            {
                return 66.47 + (13.75 * weight) + (5.003 * height) - (6.755 * age);
            }
            return 655.1 + (9.563 * weight) + (1.850 * height) - (4.676 * age);
        }
    }
}


/*
 * conn = con.getCon();
                    conn.Open();
                    cmd = new SqlCommand($"insert into UserInfo values('{fullName}','{userName}','{password}',{age},'{gender}',{weight}," +
                                                                     $"{weightG},{height},{caloriesNeeded},{waterIntake},'{selectedLevel}')", conn);
                    cmd.BeginExecuteNonQuery();
                    
                    MessageBox.Show($"Hi {userName}, you need {caloriesNeeded} calories, Registration complete");
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                    cmd.Dispose();
                    conn.Close();
 */
/*
if (Sedentary_RadioBtn.IsChecked == true)
{
    selectedLevel = "Sedentary";
    level = Convert.ToDouble(1.2);
}
else if (LightlyActive_RadioBtn.IsChecked == true)
{
    selectedLevel = "Lightly Active";
    level = Convert.ToDouble(1.375);

}
else if (ModActive_RadioBtn.IsChecked == true)
{
    selectedLevel = "Moderately Active";
    level = Convert.ToDouble(1.55);
}
else if (Active_RadioBtn.IsChecked == true)
{
    selectedLevel = "Active";
    level = Convert.ToDouble(1.725);
}
else if (VActive_RadioBtn.IsChecked == true)
{
    selectedLevel = "Very Active";
    level = Convert.ToDouble(1.9);

}
*/