using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.ViewModels;

namespace tabApp.Core.ViewModelsClient
{
    public class HomePageViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;

        public HomePageViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override async void Appearing()
        {
            await _navigationService.Navigate<HomepageViewModelClient>();
        }

        public override void DisAppearing()
        {
        }
    }
}
