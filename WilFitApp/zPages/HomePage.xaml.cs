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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WilFitApp.zPages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }
        private double caloriesConsumed = 0;
        private void AddCalories_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(caloriesInput.Text, out double caloriesToAdd))
            {
                caloriesConsumed += caloriesToAdd; // Add the specified calories to the total consumed
                UpdateProgressBar();
                UpdateCaloriesLabel();
                caloriesInput.Clear(); // Clear the input TextBox
            }
            else
            {
                MessageBox.Show("Please enter a valid number of calories.");
            }
        }

        private void UpdateProgressBar()
        {
            progressBar.Value = caloriesConsumed;
        }

        private void UpdateCaloriesLabel()
        {
            caloriesLabel.Content = $"Calories Consumed: {caloriesConsumed}";
        }
    }
}
