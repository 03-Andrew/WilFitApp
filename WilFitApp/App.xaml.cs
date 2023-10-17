using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WilFitApp.MVVM.ViewModel;
using WilFitApp.MVVM.View;
using WilFitApp.Core;
using WilFitApp.Services;

namespace WilFitApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
       
        public App()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<FoodViewModel>();
            services.AddSingleton<WorkoutViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            //services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));
            _serviceProvider = services.BuildServiceProvider();
        }
    }
}

