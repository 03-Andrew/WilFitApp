using ControlzEx.Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WilFitApp.DataBase;

namespace WilFitApp.zPages
{
    /// <summary>
    /// Interaction logic for WorkoutPage.xaml
    /// </summary>
    public partial class WorkoutPage : Page
    {
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        DateTime time1;
        string _name;
        bool otherSelected = false;
        public WorkoutPage(string name)
        {

            InitializeComponent();
            addData();
            MyComboBox.SelectedItem = MyComboBox.Items[0];
            _name = name;
            LoadDataIntoDataGrid();
            setLable();
        }
        public void addData()
        {
            List<string> workoutTypes = new List<string>();
            

            try
            {
                // Assuming conn is defined as SqlConnection
                using (conn = con.getCon())
                {
                    conn.Open();
                    string query = "SELECT WorkoutType FROM WorkoutTypes";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                workoutTypes.Add(reader["WorkoutType"].ToString());
                            }
                        }
                    }

                }

                // Assuming MyComboBox is the name of your ComboBox control in XAML
                MyComboBox.ItemsSource = workoutTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedExercise = MyComboBox.SelectedItem as string;
            if (selectedExercise.Equals("Other"))
            {
                otherTxtBox.Visibility = Visibility.Visible;
                otherTxtBlock.Visibility = Visibility.Visible;
                categoryTxtBox.Text = " ";
                otherSelected = true;
            }
            else
            {
                otherTxtBox.Visibility = Visibility.Collapsed;
                otherTxtBlock.Visibility = Visibility.Collapsed;
                // Get the category for the selected exercise
                string category = GetCategoryForExercise(selectedExercise);

                // Update the TextBox with the selected category
                categoryTxtBox.Text = category;
                otherSelected = false;
            }
           
        }

        private string GetCategoryForExercise(string exercise)
        {
            string category = string.Empty;

            try
            {
                // Assuming conn is defined as SqlConnection
                using (SqlConnection conn = con.getCon())
                {
                    conn.Open();
                    string query = "SELECT WorkoutCategory FROM WorkoutTypes WHERE WorkoutType = @Exercise";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Exercise", exercise);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            category = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return category;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = datePicker.SelectedDate;  // Use nullable DateTime to handle null selection
            try
            {
                if (ValidateTimeInput() && selectedDate.HasValue)
                {

                    string type = otherSelected ? otherTxtBox.Text : MyComboBox.SelectedItem.ToString();
                    string category = categoryTxtBox.Text;
                    string duration = durationTxtBox.Text;
                    string description = descriptionTxtBox.Text;
                    DateTime date1 = DateTime.ParseExact(selectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    time1 = DateTime.ParseExact(timeTxtBox.Text, "HH:mm", CultureInfo.InvariantCulture);
                    string dateStr = date1.ToString("yyyy-MM-dd");
                    string timeStr = time1.ToString("HH:mm");

                    
                    using (SqlConnection conn = con.getCon())
                    {
                        conn.Open();
                        string query = $"INSERT INTO {_name}_workoutLog VALUES ('{dateStr}', '{timeStr}', '{type}', '{category}', '{duration}', '{description}')";
                        MessageBox.Show("HEHE");
                        using (cmd = new SqlCommand(query, conn))
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Success");
                            LoadDataIntoDataGrid();
                        }
                    }   
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error1: " + ex);
            }
            
        }

        private bool ValidateTimeInput()
        {
            string input = timeTxtBox.Text;
            if (Regex.IsMatch(input, @"^([01]?[0-9]|2[0-3]):[0-5][0-9]$"))
            { 
                return true;
            }
            else
            {
                // Invalid time format, handle the error (e.g., show a message to the user).
                MessageBox.Show("Invalid time format. Please use hh:mm.");
                timeTxtBox.Clear(); // Clear the TextBox
                return false;
            }
        }

        private void LoadDataIntoDataGrid()
        {

            try
            {
                string query = $"SELECT * FROM {_name}_workoutLog";

                using (conn = con.getCon())
                {
                    conn.Open();
                    cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    WorkoutLog.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
        private void setLable()
        {
            using (conn = con.getCon())
            {
                conn.Open();
                string query = $"SELECT weight, goalWeight FROM UserInfo WHERE userName = '{_name}'";
                using (cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currWeightLbl.Content = reader["weight"].ToString() + "kg";
                        goalWeightLbl.Content = reader["goalWeight"].ToString() + "kg";
                        
                    }
                }
            }
        }
        private void updateWeight_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateNow = DateTime.Now;
            string formattedDate = dateNow.ToString("yyyy-MM-dd");
            double updatedWeight = 0; 
            try
            {
                updatedWeight = Convert.ToDouble(newWeight.Text);
                string query1 = $"Update UserInfo set weight = {updatedWeight} where userName = '{_name}'";
                string query2 = $"Insert into WeightDB values('{_name}','{formattedDate}',{updatedWeight})";
                using(conn = con.getCon())
                {
                    conn.Open();
                    using (cmd = new SqlCommand(query1, conn))
                    {

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Weight updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No records were updated. Check if the date exists in your database.");
                        }
                    }
                    using (cmd = new SqlCommand(query2, conn))
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Success");
                        setLable();
                    }
                }
               
            } 
            catch 
            {
                MessageBox.Show("Input a valid number");
            }


        }
    }
}
