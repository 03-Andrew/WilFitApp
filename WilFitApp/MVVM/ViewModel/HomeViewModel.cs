using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WilFitApp.Core;
using WilFitApp.Services;

namespace WilFitApp.MVVM.ViewModel
{
    internal class HomeViewModel : ObservbleObject
    {
        public HomeViewModel() 
        {
             
        }
       
    }
}

/*
       private INavigationService _navigation;

       public INavigationService Navigation 
       {
           get => _navigation;
           set 
           {
               _navigation = value;
               OnPropertyChanged();
           }
       }
       public RelayCommand NavigateToFoodCommand { get; set; }
       public RelayCommand NavigateToWorkoutCommand { get; set; }
       public HomeViewModel(INavigationService navService) 
       { 
           Navigation = navService;
           NavigateToFoodCommand = new RelayCommand(o => { Navigation.NavigateTo<FoodViewModel>(); }, o => true);
           NavigateToWorkoutCommand = new RelayCommand(o => { Navigation.NavigateTo<WorkoutViewModel>(); }, o => true);
       }
       */
