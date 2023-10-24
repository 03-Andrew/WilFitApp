using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using WilFitApp.DataBase;
using WilFitApp.MVVM.Model;
using WilFitApp.MVVM.View;

namespace WilFitApp
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Window
    {

        string _name;
        calorieHistory cal = new calorieHistory();
        connection con = new connection();
        SqlConnection conn;
        SqlCommand cmd;
        public DashBoard(string name)
        {
            InitializeComponent();
            HomeView homeView = new HomeView();
            homeView.SetLabelText(name);
            _name = name;
            setCalories();
            MainFrame.Content = new zPages.HomePage1(_name);
            //homeViewContainer.Content = homeView;
        }

        public void setCalories()
        {
            using (SqlConnection conn = con.getCon())
            {
                conn.Open();
                SqlCommand selectCmd = new SqlCommand($"SELECT calories FROM calorieHistory WHERE userName = @_name", conn);
                selectCmd.Parameters.AddWithValue("_name", _name);

                object result = selectCmd.ExecuteScalar();
                double currentValue = Convert.ToDouble(result);
                cal.calories = currentValue;
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;
                    MainBorder.CornerRadius = new CornerRadius(20);
                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    MainBorder.CornerRadius = new CornerRadius(0);
                    IsMaximized = true;
                    

                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();   
        }

        private void HomeButtonClick(object sender, RoutedEventArgs e)
        {
           MainFrame.Content = new zPages.HomePage1(_name);
        }

        private void Food_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new zPages.HomePage(_name, cal.calories);
        }

        private void WorkoutLog_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new zPages.HomePage(_name, cal.calories);
        }

        private void Workout_Btn_Click(object sender, RoutedEventArgs e)
        {
            
            MainFrame.Content = new zPages.WorkoutPage(_name);
        }

        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
