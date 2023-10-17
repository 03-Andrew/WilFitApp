using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WilFitApp.MVVM.View;

namespace WilFitApp
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Window
    {

        string _name;
        public DashBoard(string name)
        {
            InitializeComponent();
            HomeView homeView = new HomeView();
            homeView.SetLabelText(name);
            _name = name;
            MainFrame.Content = new zPages.HomePage();
            //homeViewContainer.Content = homeView;
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

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
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
            MainFrame.Content = new zPages.FoodPage(_name);
        }

        private void WorkoutLog_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new zPages.WorkoutPage(_name);
        }

        private void Workout_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new zPages.DataGridPage(_name);
        }
    }
}
