using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Main;
using tabApp.Core.ViewModelsWear;
using Xamarin.Essentials;

namespace tabApp.Core
{
    public class AppStart : MvxAppStart
    {

        private readonly IMvxNavigationService _navigationService;

        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
            : base(app, mvxNavigationService)
        {
            _navigationService = mvxNavigationService;
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Watch)
            {
                return _navigationService.Navigate<MainViewModelWear>();
            }

            return _navigationService.Navigate<MainViewModel>();
        }
    }
}
