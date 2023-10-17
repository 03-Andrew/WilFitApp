using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WilFitApp.DataBase;

namespace WilFitApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader reader;
        string usedName;
        public MainWindow()
        {
            InitializeComponent();
        }
        public string GetUsedName()
        {
            return this.usedName;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }



        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Register reg = new Register();
            reg.Show();
            this.Close();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                using (conn = con.getCon())
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM [dbo].[UserInfo] WHERE userName=@userName AND password=@password";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@userName", UserNameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@password", PasswordTextBox.Password);

                    int count = (int)cmd.ExecuteScalar();
                    usedName = UserNameTxtBox.Text;


                    if (count > 0)
                    {
                        // Successful login
                        LoggedIn li = new LoggedIn(usedName);

                        li.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        // Invalid login, handle appropriately (e.g., show an error message).
                        MessageBox.Show("Invalid username or password. Please try again.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        

        private void TogglePasswordVisibility_Click_1(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = (ToggleButton)sender;
            if (toggleButton.IsChecked == true)
            {
                VisualStateManager.GoToState(toggleButton, "Checked", true);
            }
            else
            {
                VisualStateManager.GoToState(toggleButton, "Normal", true);
            }
        }
        private bool IsMaximized = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 600;
                    this.Height = 450;
                    Border1.CornerRadius = new CornerRadius(30);
                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    Border1.CornerRadius = new CornerRadius(0);
                    IsMaximized = true;
                    

                }
            }
        }




    }


}
