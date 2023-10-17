using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WilFitApp.Core;

namespace WilFitApp.MVVM.ViewModel
{
    class MainViewModel : ObservbleObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand FoodViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public FoodViewModel FoodVm { get; set; }
        private object _currentView;

		public object CurrentView
		{
			get { return _currentView; }
			set 
			{
				_currentView = value;
				OnPropertyChanged();
			}
		}
        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            FoodVm = new FoodViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o => 
            { 
                CurrentView = HomeVM;
            });
            FoodViewCommand = new RelayCommand(o =>
            {
                CurrentView = FoodVm;
            });
        }

    }
}
