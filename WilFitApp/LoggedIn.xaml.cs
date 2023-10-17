using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WilFitApp
{
    /// <summary>
    /// Interaction logic for LoggedIn.xaml
    /// </summary>
    public partial class LoggedIn : Window
    {
        string name;
        public LoggedIn(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Close();
            DashBoard db = new DashBoard(name);
            db.Show();
            this.Close();


        }
        public string GetName()
        {

            return name;
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
    }
}
