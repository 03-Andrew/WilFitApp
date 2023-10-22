using ControlzEx.Standard;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using WilFitApp.DataBase;
using WilFitApp.MVVM.Model;

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
        DataTable dataTable;
        DateTime time1;
        string _name;
        bool otherSelected = false;
        string type;
        WorkoutLog log;
        private DataRowView selectedRow;

        public WorkoutPage(string name)
        {
            try
            {
                InitializeComponent();
                addData();
                MyComboBox.SelectedItem = MyComboBox.Items[0];
                _name = name;
                LoadDataIntoDataGrid();
                setLable();
            }
            catch (Exception e){
                MessageBox.Show("" + e);
            }
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
            string query = $"SELECT * FROM {_name}_workoutLog";

            using (conn = con.getCon())
            {
                conn.Open();
                cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                dataTable = new DataTable();
                adapter.Fill(dataTable);
                WorkoutLog.ItemsSource = dataTable.DefaultView;
            }
            
        }

        private void setLable()
        {
            using (conn = con.getCon())
            {
                conn.Open();
                string query = $"SELECT weight, goalWeight, goal FROM UserInfo WHERE userName = '{_name}'";
                using (cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currWeightLbl.Content = reader["weight"].ToString() + "kg";
                        goalWeightLbl.Content = reader["goalWeight"].ToString() + "kg";
                        goal.Content = reader["goal"].ToString();
                        
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
                        newWeight.Text = "";
                    }
                }
               
            } 
            catch 
            {
                MessageBox.Show("Input a valid number");
            }
        }

        private void updateGoalWeight_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                double updatedGoalWeight = Convert.ToDouble(newGoalWeight.Text);
                string query1 = $"Update UserInfo set goalWeight = {updatedGoalWeight} where userName = '{_name}'";
                using (conn = con.getCon())
                {
                    conn.Open();
                    using (cmd = new SqlCommand(query1, conn))
                    {

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Weight updated successfully.");
                            setLable();
                            newGoalWeight.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("No records were updated. Check if the date exists in your database.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Input a valid number: " + ex);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTxtBox.Text.ToLower();

            // Use a DataView to filter the DataTable
            DataView dataView = dataTable.DefaultView;

            if (!string.IsNullOrEmpty(searchText))
            {
                // Apply a filter to the dataView based on the search text
                dataView.RowFilter = $"workoutType LIKE '%{searchText}%'";
            }
            else
            {
                // Clear the filter if the search text is empty
                dataView.RowFilter = string.Empty;
            }

            // Update the DataGrid with the filtered data
            WorkoutLog.ItemsSource = dataView;
        }

        private void WorkoutLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WorkoutLog.SelectedItem != null)
            {
                // Assuming you have a class 'Person' with Name and Age properties.
                DataRowView selectedRow = (DataRowView)WorkoutLog.SelectedItem;

                type = selectedRow["id"]?.ToString();
                rowLbl.Content = "Selected item: " + type;
            }
            else
            {
                // Handle the case where no item is selected (clear the label)
                rowLbl.Content = "Selected Item: ";
            }

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = datePicker.SelectedDate;  // Use nullable DateTime to handle null selection
            try
            {
                if (!ValidateTimeInput() && !selectedDate.HasValue)
                {
                    return;  
                }
                DateTime date1 = DateTime.ParseExact(selectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                time1 = DateTime.ParseExact(timeTxtBox.Text, "HH:mm", CultureInfo.InvariantCulture);
                log = new WorkoutLog();
                log.workoutType = otherSelected ? otherTxtBox.Text : MyComboBox.SelectedItem.ToString();
                log.workoutCategory = categoryTxtBox.Text;
                log.duration = durationTxtBox.Text;
                log.date = date1.ToString("yyyy-MM-dd");
                log.time = time1.ToString("HH:mm");
                log.description = descriptionTxtBox.Text;

                using (SqlConnection conn = con.getCon())
                {
                    conn.Open();
                    string query = $"INSERT INTO {_name}_workoutLog VALUES ('{log.date}', '{log.time}', '{log.workoutType}'," +
                        $" '{log.workoutCategory}', '{log.duration}', '{log.description}')";
                    MessageBox.Show(log.time);
                    MessageBox.Show("HEHE");
                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Success");
                        LoadDataIntoDataGrid();
                        clearEntries();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error1: " + ex);
            }

        }
        

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(type == null)
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", 
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            using (conn = con.getCon())
            {
                conn.Open();
                string query = $"delete from {_name}_workoutLog where id = {type}";
                using (cmd = new SqlCommand(query, conn))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected < 0)
                    {
                        MessageBox.Show("Row not found or could not be deleted.");
                        return;
                        
                    }
                    MessageBox.Show("Row deleted successfully.");
                    LoadDataIntoDataGrid(); // Refresh the DataGrid after deletion.
                }
            }          
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            string vals = "";
            foreach (DataColumn column in dataTable.Columns)
            {
                //MessageBox.WriteLine(column.ColumnName);
                vals += column.ColumnName + "\n";
            }
            MessageBox.Show(vals);
            try
            {
                if (WorkoutLog.SelectedItem == null)
                {
                    return;
                }
                selectedRow = (DataRowView)WorkoutLog.SelectedItem;
                string selectedWorkoutType = selectedRow["workoutType"]?.ToString();

                // Populate input controls with data from the selected row for editing
                MyComboBox.SelectedItem = MyComboBox.Items.Contains(selectedWorkoutType) ? selectedWorkoutType : "Other";
                otherTxtBox.Text = (MyComboBox.SelectedItem as string == "Other") ? selectedWorkoutType : "";
                categoryTxtBox.Text = selectedRow["workoutCategory"]?.ToString();
                durationTxtBox.Text = selectedRow["durationMinutes"]?.ToString();
                descriptionTxtBox.Text = selectedRow["description"]?.ToString();

                string dateString = selectedRow["dateOfWorkout"]?.ToString();
                if (DateTime.TryParse(dateString, out DateTime date))
                {
                    datePicker.SelectedDate = date;
                }
                else
                {
                    datePicker.SelectedDate = null; // Handle invalid date gracefully
                }

                // Parse and set the time
                string timeString = selectedRow["timeOfWorkout"]?.ToString().Substring(0, 5);
                
                MessageBox.Show("Retrieved time: " + timeString);
                if (DateTime.TryParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
                {
                    timeTxtBox.Text = time.ToString("HH:mm");
                }
                else
                {
                    timeTxtBox.Text = ""; // Handle invalid time gracefully
                }

                enterLog.Visibility = Visibility.Collapsed;



            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = datePicker.SelectedDate;  // Use nullable DateTime to handle null selection
            try
            {
                if (!ValidateTimeInput() && !selectedDate.HasValue)
                {
                    return;
                }
                DateTime date1 = DateTime.ParseExact(selectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                time1 = DateTime.ParseExact(timeTxtBox.Text, "HH:mm", CultureInfo.InvariantCulture);
                log = new WorkoutLog();
                log.workoutType = otherSelected ? otherTxtBox.Text : MyComboBox.SelectedItem.ToString();
                log.workoutCategory = categoryTxtBox.Text;
                log.duration = durationTxtBox.Text;
                log.date = date1.ToString("yyyy-MM-dd");
                log.time = time1.ToString("HH:mm");
                log.description = descriptionTxtBox.Text;

                using (SqlConnection conn = con.getCon())
                {
                    conn.Open();
                    string query = $"UPDATE {_name}_workoutLog " +
                                   $"SET dateOfWorkout = '{log.date}', timeOfWorkout = '{log.time}', workoutType = '{log.workoutType}', " +
                                   $"workoutCategory = '{log.workoutCategory}', durationMinutes = '{log.duration}', description = '{log.description}' " +
                                   $"WHERE id = {selectedRow["id"]}";

                    using (cmd = new SqlCommand(query, conn))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected < 0)
                        {
                            MessageBox.Show("Row not found or could not be updated.");
                            return;
                        }
                        MessageBox.Show("Row updated successfully.");
                        enterLog.Visibility = Visibility.Visible;
                        LoadDataIntoDataGrid();
                        clearEntries();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }

        }
        

        private void clearEntries()
        {
            MyComboBox.SelectedItem = MyComboBox.Items[0];
            categoryTxtBox.Text = "";
            datePicker.Text = "";
            timeTxtBox.Text = "hh:mm";
            durationTxtBox.Text = "";
            otherTxtBox.Text = "";
            descriptionTxtBox.Text = "";
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if (timeTxtBox.Text == "hh:mm")
            {
                timeTxtBox.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(timeTxtBox.Text))
            {
                timeTxtBox.Text = "hh:mm";
            }
        }

    }
}

//string type = otherSelected ? otherTxtBox.Text : MyComboBox.SelectedItem.ToString();
//string category = categoryTxtBox.Text;
//string duration = durationTxtBox.Text;
//string description = descriptionTxtBox.Text;
//DateTime date1 = DateTime.ParseExact(selectedDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
//time1 = DateTime.ParseExact(timeTxtBox.Text, "HH:mm", CultureInfo.InvariantCulture);
//string dateStr = date1.ToString("yyyy-MM-dd");
//string timeStr = time1.ToString("HH:mm");